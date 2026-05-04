using Serilog;
using Serilog.Formatting.Compact;

namespace BingoHistoricoEditor.Services {
    public static class LogService {
        private static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "operaciones.json");

        static LogService() {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(new CompactJsonFormatter(), LogPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static void Write(string message, object? data = null) {
            if (data != null)
                Log.Information("{Message} {@Data}", message, data);
            else
                Log.Information("{Message}", message);
        }

        public static void Error(string message, Exception ex) {
            Log.Error(ex, "{Message}", message);
        }
    }
}