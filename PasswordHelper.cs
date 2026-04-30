using System.Security.Cryptography;
using System.Text;

namespace BingoHistoricoEditor {
    /// <summary>
    /// Gestión segura de contraseñas usando PBKDF2-SHA256.
    /// El hash almacenado tiene el formato: pbkdf2$iteraciones$sal_base64$hash_base64
    /// Nunca se almacena la contraseña en claro.
    /// </summary>
    public static class PasswordHelper {
        private const int Iterations  = 100_000;
        private const int SaltBytes   = 16;
        private const int HashBytes    = 32;
        private const char Sep        = '$';

        /// <summary>
        /// Genera un hash seguro listo para guardar en appsettings.json.
        /// </summary>
        public static string Hash(string password) {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltBytes);
            byte[] hash = Pbkdf2(password, salt);

            return string.Join(Sep,
                "pbkdf2",
                Iterations,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        /// <summary>
        /// Verifica si una contraseña coincide con un hash almacenado.
        /// Devuelve false (sin excepción) si el hash tiene formato inválido.
        /// </summary>
        public static bool Verify(string password, string storedHash) {
            if (string.IsNullOrWhiteSpace(storedHash)) return false;

            var parts = storedHash.Split(Sep);
            if (parts.Length != 4 || parts[0] != "pbkdf2") return false;

            if (!int.TryParse(parts[1], out int iterations)) return false;

            byte[] salt, expectedHash;
            try {
                salt         = Convert.FromBase64String(parts[2]);
                expectedHash = Convert.FromBase64String(parts[3]);
            }
            catch { return false; }

            byte[] actualHash = Pbkdf2(password, salt, iterations);

            // Comparación en tiempo constante — evita timing attacks
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }

        /// <summary>
        /// Devuelve true si el valor almacenado es claramente texto plano
        /// (no empieza por "pbkdf2$"), para detectar configuraciones heredadas.
        /// </summary>
        public static bool EsTextoPlano(string valor) =>
            !string.IsNullOrWhiteSpace(valor) && !valor.StartsWith("pbkdf2$");

        // ── Internos ──────────────────────────────────────────────────────────

        private static byte[] Pbkdf2(string password, byte[] salt, int iterations = Iterations) {
            using var kdf = new Rfc2898DeriveBytes(
                Encoding.UTF8.GetBytes(password), salt, iterations, HashAlgorithmName.SHA256);
            return kdf.GetBytes(HashBytes);
        }
    }
}
