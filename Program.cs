using BingoHistoricoEditor.Data;
using Microsoft.Extensions.Configuration;

namespace BingoHistoricoEditor {
    internal static class Program {
        public static IConfiguration Configuration { get; private set; } = null!;

        [STAThread]
        static void Main() {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm(new FirebirdService(Configuration), Configuration));
        }
    }
}
