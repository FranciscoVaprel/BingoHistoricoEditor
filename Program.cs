using BingoHistoricoEditor.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace BingoHistoricoEditor {
    internal static class Program {
        public static IConfiguration Configuration { get; private set; } = null!;

        private static readonly string AppSettingsPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

        [STAThread]
        static void Main() {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            ApplicationConfiguration.Initialize();

            // Primer arranque: sin hash configurado -> pedir que establezca contrasena
            string hashActual = Configuration["BingoSettings:AppPasswordHash"] ?? "";
            if (string.IsNullOrWhiteSpace(hashActual)) {
                using var setup = new SetPasswordForm(primerArranque: true);
                if (setup.ShowDialog() != DialogResult.OK) return;
                GuardarHash(setup.HashGenerado);
                // Recargar config con el hash recien guardado
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .Build();
            }

            // Login normal
            using var login = new LoginForm(Configuration);
            if (login.ShowDialog() != DialogResult.OK) return;

            Application.Run(new MainForm(new FirebirdService(Configuration), Configuration));
        }

        /// <summary>
        /// Escribe el hash en appsettings.json.
        /// Solo modifica la clave AppPasswordHash; el resto del fichero no se toca.
        /// </summary>
        public static void GuardarHash(string hash) {
            string json = File.ReadAllText(AppSettingsPath);

            // Si ya existe la clave, reemplazarla; si no, insertarla en BingoSettings
            if (json.Contains("\"AppPasswordHash\"")) {
                int inicio = json.IndexOf("\"AppPasswordHash\"");
                int dospuntos = json.IndexOf(':', inicio);
                int abre = json.IndexOf('"', dospuntos + 1);
                int cierra = json.IndexOf('"', abre + 1);
                json = json[..(abre + 1)] + hash + json[cierra..];
            }
            else {
                // Insertar antes del ultimo cierre de BingoSettings
                int insertPos = json.LastIndexOf('}');
                insertPos = json.LastIndexOf('}', insertPos - 1);
                string linea = $",\n    \"AppPasswordHash\": \"{hash}\"";
                json = json[..insertPos] + linea + json[insertPos..];
            }

            File.WriteAllText(AppSettingsPath, json);
        }
    }
}
