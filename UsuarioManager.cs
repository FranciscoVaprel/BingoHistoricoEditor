using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BingoHistoricoEditor.Services {
    public class UsuarioCredencial {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

    public static class UsuarioManager {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "usuarios.json");

        public static bool HayUsuarios() {
            var lista = CargarLista();
            return lista != null && lista.Count > 0;
        }

        public static List<UsuarioCredencial> CargarLista() {
            if (!File.Exists(FilePath)) return new List<UsuarioCredencial>();
            try {
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<UsuarioCredencial>>(json) ?? new List<UsuarioCredencial>();
            }
            catch { return new List<UsuarioCredencial>(); }
        }

        public static void GuardarUsuario(string usuario, string hash) {
            var usuarios = CargarLista();
            var existente = usuarios.FirstOrDefault(u => u.Username.Equals(usuario, StringComparison.OrdinalIgnoreCase));

            if (existente != null) {
                existente.PasswordHash = hash;
            }
            else {
                usuarios.Add(new UsuarioCredencial { Username = usuario, PasswordHash = hash });
            }

            File.WriteAllText(FilePath, JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static bool Validar(string usuario, string password) {
            var user = CargarLista().FirstOrDefault(u => u.Username.Equals(usuario, StringComparison.OrdinalIgnoreCase));
            if (user == null) return false;

            // PasswordHelper es la clase que ya usas para verificar hashes
            return PasswordHelper.Verify(password, user.PasswordHash);
        }
    }
}