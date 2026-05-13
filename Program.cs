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

        private static string EscribirOActualizar(string json, string clave, string valor) {
            string buscar = $"\"{clave}\"";
            if (json.Contains(buscar)) {
                // Reemplazar valor existente
                int inicio = json.IndexOf(buscar);
                int dospuntos = json.IndexOf(':', inicio);
                int abre = json.IndexOf('"', dospuntos + 1);
                int cierra = json.IndexOf('"', abre + 1);
                return json[..(abre + 1)] + valor + json[cierra..];
            }
            else {
                // Insertar antes del último cierre de BingoSettings
                int insertPos = json.LastIndexOf('}');
                insertPos = json.LastIndexOf('}', insertPos - 1);
                return json[..insertPos] + $",\n    \"{clave}\": \"{valor}\"" + json[insertPos..];
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
