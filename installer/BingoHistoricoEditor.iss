; Script de Inno Setup para Bingo Histórico Editor.
;
; Genera un instalador/actualizador único: si la app ya está instalada, el mismo
; setup.exe detecta la instalación existente (gracias a AppId) y actualiza los
; binarios sin tocar la configuración ya hecha en el ordenador de la sala.
;
; Requisitos antes de compilar:
;   1. Generar el build de publicación:
;      dotnet publish ..\BingoHistoricoEditor.csproj -c Release -p:PublishProfile=FolderProfile
;   2. Revisar a mano la carpeta de publicación (bin\...\publish\win-x64\) y asegurarse
;      de que NO contiene appsettings.json/users.json/logs reales de tu máquina de
;      desarrollo (solo la plantilla sin credenciales) — ver diario.md, entrada del
;      primer release, donde se explica por qué esto importa.
;   3. Compilar este script con ISCC.exe (o abrirlo con el IDE de Inno Setup).
;
; IMPORTANTE: si cambias el número de versión, actualiza MyAppVersion aquí Y
; <Version> en BingoHistoricoEditor.csproj para que coincidan.

#define MyAppName "Bingo Histórico Editor"
#define MyAppVersion "1.3.0"
#define MyAppPublisher "Vaprel"
#define MyAppExeName "BingoHistoricoEditor.exe"
#define MyPublishDir "..\bin\Release\net10.0-windows\publish\win-x64"

[Setup]
; No cambiar este identificador entre versiones: es lo que permite que un
; setup.exe posterior reconozca la instalación anterior y la actualice en el
; mismo sitio, en vez de instalar una copia nueva en paralelo.
AppId={{C0EA4B0A-A730-4D69-9507-141A44598B0A}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
VersionInfoVersion={#MyAppVersion}
DefaultDirName={autopf}\BingoHistoricoEditor
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
; Instalación por usuario (no requiere ser administrador ni UAC): evita
; problemas de permisos de escritura, ya que la app guarda su configuración,
; usuarios y la base de datos local dentro de su propia carpeta de instalación.
PrivilegesRequired=lowest
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
OutputDir=Output
OutputBaseFilename=BingoHistoricoEditor-Setup-{#MyAppVersion}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "Crear un acceso directo en el Escritorio"; GroupDescription: "Accesos directos adicionales:"; Flags: unchecked

[Files]
; Binarios de la app: se sobrescriben siempre, tanto en instalación como en
; actualización. Se excluyen explícitamente los ficheros de datos/config del
; operador, por si algún día se colasen en la carpeta de publicación.
Source: "{#MyPublishDir}\*"; DestDir: "{app}"; Excludes: "appsettings.json,users.json,logs\*,BBDD\*"; Flags: ignoreversion recursesubdirs createallsubdirs

; appsettings.json: SOLO se copia si todavía no existe (instalación nueva).
; En una actualización, el fichero real del operador (con su contraseña de BD,
; código de sala, etc.) se queda exactamente como estaba.
Source: "{#MyPublishDir}\appsettings.json"; DestDir: "{app}"; Flags: onlyifdoesntexist uninsneveruninstall

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\Desinstalar {#MyAppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Ejecutar {#MyAppName}"; Flags: nowait postinstall skipifsilent

; Nota sobre desinstalación: appsettings.json lleva el flag "uninsneveruninstall"
; en [Files], así que el desinstalador lo deja en su sitio. users.json, logs\ y
; BBDD\ nunca se declaran como ficheros del instalador (los crea la app en
; tiempo de ejecución), así que tampoco los toca el desinstalador.
