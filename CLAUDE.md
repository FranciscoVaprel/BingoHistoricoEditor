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

**The build needs the Gregal repo next to this one** (`C:\CSHARP\Gregal`): the csproj has a
`ProjectReference` to `..\Gregal\Gregal\Gregal.Protocolo\Gregal.Protocolo.csproj`, the class library
with Gregal's tag-protocol parser (used by `ImportarLogForm`). Its tests live in Gregal
(`Gregal.Tests/ReproductorLogGregalTests.cs`). Its `Gregal.Protocolo.dll` is copied into the publish
output like any other dependency.

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
- `MainForm` — two tabs above the grid (`tabTablas`): "HISTORICO" and the import table
  (`BingoSettings:TablaImportacionLog`, default `HISTORICO_LOG`). `_tablaActual` decides what
  `CargarPartidasAsync` loads; the log tab is **read-only**: the editing/lote panel
  (`tabControlEdicion`) is disabled because all of it writes `HISTORICO`, double-click opens
  `FichaPartidaForm` with `soloConsultaDe`, and `btnPasarVista` opens `PasarLogAHistoricoForm`.
  After an import the screen jumps to that tab and day (`ImportarLogForm.UltimaSesionImportada`).
  The grid is bound once to a fixed `BindingSource` (`_bsPartidas`) and loads are serialized
  (`_cargando`/`_recargarAlTerminar`): rebinding with `DataSource = null` while another load (startup,
  BUSCAR, tab change) was in flight threw "Index was out of range" / "auto-filled column is being
  resized" and left blank rows — don't go back to that pattern.
- `MainForm` (cont.) — loads `HISTORICO` rows for a selected date/sala/concesionario into a grid,
  supports single-record edit/save/delete and batch (lote) operations over a `PARTIDA_DIA` range
  (search for gaps, batch save, batch delete), backed by `nudPartidaInicial`/`nudPartidaFinal`.
- `FichaPartidaForm` — "edit every column" window for one `HISTORICO` row, opened from `MainForm`
  (`btnEditarFicha` or double-click on a grid row). Built in code, not with the designer, because its
  fields come from the database: `FirebirdService.ObtenerColumnasHistoricoAsync` reads
  `RDB$RELATION_FIELDS`/`RDB$FIELDS` once and maps each column to a `ColumnaHistorico`
  (`TipoColumna`, scale, limits, max length, nullable, `CHAR` padding). One `EditorCampo` per column
  (label + control by type + "vacío" checkbox for NULL); the 99 `PREMIOn` go in a grid instead
  (`FilaPremio`). Tab assignment and the known column descriptions live in `FichaPartidaCampos` — an
  unknown column still works, in "Otros". The four key columns sit above the tabs and **are editable**
  (user's decision). Things to preserve:
  - "Modificado" compares against what the control showed on open (`FichaPartidaCampos.Normalizar`:
    numbers as `decimal`, `CHAR` trimmed, times to the second), not the raw DB value — otherwise
    trailing spaces or sub-second times would show as edits nobody made. Amounts are `NUMERIC(15,5)`
    in the venues' databases but are shown with 2 decimals unless they really have more.
  - `ActualizarCamposAsync` writes only the changed columns, with `col IS NOT DISTINCT FROM @original`
    in the `WHERE` (optimistic concurrency: 0 rows → "la partida ha cambiado", nothing saved).
  - A key change is **copy-with-new-key + delete-original** in one transaction (`MoverPartidaAsync`),
    never an `UPDATE` of the key: the venue's `HISTORICO_AI`/`HISTORICO_AD` triggers log each
    inserted/updated or deleted key to `LOGREPLICACION` for replication, and an `UPDATE` would never
    log the old key as gone. The target key is checked first (also inside the transaction).
  - The form doesn't log; `MainForm.AbrirFichaAsync` logs `DB_UPDATE_FICHA` with every field's
    before/after from `CambiosGuardados`.
- `PasarLogAHistoricoForm` — second entry of the "⇩ LOG DE GREGAL ▾" menu: compares one session day of
  the import table with `HISTORICO` (`FirebirdService.CompararConHistoricoAsync`: same columns, normalized
  like the ficha) and copies the ticked games into `HISTORICO` (`PasarAHistoricoAsync`, one transaction).
  Missing games are ticked by default except "dudosas" (`RECLAMACION` "PARTIDA A MEDIAS…" or 0 sold).
  For ticked games that already exist with different data, it **asks** (TaskDialog) whether to replace
  the whole row or only fill the gaps (hueco = NULL/0/empty in `HISTORICO` and a value in the log) —
  the user's choice. Updates go through `ActualizarSiNoHaCambiadoAsync` (same optimistic
  `IS NOT DISTINCT FROM` check as the ficha) and inserts re-check the key, because Gregal may be writing
  that day live. Writes do fire `HISTORICO`'s triggers (replication, audit) on purpose: this is real
  data now. The import table is never modified. Available to every user (user's choice). Logged as
  `DB_LOG_A_HISTORICO` with each field's before/after.
- `ImportarLogForm` — first entry of the header menu "⇩ LOG DE GREGAL ▾": rebuilds **all** the games in a Gregal log
  (`gregal<AAAAMMDD>.log`) and saves them to a separate table, never `HISTORICO`:
  `BingoSettings:TablaImportacionLog`, default `HISTORICO_LOG` (`FirebirdService.TablaImportacionPorDefecto`).
  Games are rebuilt by Gregal's own code (`Gregal.Parsing.ReproductorLogGregal`, replaying the
  `Fragmento tag recibido` lines through `PartidaAccumulator` with each line's timestamp as the clock),
  and written with Gregal's own `Gregal.Data.FilaHistorico` (same columns, same 0/NULL defaults, same
  `UPDATE OR INSERT`) — don't reimplement either here; change them in Gregal. Sala/concesionario come
  from this app's `CodSala`/`CodConc`; the manufacturer (`MCB`/`Metronia`, changes how `A`/`M`/`p`/`z`/`y`
  are read) is chosen in the form, starts unselected, and is remembered in `BingoSettings:FabricanteLog`.
  `FirebirdService.CrearTablaComoHistoricoAsync` creates the target table from `HISTORICO`'s own
  metadata (`ColumnaHistorico.TipoSql`, same NOT NULLs and primary key) **without triggers**, so imported
  rows never reach the venue's replication (`LOGREPLICACION`) or `HISTORICO`'s audit trigger.
  `ImportarPartidasAsync` writes all games in one transaction; a key already in the table is
  overwritten, and a number repeated within the log keeps the last one (as Gregal did). Each import is
  logged as `IMPORTAR_LOG_GREGAL` via `MainForm.EscribirLog`.
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
