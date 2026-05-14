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

        /// <summary>Añade usuario con contraseña en claro (se hashea aquí).</summary>
        public static void AgregarUsuario(string username, string password) =>
            AgregarUsuarioConHash(username, PasswordHelper.Hash(password));

        /// <summary>Añade usuario con hash ya generado (desde SetPasswordForm).</summary>
        public static void AgregarUsuarioConHash(string username, string hash) {
            var usuarios = CargarUsuarios();
            if (ExisteUsuario(username))
                throw new InvalidOperationException($"El usuario '{username}' ya existe.");
            usuarios.Add(new UserEntry { Username = username.Trim(), Hash = hash });
            Guardar(usuarios);
        }

        public static void EliminarUsuario(string username) {
            var usuarios = CargarUsuarios();
            if (usuarios.Count <= 1)
                throw new InvalidOperationException("No se puede eliminar el último usuario.");
            int n = usuarios.RemoveAll(u =>
                u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));
            if (n == 0) throw new InvalidOperationException($"Usuario '{username}' no encontrado.");
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

        public static void MigrarDesdeAppSettings(string? appUser, string? appPasswordHash) {
            if (!HayUsuarios()
                && !string.IsNullOrWhiteSpace(appUser)
                && !string.IsNullOrWhiteSpace(appPasswordHash))
                Guardar(new List<UserEntry> {
                    new() { Username = appUser, Hash = appPasswordHash }
                });
        }

        private static void Guardar(List<UserEntry> usuarios) =>
            File.WriteAllText(UsersPath, JsonSerializer.Serialize(usuarios, JsonOpts));
    }

    public class UserEntry {
        public string Username { get; set; } = "";
        public string Hash     { get; set; } = "";
    }
}
