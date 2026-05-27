using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using BingoHistoricoEditor.Data;

namespace BingoHistoricoEditor.Services {
    public static class LogService {
        private static ILogger _logger;
        private static string _currentUser = "SISTEMA";
        private static FirebirdService _dbService;

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
        /// Asigna el servicio de base de datos para loguear operaciones
        /// </summary>
        public static void SetDbService(FirebirdService dbService) {
            _dbService = dbService;
        }

        /// <summary>
        /// Registro general de operaciones
        /// </summary>
        public static void Write(string operacion, object datos = null) {
            try {
                // El uso de @ es crítico para que Serilog desestructure objetos complejos
                _logger?.Information("{Operacion} {@Datos}", operacion, datos);

                // También guardar en la base de datos si está disponible
                _ = RegistrarEnBDAsync(operacion, datos);
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

        /// <summary>
        /// Registra la operación en la base de datos de forma asíncrona
        /// </summary>
        private static async Task RegistrarEnBDAsync(string operacion, object datos) {
            if (_dbService == null) return;

            try {
                string detalles = datos != null ? JsonSerializer.Serialize(datos) : "";
                await _dbService.InsertarOperacionAsync(_currentUser, operacion, detalles);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error al registrar en BD: {ex.Message}");
            }
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