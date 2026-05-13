using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace BingoHistoricoEditor.Services {
    // Esta clase es estática: todo lo que hay dentro debe ser 'static'
    public static class LogService {
        private static ILogger _logger;
        private static string _currentUser = "SISTEMA";

        static LogService() {
            try {
                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);

                string logPath = Path.Combine(logDir, "operaciones.json");

                // Pasamos una función que devuelve el usuario actual al formateador
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

        public static void Write(string operacion, object datos = null) {
            try {
                // El @ es vital para que Serilog desestructure el objeto (entry)
                _logger?.Information("{Operacion} {@Datos}", operacion, datos);
            }
            catch { }
        }

        public static void Close() => Log.CloseAndFlush();
    }

    // Esta clase NO es estática porque implementa una interfaz (instancia)
    public class JsonLogFormatter : ITextFormatter {
        private readonly Func<string> _getUserName;

        public JsonLogFormatter(Func<string> getUserName) {
            _getUserName = getUserName;
        }

        public void Format(LogEvent logEvent, TextWriter output) {
            try {
                // Extraer la operación
                string operacionValue = "";
                if (logEvent.Properties.TryGetValue("Operacion", out var op))
                    operacionValue = op.ToString().Trim('"');

                // Extraer los datos desestructurados (el objeto entry)
                object detallesValue = null;
                if (logEvent.Properties.TryGetValue("Datos", out var datosProp)) {
                    detallesValue = datosProp;
                }

                // Construimos el diccionario para el JSON
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