using Serilog;
using Serilog.Core;
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
        private static Logger? _logger;
        // Log de fallos (logs\errores<fecha>.log): texto legible con la traza
        // completa de cada excepción, aparte de la auditoría de operaciones.
        private static Logger? _errores;
        private static string _currentUser = "SISTEMA";
        private static FirebirdService? _dbService;

        static LogService() {
            string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            try {
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
            try {
                _errores = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File(Path.Combine(logDir, "errores.log"),
                                 rollingInterval: RollingInterval.Day,
                                 retainedFileCountLimit: 60,
                                 shared: true,
                                 outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error LogService (errores): {ex.Message}");
            }
        }

        /// <summary>
        /// Anota un fallo en logs\errores&lt;fecha&gt;.log, con la traza de la
        /// excepción. Solo va al fichero: si el fallo es de la BD, intentar
        /// registrarlo en ella fallaría también.
        /// </summary>
        public static void Error(string mensaje, Exception? ex = null) {
            try { _errores?.Error(ex, "{Usuario}: {Mensaje}", _currentUser, mensaje); }
            catch { }
        }

        /// <summary>
        /// Fallo que cierra el programa: se escribe al momento, sin esperar.
        /// </summary>
        public static void Fatal(string mensaje, Exception? ex = null) {
            try { _errores?.Fatal(ex, "{Usuario}: {Mensaje}", _currentUser, mensaje); }
            catch { }
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

        // Cierra (y vacía a disco) los dos ficheros. Log.CloseAndFlush() no
        // servía: cierra el logger global de Serilog, que aquí no se usa.
        public static void Close() {
            _logger?.Dispose();
            _errores?.Dispose();
        }
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