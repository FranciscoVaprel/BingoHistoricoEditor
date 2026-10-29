using System.Text.Json;

namespace BingoHistoricoEditor {
    public static class UserStore {
        private static readonly string UsersPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");
        private static readonly JsonSerializerOptions JsonOpts = new() { WriteIndented = true };

        public static List<UserEntry> CargarUsuarios() {
            if (!File.Exists(UsersPath)) return new List<UserEntry>();
            try {
                return JsonSerializer.Deserialize<List<UserEntry>>(
                    File.ReadAllText(UsersPath)) ?? new List<UserEntry>();
            }
            catch { return new List<UserEntry>(); }
        }

        public static string? VerificarLogin(string usuario, string password) {
            var entry = CargarUsuarios().FirstOrDefault(u =>
                u.Username.Equals(usuario.Trim(), StringComparison.OrdinalIgnoreCase));
            if (entry is null) return null;
            return PasswordHelper.Verify(password, entry.Hash) ? entry.Username : null;
        }

        public static bool HayUsuarios()              => CargarUsuarios().Count > 0;
        public static bool ExisteUsuario(string name) => CargarUsuarios()
            .Any(u => u.Username.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

        public static bool EsAdmin(string username) => CargarUsuarios()
            .Any(u => u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase) && u.IsAdmin);

        /// <summary>
        /// Garantiza que exista al menos un usuario administrador. En instalaciones
        /// migradas desde una versión sin roles (ningún usuario marcado IsAdmin),
        /// promueve al primer usuario de la lista para no dejar la auditoría sin
        /// nadie que pueda verla.
        /// </summary>
        public static void AsegurarAdminExiste() {
            var usuarios = CargarUsuarios();
            if (usuarios.Count == 0 || usuarios.Any(u => u.IsAdmin)) return;

            usuarios[0].IsAdmin = true;
            Guardar(usuarios);
        }

        /// <summary>Añade usuario con contraseña en claro (se hashea aquí).</summary>
        public static void AgregarUsuario(string username, string password, bool isAdmin = false) =>
            AgregarUsuarioConHash(username, PasswordHelper.Hash(password), isAdmin);

        /// <summary>Añade usuario con hash ya generado (desde SetPasswordForm).</summary>
        public static void AgregarUsuarioConHash(string username, string hash, bool isAdmin = false) {
            var usuarios = CargarUsuarios();
            if (ExisteUsuario(username))
                throw new InvalidOperationException($"El usuario '{username}' ya existe.");
            usuarios.Add(new UserEntry { Username = username.Trim(), Hash = hash, IsAdmin = isAdmin });
            Guardar(usuarios);
        }

        public static void EliminarUsuario(string username) {
            var usuarios = CargarUsuarios();
            if (usuarios.Count <= 1)
                throw new InvalidOperationException("No se puede eliminar el último usuario.");

            var entry = usuarios.FirstOrDefault(u =>
                u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException($"Usuario '{username}' no encontrado.");

            if (entry.IsAdmin && usuarios.Count(u => u.IsAdmin) <= 1)
                throw new InvalidOperationException("No se puede eliminar el último usuario administrador.");

            usuarios.Remove(entry);
            Guardar(usuarios);
        }

        public static void CambiarPassword(string username, string nuevaPassword) {
            var usuarios = CargarUsuarios();
            var entry = usuarios.FirstOrDefault(u =>
                u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException($"Usuario '{username}' no encontrado.");
            entry.Hash = PasswordHelper.Hash(nuevaPassword);
            Guardar(usuarios);
        }

        /// <summary>
        /// Recuperación de acceso perdido: hace una copia de seguridad de users.json
        /// (si existe) y lo elimina, dejando la app sin usuarios. Combinado con
        /// Program.LimpiarCredencialesIniciales(), fuerza que el próximo arranque
        /// muestre otra vez el asistente de primera configuración para crear un
        /// administrador nuevo. No es una puerta trasera: requiere acceso al
        /// sistema de archivos del propio ordenador y confirmación explícita en la UI.
        /// </summary>
        public static void EliminarTodosLosUsuarios() {
            if (!File.Exists(UsersPath)) return;

            string backup = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                $"users.json.bak-{DateTime.Now:yyyyMMdd-HHmmss}");
            File.Move(UsersPath, backup, overwrite: true);
        }

        public static void MigrarDesdeAppSettings(string? appUser, string? appPasswordHash) {
            if (!HayUsuarios()
                && !string.IsNullOrWhiteSpace(appUser)
                && !string.IsNullOrWhiteSpace(appPasswordHash))
                Guardar(new List<UserEntry> {
                    // El usuario creado en la configuración inicial es el administrador.
                    new() { Username = appUser, Hash = appPasswordHash, IsAdmin = true }
                });
        }

        private static void Guardar(List<UserEntry> usuarios) =>
            File.WriteAllText(UsersPath, JsonSerializer.Serialize(usuarios, JsonOpts));
    }

    public class UserEntry {
        public string Username { get; set; } = "";
        public string Hash     { get; set; } = "";
        public bool   IsAdmin  { get; set; } = false;
    }
}
