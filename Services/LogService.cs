using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace BingoHistoricoEditor.Services {
    public static class LogService {
        private static ILogger _logger;
        private static string _currentUser = "SISTEMA";

        static LogService() {
            try {
                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);

                string logPath = Path.Combine(logDir, "operaciones.json");

                _logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File(new JsonLogFormatter(() => _currentUser),
                                 logPath,
                                 rollingInterval: RollingInterval.Day,
                                 shared: true,
                                 flushToDiskInterval: TimeSpan.FromSeconds(1))
                    .CreateLogger();
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error LogService: {ex.Message}");
            }
        }

        public static void SetCurrentUser(string username) => _currentUser = username?.ToUpper() ?? "SISTEMA";

        /// <summary>
        /// Registro general de operaciones
        /// </summary>
        public static void Write(string operacion, object datos = null) {
            try {
                // El uso de @ es crítico para que Serilog desestructure objetos complejos
                _logger?.Information("{Operacion} {@Datos}", operacion, datos);
            }
            catch { }
        }

        /// <summary>
        /// Registro de eventos internos del sistema o flujo de la aplicación
        /// </summary>
        public static void Sistema(string mensaje) {
            Write("SISTEMA", mensaje);
        }

        /// <summary>
        /// Registro de auditoría para cambios en base de datos
        /// </summary>
        public static void AuditDB(string operacion, object datos) {
            Write(operacion, datos);
        }

        public static void Close() => Log.CloseAndFlush();
    }

    public class JsonLogFormatter : ITextFormatter {
        private readonly Func<string> _getUserName;
        public JsonLogFormatter(Func<string> getUserName) => _getUserName = getUserName;

        public void Format(LogEvent logEvent, TextWriter output) {
            try {
                string operacionValue = "";
                if (logEvent.Properties.TryGetValue("Operacion", out var op))
                    operacionValue = op.ToString().Trim('"');

                object detallesValue = null;
                if (logEvent.Properties.TryGetValue("Datos", out var datosProp)) {
                    detallesValue = datosProp;
                }

                var dictionary = new Dictionary<string, object>
                {
                    { "fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "usuario", _getUserName() },
                    { "operacion", operacionValue },
                    { "nivel", logEvent.Level.ToString() },
                    { "detalles", detallesValue }
                };

                output.WriteLine(JsonSerializer.Serialize(dictionary));
            }
            catch { }
        }
    }
}