using BingoHistoricoEditor.Data;
using BingoHistoricoEditor.Services;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BingoHistoricoEditor {
    internal static class Program {
        // Una sola instancia para toda la aplicación: al guardar appsettings.json
        // se recarga (GuardarJson), y todos los que la tienen ven los valores nuevos.
        public static IConfigurationRoot Configuration { get; private set; } = null!;

        private static readonly string AppSettingsPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

        [STAThread]
        static void Main() {
            CapturarErroresNoControlados();
            Configuration = CargarConfig();
            ApplicationConfiguration.Initialize();
            PonerIconoEnVentanas();

            // Primer arranque: sin credenciales → mostrar setup
            string hashActual = Configuration["BingoSettings:AppPasswordHash"] ?? "";
            string usuarioActual = Configuration["BingoSettings:AppUser"] ?? "";

            if (string.IsNullOrWhiteSpace(hashActual) || string.IsNullOrWhiteSpace(usuarioActual)) {
                using var setup = new SetPasswordForm(primerArranque: true);
                if (setup.ShowDialog() != DialogResult.OK) return;

                GuardarCredenciales(setup.UsuarioGenerado, setup.HashGenerado);
                Configuration = CargarConfig();  // recargar con los nuevos valores
            }

            // Login
            using var login = new LoginForm(Configuration);
            if (login.ShowDialog() != DialogResult.OK) return;

            Application.Run(new MainForm(
                new FirebirdService(Configuration),
                Configuration,
                login.UsuarioLogueado));
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Red de seguridad: un fallo que se escapa de todos los try/catch queda
        /// anotado en logs\errores&lt;fecha&gt;.log (antes solo dejaba rastro en el
        /// Visor de eventos de Windows, que en una sala nadie mira).
        /// </summary>
        private static void CapturarErroresNoControlados() {
            // Fallo en un evento de una ventana (también los async void): se
            // anota, se avisa y el programa sigue, como ya hacía WinForms.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, e) => {
                LogService.Error("Excepción no controlada en la interfaz.", e.Exception);
                try {
                    MessageBox.Show($"Ha ocurrido un error inesperado:\n{e.Exception.Message}\n\n"
                        + "Queda anotado en la carpeta logs (errores<fecha>.log).",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch { }
            };
            // Fallo fuera del hilo de las ventanas: el proceso se va a cerrar.
            AppDomain.CurrentDomain.UnhandledException += (_, e) => {
                LogService.Fatal($"Excepción no controlada: el programa se va a cerrar (IsTerminating={e.IsTerminating}).",
                    e.ExceptionObject as Exception);
                LogService.Close();
            };
            // Una Task que falla sin que nadie espere su resultado: no cierra el
            // programa, pero el error se perdería sin más.
            TaskScheduler.UnobservedTaskException += (_, e) => {
                LogService.Error("Excepción no observada en una tarea en segundo plano.", e.Exception);
                e.SetObserved();
            };
        }

        /// <summary>
        /// Todas las ventanas llevan el icono del exe (ApplicationIcon) en vez
        /// del genérico de WinForms: se cambia el icono por defecto de Form, que
        /// es privado (s_defaultIcon), en vez de ponerlo ventana a ventana. Si
        /// una versión futura de WinForms lo renombra, simplemente se queda el
        /// genérico.
        /// </summary>
        private static void PonerIconoEnVentanas() {
            try {
                var icono = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                typeof(Form).GetField("s_defaultIcon",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    ?.SetValue(null, icono);
            }
            catch { }
        }

        private static IConfigurationRoot CargarConfig() =>
            new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

        /// <summary>
        /// Obtiene el valor de PrecioPartida desde la configuración
        /// </summary>
        public static decimal ObtenerPrecioPartida() {
            string valorStr = Configuration["BingoSettings:PrecioPartida"] ?? "0.50";
            if (SettingsValidator.TryParseDecimal(valorStr, out decimal valor)) {
                return valor;
            }
            return 0.50m;
        }

        /// <summary>
        /// Escribe usuario y hash en appsettings.json sin tocar el resto del fichero.
        /// </summary>
        public static void GuardarCredenciales(string usuario, string hash) {
            var root = CargarJson();
            var settings = ObtenerSeccionBingoSettings(root);

            settings["AppUser"] = usuario;
            settings["AppPasswordHash"] = hash;
            settings.Remove("AppPassword"); // eliminar contraseña en texto plano si existía

            GuardarJson(root);
        }

        /// <summary>
        /// Recuperación de acceso perdido: borra AppUser/AppPasswordHash de
        /// appsettings.json. Combinado con UserStore.EliminarTodosLosUsuarios(),
        /// fuerza que el próximo arranque muestre otra vez el asistente de
        /// primera configuración (ver el chequeo al inicio de Main()).
        /// </summary>
        public static void LimpiarCredencialesIniciales() {
            var root = CargarJson();
            var settings = ObtenerSeccionBingoSettings(root);

            settings.Remove("AppUser");
            settings.Remove("AppPasswordHash");

            GuardarJson(root);
        }

        /// <summary>
        /// Guarda todos los parámetros de configuración en appsettings.json
        /// </summary>
        public static void GuardarConfiguracion(
            string? dbModo = null,
            string? dbSource = null,
            string? dbPort = null,
            string? dbRelativePath = null,
            string? dbAbsolutePath = null,
            string? dbRemotePath = null,
            string? dbUser = null,
            string? dbPass = null,
            string? dbCharset = null,
            string? dbDialect = null,
            string? codSala = null,
            string? codConc = null,
            string? appUser = null,
            string? precioPartida = null) {

            var root = CargarJson();
            var settings = ObtenerSeccionBingoSettings(root);

            if (dbModo != null) settings["DbModo"] = dbModo;
            if (dbSource != null) settings["DbSource"] = dbSource;
            if (dbPort != null) settings["DbPort"] = dbPort;
            if (dbRelativePath != null) settings["DbRelativePath"] = dbRelativePath;
            if (dbAbsolutePath != null) settings["DbAbsolutePath"] = dbAbsolutePath;
            if (dbRemotePath != null) settings["DbRemotePath"] = dbRemotePath;
            if (dbUser != null) settings["DbUser"] = dbUser;
            if (dbPass != null) settings["DbPass"] = dbPass;
            if (dbCharset != null) settings["DbCharset"] = dbCharset;
            if (dbDialect != null) settings["DbDialect"] = dbDialect;
            if (codSala != null) settings["CodSala"] = ParseEntero(codSala, "CodSala");
            if (codConc != null) settings["CodConc"] = ParseEntero(codConc, "CodConc");
            if (appUser != null) settings["AppUser"] = appUser;
            if (precioPartida != null) settings["PrecioPartida"] = precioPartida;

            GuardarJson(root);
        }

        /// <summary>
        /// Recuerda en appsettings.json el último fabricante elegido al importar
        /// un log de Gregal (BingoSettings:FabricanteLog).
        /// </summary>
        public static void GuardarFabricanteLog(string fabricante) {
            var root = CargarJson();
            ObtenerSeccionBingoSettings(root)["FabricanteLog"] = fabricante;
            GuardarJson(root);
        }

        private static int ParseEntero(string valor, string campo) =>
            int.TryParse(valor, out int n)
                ? n
                : throw new FormatException($"{campo} debe ser un número entero: '{valor}'.");

        // ── Lectura/escritura de appsettings.json como JSON real ────────────────

        private static readonly JsonSerializerOptions JsonWriteOpts = new() { WriteIndented = true };

        private static JsonObject CargarJson() =>
            JsonNode.Parse(File.ReadAllText(AppSettingsPath))?.AsObject()
                ?? throw new InvalidOperationException("appsettings.json no contiene un objeto JSON válido.");

        private static JsonObject ObtenerSeccionBingoSettings(JsonObject root) {
            if (root["BingoSettings"] is not JsonObject settings) {
                settings = new JsonObject();
                root["BingoSettings"] = settings;
            }
            return settings;
        }

        private static void GuardarJson(JsonObject root) {
            File.WriteAllText(AppSettingsPath, root.ToJsonString(JsonWriteOpts));
            Configuration?.Reload();
        }
    }
}
