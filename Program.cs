using BingoHistoricoEditor.Data;
using Microsoft.Extensions.Configuration;

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
            string json = File.ReadAllText(AppSettingsPath);
            json = EscribirOActualizar(json, "AppUser", usuario);
            json = EscribirOActualizar(json, "AppPasswordHash", hash);
            // Eliminar AppPassword en texto plano si existía
            json = EliminarClave(json, "AppPassword");
            File.WriteAllText(AppSettingsPath, json);
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

            string json = File.ReadAllText(AppSettingsPath);

            if (dbModo != null) json = EscribirOActualizar(json, "DbModo", dbModo);
            if (dbSource != null) json = EscribirOActualizar(json, "DbSource", dbSource);
            if (dbPort != null) json = EscribirOActualizar(json, "DbPort", dbPort);
            if (dbRelativePath != null) json = EscribirOActualizar(json, "DbRelativePath", dbRelativePath);
            if (dbAbsolutePath != null) json = EscribirOActualizar(json, "DbAbsolutePath", dbAbsolutePath);
            if (dbRemotePath != null) json = EscribirOActualizar(json, "DbRemotePath", dbRemotePath);
            if (dbUser != null) json = EscribirOActualizar(json, "DbUser", dbUser);
            if (dbPass != null) json = EscribirOActualizar(json, "DbPass", dbPass);
            if (dbCharset != null) json = EscribirOActualizar(json, "DbCharset", dbCharset);
            if (dbDialect != null) json = EscribirOActualizar(json, "DbDialect", dbDialect);
            if (codSala != null) json = EscribirOActualizar(json, "CodSala", codSala, isNumber: true);
            if (codConc != null) json = EscribirOActualizar(json, "CodConc", codConc, isNumber: true);
            if (appUser != null) json = EscribirOActualizar(json, "AppUser", appUser);
            if (precioPartida != null) json = EscribirOActualizar(json, "PrecioPartida", precioPartida);

            File.WriteAllText(AppSettingsPath, json);
        }

        private static string EscribirOActualizar(string json, string clave, string valor, bool isNumber = false) {
            string buscar = $"\"{clave}\"";
            if (json.Contains(buscar)) {
                // Reemplazar valor existente
                int inicio = json.IndexOf(buscar);
                int dospuntos = json.IndexOf(':', inicio);

                if (isNumber) {
                    // Para números: buscar siguiente número/número
                    int numInicio = dospuntos + 1;
                    while (numInicio < json.Length && (json[numInicio] == ' ' || json[numInicio] == '\t')) numInicio++;
                    int numFin = numInicio;
                    while (numFin < json.Length && char.IsDigit(json[numFin])) numFin++;
                    return json[..numInicio] + valor + json[numFin..];
                }
                else {
                    // Para strings: buscar comillas
                    int abre = json.IndexOf('"', dospuntos + 1);
                    int cierra = json.IndexOf('"', abre + 1);
                    return json[..(abre + 1)] + valor + json[cierra..];
                }
            }
            else {
                // Insertar antes del último cierre de BingoSettings
                int insertPos = json.LastIndexOf('}');
                insertPos = json.LastIndexOf('}', insertPos - 1);
                string valorFormato = isNumber ? valor : $"\"{valor}\"";
                return json[..insertPos] + $",\n    \"{clave}\": {valorFormato}" + json[insertPos..];
            }
        }

        private static string EliminarClave(string json, string clave) {
            string buscar = $"\"{clave}\"";
            if (!json.Contains(buscar)) return json;

            int inicio = json.LastIndexOf('\n', json.IndexOf(buscar));
            int fin = json.IndexOf('\n', json.IndexOf(buscar));
            if (inicio < 0 || fin < 0) return json;
            return json[..inicio] + json[fin..];
        }
    }
}
