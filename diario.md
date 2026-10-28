# Diario de cambios

Registro cronológico de los cambios relevantes hechos en el proyecto (qué se cambió y por qué). Se añade una entrada nueva arriba del todo cada vez que se hace un cambio significativo.

---

## 2026-10-28

- **Instalador/actualizador con Inno Setup que no sobreescribe la configuración**: pedido explícitamente porque un instalador normal sobreescribiría `appsettings.json` del ordenador de la sala en cada actualización. Nuevo `installer/BingoHistoricoEditor.iss`:
  - Usa un `AppId` fijo (GUID), lo que permite que el mismo `setup.exe` sirva tanto para instalar como para actualizar: si detecta una instalación previa con ese `AppId`, actualiza en el mismo sitio en vez de instalar una copia en paralelo.
  - `appsettings.json` se instala con `Flags: onlyifdoesntexist uninsneveruninstall` — solo se copia la plantilla si el fichero no existe ya; en una actualización (o al desinstalar) la configuración real del operador se queda intacta. El resto de binarios se sobrescriben siempre (`ignoreversion`).
  - `users.json`, `logs\` y `BBDD\` nunca forman parte del paquete del instalador (los crea la propia app en tiempo de ejecución), así que tampoco los toca nunca.
  - Instalación por usuario (`PrivilegesRequired=lowest`), sin necesitar permisos de administrador ni UAC — evita problemas de permisos de escritura en la carpeta de instalación.
  - **Probado manualmente** (no solo compilado a ciegas): instalación en una carpeta temporal, edición a mano de `appsettings.json` simulando la configuración real del operador, y reinstalación sobre la misma carpeta — se confirmó que `DbPass`/`CodSala` editados sobreviven intactos y el `.exe` se sustituye con normalidad.
  - Compilado con Inno Setup 6 (`ISCC.exe`, instalado en esta sesión vía `winget install JRSoftware.InnoSetup` porque no estaba presente). El `.exe` generado (`installer/Output/*.exe`) no se versiona en git (añadido a `.gitignore`): se publica como asset del Release de GitHub, igual que el `.zip`. Subido `BingoHistoricoEditor-Setup-1.0.0.exe` al Release `v1.0.0` ya existente.
  - **Aviso repetido, esta vez con una vuelta de tuerca**: al generar el build de publicación para este instalador, se volvió a colar en `bin/.../publish/win-x64/appsettings.json` un fichero con `AppUser`/`AppPasswordHash` reales (no el de desarrollo con la contraseña de Firebird esta vez, sino uno creado por una ejecución accidental de la app en una carpeta de prueba). Se detectó y se corrigió antes de compilar el instalador final. Confirma la regla ya anotada: **revisar siempre `bin/.../publish/.../` a mano antes de empaquetar nada para distribuir**, nunca asumir que sigue limpio de una vez para otra.

- **Repositorio subido a GitHub y primer Release publicado**: el remoto `origin` apuntaba a `fralurbe/BingoHistoricoEditor`, que no existía ni era accesible con la cuenta autenticada (`gh auth login` → `FranciscoVaprel`). Se creó el repositorio público `github.com/FranciscoVaprel/BingoHistoricoEditor`, se actualizó el remoto y se subieron los 25 commits que había en local.
  - Añadido `<Version>1.0.0</Version>` al `.csproj` (no tenía ninguno antes).
  - Generado un build de publicación self-contained win-x64 con el `FolderProfile.pubxml` ya existente, empaquetado como `BingoHistoricoEditor-v1.0.0-win-x64.zip` y publicado como asset del tag/Release `v1.0.0` (no se versiona en git, solo vive en la pestaña Releases).
  - **Importante, revisado antes de subir nada**: el build de publicación copió por defecto el `appsettings.json` real de desarrollo (con la contraseña de Firebird y el hash de usuario reales) y también un `users.json` y una carpeta `logs/` con datos reales de pruebas, todos generados/presentes localmente. Se sustituyeron/eliminaron a mano en la carpeta de publicación antes de empaquetar el zip, dejando solo la plantilla `appsettings.example.json` sin credenciales. Si se vuelve a publicar en el futuro, revisar siempre el contenido de `bin/.../publish/.../` antes de comprimir y subir nada.



- **Corregido crash al seleccionar una partida fuera de los límites habituales de la UI** (p. ej. `PARTIDA_DIA = 0`, o un precio/premio superior al máximo del control): `MainForm.grid_SelectionChanged` asignaba `_seleccionada.PartidaDia`/`Precio`/`Vendidos`/`Premio1`/`Premio2` directamente a los `NumericUpDown` (`nudPartida`, `nudPrecio`, `nudVendidos`, `nudLinea`, `nudBingo`, `nudPartidaInicial`, `nudPartidaFinal`) sin comprobar límites. Como `nudPartida.Minimum = 1`, si ya existiera en la BD una partida con `PARTIDA_DIA = 0` (insertada por otra vía, p. ej. SQL directo, ya que la UI no deja crear una así) al seleccionarla en la rejilla `NumericUpDown.Value` lanzaba `ArgumentOutOfRangeException` y la app se caía. Añadido `AsignarAcotado(control, valor)`, que acota el valor a `[Minimum, Maximum]` antes de asignarlo, usado en los siete controles de ese método.
  - Nota: sigue sin ser posible **crear** una partida 0 desde la UI (`nudPartida.Minimum = 1` se mantiene); este arreglo cubre el caso de que ya exista una así por otra vía.

---

## 2026-09-30

- **Selector de fichero .fdb para Ruta Relativa/Absoluta**: en `SettingsForm`, junto a esos dos campos hay ahora un botón "…" que abre un `OpenFileDialog` filtrado a `*.fdb`. Al elegir un fichero: para Ruta Absoluta se usa la ruta completa tal cual; para Ruta Relativa se calcula con `Path.GetRelativePath` respecto a la carpeta de la aplicación. El diálogo permite seleccionar una ruta que todavía no exista (`CheckFileExists = false`), porque Firebird crea el `.fdb` la primera vez que se conecta si no está ya creado.

- **Campos de conexión a BD editables en `SettingsForm`**: cambio de diseño explícitamente pedido — hasta ahora Modo BD, Servidor, Puerto, Ruta Relativa/Absoluta/Remota, Usuario/Contraseña BD, Charset y Dialecto eran de solo lectura (documentado así en `CLAUDE.md`). Ahora son editables:
  - `cmbDbModo`/`cmbDbCharset` pasan a `ComboBoxStyle.DropDownList` (solo se puede elegir de la lista, no escribir libremente) para evitar valores inválidos.
  - `ValidarTodosLosValores` en `SettingsForm.cs` valida ahora también estos campos con `SettingsValidator` (Modo, Servidor, Puerto, Usuario/Contraseña/Charset/Dialecto BD), más una comprobación de coherencia nueva: en modo Local hace falta Ruta Absoluta o Relativa; en modo Remoto/Alias hace falta Ruta Remota.
  - `btnGuardar_Click` ya no pasa `null` para los campos de BD a `Program.GuardarConfiguracion`; se guardan de verdad.
  - Recuerda que, como ya pasaba con Código Sala/Conc, los cambios de BD requieren reiniciar la app (el connection string se construye una sola vez al crear `FirebirdService`).
  - `CLAUDE.md` actualizado para reflejar esto (ya no dice que la BD es "read-only in the UI by design"), y de paso corregida una frase desfasada que aún describía la escritura de `appsettings.json` como "string search/replace" en vez de `JsonNode`.

- **Corregido bug del doble clic en "Configuración"**: `btnConfiguracion.Click` estaba suscrito dos veces (una en `MainForm.Designer.cs` y otra, redundante, en el constructor de `MainForm.cs`). Por eso al cerrar la ventana de Configuración (con Cancelar o Guardar) se abría otra igual justo debajo: el cierre disparaba la segunda suscripción pendiente del mismo clic original. Eliminada la suscripción duplicada.

- **Tooltips explicativos en `SettingsForm`**: añadido un `ToolTip` con explicaciones al pasar el ratón por cada campo de la pestaña "Base de Datos" y "Aplicación" — en particular, aclara la prioridad real entre Ruta Relativa/Absoluta/Remota según el Modo BD (motivado por una pregunta sobre cuál de las dos rutas se estaba usando realmente), y deja claro que "Usuario App" ya no controla el login (eso lo gestiona `UserStore`).

---

## 2026-09-22

- **Corregido espaciado del diálogo "Verificar identidad"** (`AutenticacionPrevia` en `LoginForm.cs`): la etiqueta "Contraseña" quedaba casi pegada a la caja de texto de usuario (2 px de separación), controles demasiado apretados verticalmente. Se rehizo el layout con márgenes consistentes (~12 px entre etiqueta y campo anterior), se cambió `Size` por `ClientSize` (igual que en la pantalla de login, para que el borde/barra de título de Windows no recorte espacio) y se fijó explícitamente `Font = "Segoe UI", 9f` en el formulario, que antes no lo tenía.

- **Roles de usuario (admin) y pantalla de auditoría**: `UserEntry` incorpora `IsAdmin`. El usuario creado en la configuración inicial (primer arranque) es admin automáticamente; al añadir usuarios nuevos desde "Gestionar usuarios" se puede marcar la casilla "Es administrador". Nuevo `AuditoriaForm` (`MainForm` → botón "🛡 AUDITORÍA", solo visible para administradores) que lista el contenido de `operacionesBGEditor` usando `FirebirdService.ObtenerOperacionesAsync`.
  - `UserStore.AsegurarAdminExiste()` promueve al primer usuario existente si una instalación previa (sin roles) no tiene ningún admin, para no dejar la auditoría inaccesible tras la actualización.
  - `UserStore.EliminarUsuario` ahora impide borrar al último administrador restante (además de seguir impidiendo borrar al último usuario), para no perder el acceso a la auditoría.
  - Comprobación de rol duplicada a propósito: el botón se oculta si no eres admin, y el propio `btnAuditoria_Click` vuelve a comprobarlo antes de abrir el formulario (mismo patrón de "defensa en profundidad" que ya se usaba para las acciones sensibles de `UserManagerForm`).

- **Auditoría a nivel de base de datos**: nuevo trigger `TRG_HISTORICO_AUDITORIA` sobre `HISTORICO` (`AFTER INSERT OR UPDATE OR DELETE`) que inserta automáticamente una fila en `operacionesBGEditor` con los valores antes/después de cada cambio (`OPERACION` = `DB_TRIGGER_INSERT` / `DB_TRIGGER_UPDATE` / `DB_TRIGGER_DELETE`).
  - Por qué: hasta ahora el registro de auditoría solo lo escribía la aplicación (`LogService`) *después* de guardar; cualquiera que modificara `HISTORICO` con otra herramienta (IBExpert, `isql`, etc.) usando las credenciales de `appsettings.json` no dejaba ningún rastro. El trigger vive dentro de Firebird, así que salta siempre, venga el cambio de donde venga.
  - Se crea automáticamente al arrancar la app (`FirebirdService.CrearTriggerAuditoriaHistoricoAsync`, llamado desde `MainForm.OnLoad` justo después de `CrearTablaOperacionesAsync`), igual que ya se hacía con la tabla/generador/trigger de `operacionesBGEditor`.
  - Limitación conocida: como todos los usuarios de la app comparten las credenciales `SYSDBA` de Firebird, el campo `USUARIO` que graba el trigger (`CURRENT_USER`) será siempre `SYSDBA`, no el usuario de la app que hizo el cambio (eso sigue quedando registrado, con el usuario real, en las filas que ya inserta la propia app). Para poder atribuir también a nivel de BD haría falta dar a cada operador su propio usuario de Firebird en vez de una cuenta compartida.
  - **Pendiente de verificar contra un servidor Firebird real**: el trigger se ha escrito y compila como código C#, pero su sintaxis PSQL no se ha podido probar en este entorno (no hay Firebird disponible). Revisar el log de depuración (`Debug.WriteLine`) o `SELECT * FROM RDB$TRIGGERS WHERE RDB$TRIGGER_NAME = 'TRG_HISTORICO_AUDITORIA'` la primera vez que se arranque contra una base real.

- **Auditoría de aplicación ampliada con valores reales**: el campo `DETALLES` de `operacionesBGEditor` (y del log de fichero) ahora guarda los valores concretos de cada operación (precio, vendidos, premios, rango) en vez de quedar casi siempre vacío. En el borrado (individual y en lote) los valores se capturan *antes* de eliminar. De paso, el campo `OPERACION` dejó de contener la línea completa del log (fecha+usuario+texto) y pasa a ser solo el tipo de acción, ya que fecha/usuario tienen sus propias columnas.

- **Guardado en lote con transacción única**: `FirebirdService.GuardarLoteAsync` reemplaza el bucle que abría una conexión por partida; ahora usa una sola conexión + `FbTransaction` para todo el rango, con `Rollback` si algo falla a mitad.

- **Carga automática del último día con datos**: al arrancar, `MainForm` consulta `MAX(FECHA_SESION)` para la sala/concesionario configurados y coloca ahí el selector de fecha (antes siempre abría en el día de hoy, aunque no hubiera datos).

- **`appsettings.json` se escribe como JSON real**: `Program.GuardarCredenciales`/`GuardarConfiguracion` pasaron de buscar comillas y llaves a mano a usar `System.Text.Json.Nodes.JsonNode`. Ya no depende de asumir una estructura de anidación fija ni se rompe con valores que contengan comillas.

- **Corregida inconsistencia en `DbModo`**: el validador (`SettingsValidator.ValidateDbModo`) aceptaba `"Local"`/`"Remote"` (inglés) pero la resolución real de ruta (`FirebirdService.ResolverRuta`) esperaba `"Local"`/`"Remoto"`/`"Alias"` (español). Ahora ambos coinciden.

- **Corregido centrado del login**: al pasar la ventana de `FormBorderStyle.None` a `FixedDialog` (ver rediseño de colores), se seguía fijando `this.Size` en vez de `this.ClientSize`, así que el borde/barra de título de Windows descuadraba el panel central. Ahora se fija `ClientSize`.

- **Rediseño visual con colores de Windows**: sustituida la paleta oscura personalizada (RGB fijos) por `SystemColors` de Windows Forms en todos los formularios (`MainForm`, `LoginForm`, `SetPasswordForm`, `SettingsForm`, `UserManagerForm`) — fondos, cajas de texto, botones estándar del sistema, etc.

- **Corregido posible crash al arrancar sin BD disponible**: `FirebirdService.TestConnectionAsync` abría una conexión de prueba sin `try/catch` antes del bloque protegido; si Firebird no estaba accesible, la excepción se propagaba sin control. Simplificado a una única conexión protegida.

- **Gestión de usuarios sin autenticar (seguridad)**: desde la pantalla de login, sin escribir ninguna contraseña, se podía añadir o eliminar usuarios (`UserManagerForm`) — solo "cambiar contraseña" pedía verificación previa. Ahora las tres acciones sensibles (añadir, eliminar, cambiar contraseña) exigen `AutenticacionPrevia`.

- **`CLAUDE.md`**: creado con la arquitectura del proyecto (flujo de arranque, capa de datos, los dos sistemas de usuarios existentes —`UserStore` activo vs. `UsuarioManager` código muerto—, los tres destinos del log) para futuras sesiones de trabajo sobre este repo.

### Pendiente / sugerido, no implementado todavía

- Rol de Firebird con permisos mínimos en vez de usar `SYSDBA` para la conexión de la app (y sin permisos de `UPDATE`/`DELETE` sobre `operacionesBGEditor`, para que la auditoría no se pueda manipular ni desde la propia app).
- Eliminar código muerto: `UsuarioManager.cs`/`usuarios.json` (sustituido por `UserStore`) y `ConfirmDialog.cs`/`.Designer.cs` (no referenciado desde ningún sitio).
- Limpiar los avisos de compilación de nullable (`CS8600`/`CS8604`/`CS8618`/`CS8622`/`CS8625`) que ya existían antes de esta sesión.
