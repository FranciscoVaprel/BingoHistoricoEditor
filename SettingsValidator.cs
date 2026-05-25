using System;
using System.IO;

namespace BingoHistoricoEditor {
    /// <summary>
    /// Valida los valores de configuración de appsettings.json
    /// </summary>
    public static class SettingsValidator {

        public static (bool isValid, string errorMessage) ValidateDbModo(string dbModo) {
            if (string.IsNullOrWhiteSpace(dbModo)) {
                return (false, "DbModo no puede estar vacío.");
            }
            if (dbModo != "Local" && dbModo != "Remote") {
                return (false, "DbModo debe ser 'Local' o 'Remote'.");
            }
            return (true, "");
        }

        public static (bool isValid, string errorMessage) ValidateDbPort(string port) {
            if (string.IsNullOrWhiteSpace(port)) {
                return (false, "Puerto no puede estar vacío.");
            }
            if (!int.TryParse(port, out int portNum) || portNum < 1 || portNum > 65535) {
                return (false, "Puerto debe ser un número entre 1 y 65535.");
            }
            return (true, "");
        }

        public static (bool isValid, string errorMessage) ValidateNumericRange(string value, string fieldName, int min, int max) {
            if (string.IsNullOrWhiteSpace(value)) {
                return (false, $"{fieldName} no puede estar vacío.");
            }
            if (!int.TryParse(value, out int numValue) || numValue < min || numValue > max) {
                return (false, $"{fieldName} debe ser un número entre {min} y {max}.");
            }
            return (true, "");
        }

        public static (bool isValid, string errorMessage) ValidateDecimalRange(string value, string fieldName, decimal min, decimal max) {
            if (string.IsNullOrWhiteSpace(value)) {
                return (false, $"{fieldName} no puede estar vacío.");
            }
            if (!decimal.TryParse(value, out decimal decValue) || decValue < min || decValue > max) {
                return (false, $"{fieldName} debe ser un número decimal entre {min} y {max}.");
            }
            return (true, "");
        }

        public static (bool isValid, string errorMessage) ValidateFilePath(string path) {
            if (string.IsNullOrWhiteSpace(path)) {
                return (false, "Ruta no puede estar vacía.");
            }
            try {
                // Intentar construir la ruta relativa o absoluta
                string fullPath = Path.IsPathRooted(path) ? path : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                // Validar caracteres inválidos
                var invalidChars = Path.GetInvalidPathChars();
                if (path.IndexOfAny(invalidChars) >= 0) {
                    return (false, "La ruta contiene caracteres inválidos.");
                }
                return (true, "");
            }
            catch {
                return (false, "Ruta inválida.");
            }
        }

        public static (bool isValid, string errorMessage) ValidateUsername(string username) {
            if (string.IsNullOrWhiteSpace(username)) {
                return (false, "Usuario no puede estar vacío.");
            }
            if (username.Length > 50) {
                return (false, "Usuario no puede tener más de 50 caracteres.");
            }
            return (true, "");
        }

        public static (bool isValid, string errorMessage) ValidateDbUser(string dbUser) {
            if (string.IsNullOrWhiteSpace(dbUser)) {
                return (false, "Usuario BD no puede estar vacío.");
            }
            if (dbUser.Length > 50) {
                return (false, "Usuario BD no puede tener más de 50 caracteres.");
            }
            return (true, "");
        }

        public static (bool isValid, string errorMessage) ValidateDbPassword(string dbPass) {
            if (string.IsNullOrWhiteSpace(dbPass)) {
                return (false, "Contraseña BD no puede estar vacía.");
            }
            if (dbPass.Length > 100) {
                return (false, "Contraseña BD no puede tener más de 100 caracteres.");
            }
            return (true, "");
        }

        public static (bool isValid, string errorMessage) ValidateDbDialect(string dialect) {
            if (string.IsNullOrWhiteSpace(dialect)) {
                return (false, "Dialecto no puede estar vacío.");
            }
            if (dialect != "1" && dialect != "3") {
                return (false, "Dialecto debe ser '1' o '3'.");
            }
            return (true, "");
        }

        public static (bool isValid, string errorMessage) ValidateDbCharset(string charset) {
            if (string.IsNullOrWhiteSpace(charset)) {
                return (false, "Conjunto de caracteres no puede estar vacío.");
            }
            // Validaciones básicas - agregar más según sea necesario
            var validCharsets = new[] { "UTF8", "ISO8859_1", "WIN1252" };
            if (Array.IndexOf(validCharsets, charset) < 0) {
                return (false, "Conjunto de caracteres no válido. Use UTF8, ISO8859_1 o WIN1252.");
            }
            return (true, "");
        }

        public static (bool isValid, string errorMessage) ValidateDbSource(string source) {
            if (string.IsNullOrWhiteSpace(source)) {
                return (false, "Servidor no puede estar vacío.");
            }
            return (true, "");
        }
    }
}
