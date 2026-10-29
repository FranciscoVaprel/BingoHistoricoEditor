# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A Windows Forms (.NET, `net10.0-windows`) desktop tool for editing historical bingo game records
(`HISTORICO` table) in a Firebird database, with a secondary audit log of operations
(`operacionesBGEditor` table). Spanish is the language used throughout the UI, logs, and code
(variable/method names, comments, log messages).

## Build & run

```powershell
dotnet build BingoHistoricoEditor.slnx
dotnet run --project BingoHistoricoEditor.csproj
```

There is no test project in this repo — verification is done by running the WinForms app against a
real (or local) Firebird database.

## Installer

`installer/BingoHistoricoEditor.iss` is an Inno Setup script that builds a Windows installer from the
self-contained win-x64 publish output (`FolderProfile.pubxml`). Build order:

```powershell
dotnet publish BingoHistoricoEditor.csproj -c Release -p:PublishProfile=FolderProfile
# Inspect bin\Release\net10.0-windows\publish\win-x64\ by hand before compiling — see warning below.
& "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe" installer\BingoHistoricoEditor.iss
```

The compiled setup (`installer/Output/*.exe`, gitignored — ships as a GitHub Release asset, not
committed) doubles as the updater: it uses a fixed `AppId`, so running it again over an existing
install upgrades the binaries in place. `appsettings.json` is installed with `Flags: onlyifdoesntexist
uninsneveruninstall`, so a real config already on the venue PC is never touched by an update or an
uninstall; `users.json`/`logs/`/`BBDD/` are never part of the installer payload at all (the app creates
them at runtime), so they're untouched for the same reason.

**Before every publish, manually verify `bin/.../publish/win-x64/` has no real secrets** — the csproj's
`Content Include="appsettings.json"` condition means your own local `appsettings.json` (with its real
DB password) gets copied into the publish output if it's newer than what's already there, and a prior
app run in that same output folder can leave a real `users.json`/`logs/` behind too. Check with
`grep -rl "pbkdf2\$" bin/.../publish/win-x64/` (and inspect `appsettings.json` itself) before compiling
or zipping anything meant for distribution.

## Runtime dependencies

- Requires a running **Firebird 2.5+** server (`FirebirdServerDefaultInstance` service on Windows).
- Connects via `FirebirdSql.Data.FirebirdClient`.
- Local config lives in `appsettings.json` (ignored by Git); `appsettings.example.json` is the
  tracked template copied to the output directory when no local config exists. It contains DB
  connection mode/path/credentials, `CodSala`/`CodConc` (room/concessionaire codes used to scope
  every query), and `PrecioPartida` (default game price). Set the local Firebird password before
  connecting.

## Architecture

**Startup flow** (`Program.cs`): load config → if no `AppUser`/`AppPasswordHash` is set, run
`SetPasswordForm` first-run setup → `LoginForm` → `MainForm`. `Program` also owns the logic for
rewriting `appsettings.json` in place (`GuardarCredenciales`, `GuardarConfiguracion`), parsing it as
JSON via `System.Text.Json.Nodes.JsonNode`/`JsonObject` and writing it back with
`ToJsonString(WriteIndented)` — not hand-rolled string search/replace.

**Data access** (`Data/FirebirdService.cs`): the only class that talks to Firebird. It builds its own
connection string from `IConfiguration` (`BuildConnectionString`/`ResolverRuta`, supporting Local,
Remote, and Alias `DbModo` values), and exposes CRUD for `HISTORICO` (`CargarPorFechaAsync`,
`GuardarAsync` using `UPDATE OR INSERT`, `EliminarAsync`, range variants for batch operations) plus
self-provisioning of the audit table (`CrearTablaOperacionesAsync` creates the table, generator, and
BEFORE INSERT trigger for `operacionesBGEditor` if missing) and audit read/write methods
(`InsertarOperacionAsync`, `ObtenerOperacionesAsync`, ...). Every `HISTORICO` query is scoped by
`COD_SALA`/`COD_CONC`/`FECHA_SESION`; `PARTIDA_DIA` is the per-day game number.

**Two independent auth/audit trails exist — know which is live:**
- App login credentials: `UserStore` (`users.json`) is the active implementation used by
  `LoginForm`/`UserManagerForm`. `Program`/`LoginForm` migrate the single legacy
  `AppUser`/`AppPasswordHash` pair from `appsettings.json` into `users.json` on first login
  (`UserStore.MigrarDesdeAppSettings`).
  `UsuarioManager` (`usuarios.json`) is a separate, effectively unused parallel implementation
  (only self-referenced) — don't extend it; use `UserStore`.
- Operation logging: `Services/LogService.cs` writes structured JSON logs to `logs/operaciones-*.json`
  (via Serilog, one physical sink) **and** mirrors each entry into the `operacionesBGEditor` DB table
  (via `FirebirdService.InsertarOperacionAsync`, best-effort/non-throwing). `MainForm.EscribirLog`
  additionally appends human-readable lines to the in-form `txtLog` textbox — these three are separate
  destinations, not a single call.

Passwords (both app users and — historically — the legacy `AppUser`) are hashed with
`PasswordHelper` (PBKDF2-SHA256, salted, constant-time compare); never store or compare plaintext.

**Forms** (`*.Designer.cs` are generated by the WinForms designer — edit visual layout there only via
the designer or with care; put logic in the paired non-designer `.cs` partial):
- `SetPasswordForm` — first-run credential setup.
- `LoginForm` — login + entry point to `UserManagerForm` (also hosts `AutenticacionPrevia`, an inline
  re-auth dialog). Also hosts the lost-access recovery link (`lnkRestablecer` →
  `ConfirmarRestablecerForm`, type-to-confirm dialog): wipes `users.json` (backed up to
  `users.json.bak-<timestamp>`) and clears `AppUser`/`AppPasswordHash` from `appsettings.json`
  (`UserStore.EliminarTodosLosUsuarios` + `Program.LimpiarCredencialesIniciales`), then
  `Application.Restart()`s so the first-run wizard (`SetPasswordForm`) runs again. Deliberately *not*
  a hidden/hardcoded bypass credential — requires physical file-system access to the machine plus
  explicit confirmation in the UI; was requested as a "backdoor" and pushed back on for that reason.
- `MainForm` — main screen: loads `HISTORICO` rows for a selected date/sala/concesionario into a grid,
  supports single-record edit/save/delete and batch (lote) operations over a `PARTIDA_DIA` range
  (search for gaps, batch save, batch delete), backed by `nudPartidaInicial`/`nudPartidaFinal`.
- `SettingsForm` — edits all `BingoSettings` fields at runtime, including the DB connection ones
  (`DbModo`, `DbSource`, `DbPort`, `DbRelativePath`/`DbAbsolutePath`/`DbRemotePath`, `DbUser`,
  `DbPass`, `DbCharset`, `DbDialect`); these were previously read-only by design, but that was
  changed on request. `ToolTip`s on every field explain the mode/path precedence rules from
  `FirebirdService.ResolverRuta`. Saving DB fields requires an app restart to take effect (the
  connection string is built once, at `FirebirdService` construction).
- `UserManagerForm` — add/remove app users, change passwords, via `UserStore`. Users have an
  `IsAdmin` flag; only admins see/open `AuditoriaForm` (lists `operacionesBGEditor`) from `MainForm`.
- `AuditoriaForm` — read-only grid over `operacionesBGEditor`, admin-only.
- `ConfirmDialog` — generic Yes/No confirmation dialog, currently unreferenced dead code.

`SettingsValidator` centralizes field validation (ranges, allowed enums like charset/dialect) used by
`SettingsForm` before persisting.
