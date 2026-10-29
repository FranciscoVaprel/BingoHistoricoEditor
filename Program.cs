using BingoHistoricoEditor.Data;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BingoHistoricoEditor {
    internal static class Program {
        public static IConfiguration Configuration { get; private set; } = null!;

        private static readonly string AppSettingsPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

        [STAThread]
        static void Main() {
            Configuration = CargarConfig();
            ApplicationConfiguration.Initialize();

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

        private static IConfiguration CargarConfig() =>
            new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

        /// <summary>
        /// Obtiene el valor de PrecioPartida desde la configuración
        /// </summary>
        public static decimal ObtenerPrecioPartida() {
            string valorStr = Configuration["BingoSettings:PrecioPartida"] ?? "0.50";
            if (decimal.TryParse(valorStr, out decimal valor)) {
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

        private static void GuardarJson(JsonObject root) =>
            File.WriteAllText(AppSettingsPath, root.ToJsonString(JsonWriteOpts));
    }
}
