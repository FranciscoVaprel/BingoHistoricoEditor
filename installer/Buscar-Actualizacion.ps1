<#
    Busca en GitHub una versión de Bingo Histórico Editor más nueva que la
    instalada y, si la hay, la instala con BingoHistoricoEditor-Setup en modo
    silencioso. El instalador conserva appsettings.json, users.json, logs y
    BBDD.

    Lo lanza el acceso directo "Actualizar Bingo Histórico Editor" del
    escritorio que crea el instalador. No necesita permisos de administrador
    (la instalación es por usuario).

        .\Buscar-Actualizacion.ps1                 # comprueba y pregunta
        .\Buscar-Actualizacion.ps1 -SoloComprobar  # solo dice si hay versión nueva
#>
param([switch]$SoloComprobar)

$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"   # en PowerShell 5.1 la barra de progreso hace lentísima la descarga
$Repositorio = "FranciscoVaprel/BingoHistoricoEditor"
$Exe = "BingoHistoricoEditor.exe"
$InstallDir = $PSScriptRoot

function Write-Step($msg) { Write-Host "`n==> $msg" -ForegroundColor Cyan }
function Write-Warn($msg) { Write-Host "! $msg" -ForegroundColor Yellow }
function Write-Ok($msg) { Write-Host "OK $msg" -ForegroundColor Green }
function Salir([int]$Codigo) {
    if (-not $SoloComprobar) { Read-Host "`nPulsa Intro para cerrar esta ventana" | Out-Null }
    exit $Codigo
}

function Get-VersionInstalada([string]$Carpeta) {
    $ruta = Join-Path $Carpeta $Exe
    if (-not (Test-Path $ruta)) { return $null }
    # ProductVersion = "1.6.2+<commit>"
    return [version]((Get-Item $ruta).VersionInfo.ProductVersion.Split('+')[0])
}

Write-Step "Buscando actualizaciones de Bingo Histórico Editor"
$instalada = Get-VersionInstalada $InstallDir
if (-not $instalada) {
    Write-Warn "No se encuentra $Exe en '$InstallDir': este script tiene que estar en la carpeta del programa."
    Salir 1
}
Write-Host "Versión instalada: $instalada"

try {
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    $release = Invoke-RestMethod "https://api.github.com/repos/$Repositorio/releases/latest" -UseBasicParsing -TimeoutSec 30
}
catch {
    Write-Warn "No se pudo consultar GitHub (¿sin conexión a internet?): $($_.Exception.Message)"
    Salir 1
}
$ultima = [version]($release.tag_name.TrimStart('v'))
Write-Host "Última versión publicada: $ultima"

if ($ultima -le $instalada) {
    Write-Ok "Bingo Histórico Editor está al día."
    Salir 0
}

$setup = $release.assets | Where-Object { $_.name -like "BingoHistoricoEditor-Setup-*.exe" } | Select-Object -First 1
if (-not $setup) {
    Write-Warn "La versión $ultima no tiene instalador (BingoHistoricoEditor-Setup-*.exe) en GitHub."
    Salir 1
}
if ($SoloComprobar) {
    Write-Host "Hay una versión nueva: $ultima."
    exit 0
}

Write-Step "Novedades de la versión $ultima"
# Solo los cambios: las notas acaban con secciones para quien instala a mano
# (descargas, instalación, requisitos). Como mucho 25 líneas: el detalle
# está en GitHub.
$notas = foreach ($linea in ($release.body -split "`r?`n")) { if ($linea -match '^##\s*(Descargas|Instalaci|Requisitos)') { break }; $linea }
$notas | Select-Object -First 25 | ForEach-Object { Write-Host "  $_" }
Write-Host "  ($($release.html_url))"
Write-Host ""
Write-Host "La configuración, los usuarios y los datos no se tocan."
$respuesta = Read-Host "¿Actualizar Bingo Histórico Editor a la versión $ultima ahora? (S/n)"
if ($respuesta -match "^[nN]") { Salir 0 }

# El programa tiene que estar cerrado para sustituirlo. Se le pide que se
# cierre como si se pulsara la X, para que avise si hay cambios sin guardar.
$abierto = @(Get-Process BingoHistoricoEditor -ErrorAction SilentlyContinue)
if ($abierto.Count -gt 0) {
    Write-Step "Cerrando Bingo Histórico Editor"
    $abierto | ForEach-Object { [void]$_.CloseMainWindow() }
    foreach ($p in $abierto) { [void]$p.WaitForExit(60000) }
    if (@(Get-Process BingoHistoricoEditor -ErrorAction SilentlyContinue).Count -gt 0) {
        Write-Warn "Bingo Histórico Editor sigue abierto. Ciérralo y vuelve a pulsar 'Actualizar'."
        Salir 1
    }
}

Write-Step "Descargando $($setup.name)"
$destino = Join-Path $env:TEMP $setup.name
try {
    Invoke-WebRequest $setup.browser_download_url -OutFile $destino -UseBasicParsing -TimeoutSec 600
}
catch {
    Write-Warn "No se pudo descargar: $($_.Exception.Message)"
    Salir 1
}
if ((Get-Item $destino).Length -ne $setup.size) {
    Write-Warn "La descarga está incompleta ($((Get-Item $destino).Length) de $($setup.size) bytes). Vuelve a intentarlo."
    Remove-Item $destino -ErrorAction SilentlyContinue
    Salir 1
}
Write-Ok "Descargado."

Write-Step "Instalando la versión $ultima"
$log = Join-Path $env:TEMP "BingoHistoricoEditor-Setup-$ultima.log"
$proceso = Start-Process $destino -ArgumentList "/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /LOG=`"$log`"" -Wait -PassThru
Remove-Item $destino -ErrorAction SilentlyContinue
if ($proceso.ExitCode -ne 0) {
    Write-Warn "El instalador ha terminado con error ($($proceso.ExitCode)). Detalle en $log"
    Salir 1
}

# Las instalaciones de hasta la 1.6.1 estaban en la carpeta del usuario: el
# instalador las mueve a C:\BingoHistoricoEditor. La carpeta buena es la que
# queda registrada.
$carpeta = $InstallDir
$clave = "HKCU:\Software\Microsoft\Windows\CurrentVersion\Uninstall\{C0EA4B0A-A730-4D69-9507-141A44598B0A}_is1"
$registrada = (Get-ItemProperty $clave -ErrorAction SilentlyContinue).InstallLocation
if ($registrada) { $carpeta = $registrada.TrimEnd('\') }

$instalada = Get-VersionInstalada $carpeta
if ($instalada -eq $ultima) { Write-Ok "Bingo Histórico Editor actualizado a la versión $instalada." }
else { Write-Warn "El instalador terminó, pero la versión instalada es $instalada. Detalle en $log" }

Start-Process (Join-Path $carpeta $Exe) -WorkingDirectory $carpeta
Salir 0
