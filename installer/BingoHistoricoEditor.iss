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

#define MyAppVersion "1.6.4"
#define MyAppPublisher "Vaprel"
#define MyAppExeName "BingoHistoricoEditor.exe"
#define MyPublishDir "..\bin\Release\net10.0-windows\publish\win-x64"
; Nombre, GUID y carpeta se pueden cambiar con /D al compilar solo para probar
; el instalador sin tocar la instalación real del PC.
#ifndef MyAppName
  #define MyAppName "Bingo Histórico Editor"
#endif
#ifndef MyAppGuid
  #define MyAppGuid "C0EA4B0A-A730-4D69-9507-141A44598B0A"
#endif
#ifndef DirPorDefecto
  #define DirPorDefecto "{sd}\BingoHistoricoEditor"
#endif

[Setup]
; No cambiar este identificador entre versiones: es lo que permite que un
; setup.exe posterior reconozca la instalación anterior y la actualice, en vez
; de instalar una copia nueva en paralelo.
AppId={{{#MyAppGuid}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
VersionInfoVersion={#MyAppVersion}
; C:\BingoHistoricoEditor, como Gregal en C:\Gregal. Hasta la 1.6.1 era
; %LOCALAPPDATA%\Programs\BingoHistoricoEditor: al actualizar desde ahí,
; PrepareToInstall mueve los datos a la carpeta nueva y desinstala la vieja
; (por eso no se reutiliza la carpeta de la instalación anterior).
DefaultDirName={#DirPorDefecto}
UsePreviousAppDir=no
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
; Instalación por usuario (no requiere ser administrador ni UAC). En la raíz
; de C:\ cualquier usuario puede crear carpetas, y Windows da permiso de
; modificación en ellas a todos los usuarios: la app puede escribir su
; configuración, usuarios, logs y la BD local dentro de su propia carpeta.
PrivilegesRequired=lowest
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
OutputDir=Output
OutputBaseFilename=BingoHistoricoEditor-Setup-{#MyAppVersion}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
SetupIconFile=..\BingoHistoricoEditor.ico
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

; Lo usa el acceso directo "Actualizar": busca una versión nueva en GitHub y
; la instala con este mismo setup en silencio.
Source: "Buscar-Actualizacion.ps1"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\Desinstalar {#MyAppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
; Siempre, también al actualizar: así una sala ya instalada lo tiene tras su
; próxima actualización.
Name: "{autodesktop}\Actualizar {#MyAppName}"; Filename: "{sys}\WindowsPowerShell\v1.0\powershell.exe"; Parameters: "-NoProfile -ExecutionPolicy Bypass -File ""{app}\Buscar-Actualizacion.ps1"""; WorkingDir: "{app}"; IconFilename: "{app}\{#MyAppExeName}"; Comment: "Buscar e instalar una versión nueva"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Ejecutar {#MyAppName}"; Flags: nowait postinstall skipifsilent

; Nota sobre desinstalación: appsettings.json lleva el flag "uninsneveruninstall"
; en [Files], así que el desinstalador lo deja en su sitio. users.json, logs\ y
; BBDD\ nunca se declaran como ficheros del instalador (los crea la app en
; tiempo de ejecución), así que tampoco los toca el desinstalador.

[Code]
// Carpeta de la instalación anterior de este usuario ('' si no hay).
function CarpetaAnterior: String;
begin
  Result := '';
  if RegQueryStringValue(HKCU, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{' + '{#MyAppGuid}' + '}_is1',
                         'InstallLocation', Result) then
    Result := RemoveBackslash(Result);
end;

// True si el fichero está abierto por otro proceso (p. ej. la app en marcha).
function EnUso(const Fichero: String): Boolean;
var
  F: TFileStream;
begin
  Result := False;
  if not FileExists(Fichero) then Exit;
  try
    F := TFileStream.Create(Fichero, fmOpenReadWrite or fmShareExclusive);
    F.Free;
  except
    Result := True;
  end;
end;

// Mueve un fichero o carpeta de datos de la instalación anterior a la nueva.
// Si ya existe en la nueva, no se pisa (se queda el de la vieja, que el
// desinstalador no borra). Devuelve False si no se pudo mover.
function Mover(const Origen, Destino, Nombre: String): Boolean;
begin
  Result := True;
  if not FileExists(Origen + '\' + Nombre) and not DirExists(Origen + '\' + Nombre) then Exit;
  if FileExists(Destino + '\' + Nombre) or DirExists(Destino + '\' + Nombre) then Exit;
  Result := RenameFile(Origen + '\' + Nombre, Destino + '\' + Nombre);
  if Result then
    Log('Movido ' + Origen + '\' + Nombre + ' -> ' + Destino)
  else
    Log('No se pudo mover ' + Origen + '\' + Nombre);
end;

// Actualización desde otra carpeta (la de usuario de hasta la 1.6.1): se
// llevan la configuración, los usuarios, los logs y la BD local (BBDD\, a la
// que apunta la ruta relativa por defecto) a la carpeta nueva, y se
// desinstala la copia vieja (binarios, accesos directos y registro).
function PrepareToInstall(var NeedsRestart: Boolean): String;
var
  Anterior, Nueva, Nombre: String;
  FindRec: TFindRec;
  Codigo: Integer;
  Nombres: TArrayOfString;
  I: Integer;
begin
  Result := '';
  Anterior := CarpetaAnterior;
  Nueva := ExpandConstant('{app}');
  if (Anterior = '') or (CompareText(Anterior, Nueva) = 0) or not DirExists(Anterior) then Exit;

  if EnUso(Anterior + '\{#MyAppExeName}') then
  begin
    Result := 'Cierra {#MyAppName} y vuelve a ejecutar el instalador: hay que mover sus datos de ' +
              Anterior + ' a ' + Nueva + '.';
    Exit;
  end;

  ForceDirectories(Nueva);
  Nombres := ['appsettings.json', 'users.json', 'logs', 'BBDD'];
  if FindFirst(Anterior + '\users.json.bak-*', FindRec) then
  begin
    try
      repeat
        SetArrayLength(Nombres, GetArrayLength(Nombres) + 1);
        Nombres[GetArrayLength(Nombres) - 1] := FindRec.Name;
      until not FindNext(FindRec);
    finally
      FindClose(FindRec);
    end;
  end;

  for I := 0 to GetArrayLength(Nombres) - 1 do
  begin
    Nombre := Nombres[I];
    if not Mover(Anterior, Nueva, Nombre) then
    begin
      Result := 'No se pudo mover ' + Anterior + '\' + Nombre + ' a ' + Nueva + '.' + #13#10#13#10 +
                'Si es la base de datos (BBDD), puede que Firebird la tenga abierta: cierra los programas ' +
                'que la usen y vuelve a ejecutar el instalador.';
      Exit;
    end;
  end;

  if FileExists(Anterior + '\unins000.exe') then
    Exec(Anterior + '\unins000.exe', '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART', '', SW_HIDE, ewWaitUntilTerminated, Codigo);
end;
