using Microsoft.Extensions.Configuration;
using BingoHistoricoEditor.Services;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingoHistoricoEditor {
    public partial class LoginForm : Form {
        private readonly string _hashAlmacenado;
        private readonly string _usuarioEsperado;
        private int _intentosFallidos = 0;
        private const int MaxIntentos = 3;
        public string UsuarioLogueado { get; private set; } = "";

        public LoginForm(IConfiguration config) {
            // Leemos los valores directamente de tu JSON
            _usuarioEsperado = config["BingoSettings:AppUser"] ?? "admin";
            // Soporta tanto el Hash como la password en texto plano para desarrollo
            _hashAlmacenado = config["BingoSettings:AppPasswordHash"] ?? config["BingoSettings:AppPassword"] ?? "";

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            txtUsuario.Focus(); // El foco siempre inicia en el nombre de usuario
        }

        private void btnEntrar_Click(object sender, EventArgs e) => IntentarLogin();

        private void txtUsuario_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                e.SuppressKeyPress = true;
                txtPassword.Focus(); // Al pulsar Enter en usuario, pasa a password
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                e.SuppressKeyPress = true;
                IntentarLogin(); // Al pulsar Enter en password, intenta entrar
            }
        }

        private void IntentarLogin() {
            bool usuarioOk = txtUsuario.Text.Trim().ToLower() == _usuarioEsperado.ToLower();
            bool passOk = PasswordHelper.Verify(txtPassword.Text, _hashAlmacenado) || txtPassword.Text == _hashAlmacenado;

            if (usuarioOk && passOk) {
                // Guardamos el nombre antes de cerrar
                this.UsuarioLogueado = txtUsuario.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            // Gestión de errores e intentos
            _intentosFallidos++;
            txtPassword.Clear();

            int restantes = MaxIntentos - _intentosFallidos;
            if (restantes <= 0) {
                MessageBox.Show("Acceso bloqueado por seguridad.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            lblError.Text = $"Credenciales incorrectas ({restantes} intentos)";
            lblError.Visible = true;

            // Si el usuario es el que está mal, devolvemos el foco allí
            if (!usuarioOk) {
                txtUsuario.Focus();
                txtUsuario.SelectAll();
            }
            else {
                txtPassword.Focus();
            }

            AnimarError();
        }

        private async void AnimarError() {
            int x = pnlCentro.Left;
            foreach (int offset in new[] { -6, 6, -4, 4, -2, 2, 0 }) {
                pnlCentro.Left = x + offset;
                await Task.Delay(30);
            }
            pnlCentro.Left = x;
        }
    }
}