using Serilog;
using Serilog.Formatting.Compact;

namespace BingoHistoricoEditor.Services {
    public static class LogService {
        private static readonly ILogger _logger;

        static LogService() {
            string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            string logPath = Path.Combine(logDir, "operaciones.json");

            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(new CompactJsonFormatter(), logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static void Write(string message, object? data = null) {
            try {
                if (data != null) _logger.Information("{Message} {@Data}", message, data);
                else _logger.Information("{Message}", message);
            }
            catch { }
        }

        public static void Error(string message, Exception? ex = null) {
            try {
                if (ex != null) _logger.Error(ex, "{Message}", message);
                else _logger.Error("{Message}", message);
            }
            catch { }
        }
    }
}
