using BingoHistoricoEditor.Services;
using Microsoft.Extensions.Configuration;

namespace BingoHistoricoEditor {
    public partial class LoginForm : Form {
        private int _intentosFallidos = 0;
        private const int MaxIntentos = 3;
        public string UsuarioLogueado { get; private set; } = "";

        public LoginForm(IConfiguration config) {
            // Migrar usuario antiguo de appsettings → users.json si hace falta
            UserStore.MigrarDesdeAppSettings(
                config["BingoSettings:AppUser"],
                config["BingoSettings:AppPasswordHash"]);

            // Instalaciones ya existentes (users.json sin roles): asegurar un admin.
            UserStore.AsegurarAdminExiste();

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            txtUsuario.Focus();
        }

        // ── Eventos ───────────────────────────────────────────────────────────

        private void btnEntrar_Click(object sender, EventArgs e)    => IntentarLogin();
        private void btnGestionar_Click(object sender, EventArgs e) => AbrirGestionUsuarios();
        private void btnCerrar_Click(object sender, EventArgs e)    => CerrarAplicacion();
        private void lnkRestablecer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => RestablecerAdministrador();

        private void txtUsuario_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; txtPassword.Focus(); }
        }
        private void txtPassword_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; IntentarLogin(); }
        }

        // ── Lógica de login ───────────────────────────────────────────────────

        private void IntentarLogin() {
            string? usuarioOk = UserStore.VerificarLogin(txtUsuario.Text, txtPassword.Text);

            if (usuarioOk is not null) {
                UsuarioLogueado = usuarioOk;
                LogService.Sistema($"LOGIN OK: {usuarioOk}");
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            _intentosFallidos++;
            txtPassword.Clear();

            int restantes = MaxIntentos - _intentosFallidos;
            if (restantes <= 0) {
                LogService.Sistema($"LOGIN BLOQUEADO: {txtUsuario.Text.Trim()} (3 intentos)");
                MessageBox.Show("Acceso bloqueado por seguridad.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            lblError.Text    = $"Credenciales incorrectas — {restantes} intento{(restantes > 1 ? "s" : "")} restante{(restantes > 1 ? "s" : "")}";
            lblError.Visible = true;

            if (!UserStore.ExisteUsuario(txtUsuario.Text.Trim()))
                txtUsuario.Focus();
            else
                txtPassword.Focus();

            AnimarError();
        }

        // ── Gestión de usuarios ───────────────────────────────────────────────

        private void AbrirGestionUsuarios() {
            // La gestión de usuarios está disponible sin autenticación previa.
            // La autenticación se solicita solo cuando se va a cambiar la contraseña.
            using var gestor = new UserManagerForm();
            gestor.ShowDialog();
        }

        private void CerrarAplicacion() {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // ── Recuperación de acceso perdido ───────────────────────────────────

        /// <summary>
        /// Elimina TODOS los usuarios (con copia de seguridad de users.json) y
        /// limpia las credenciales iniciales de appsettings.json, para forzar
        /// que el próximo arranque muestre el asistente de primera configuración.
        /// No es una puerta trasera: requiere confirmación explícita y acceso
        /// físico al ordenador; no hay ningún usuario/contraseña oculto.
        /// </summary>
        private void RestablecerAdministrador() {
            using var confirmar = new ConfirmarRestablecerForm();
            if (confirmar.ShowDialog(this) != DialogResult.OK) return;

            try {
                UserStore.EliminarTodosLosUsuarios();
                Program.LimpiarCredencialesIniciales();
                LogService.Sistema("RESET ADMINISTRADOR: se eliminaron todos los usuarios desde la pantalla de login.");

                MessageBox.Show(
                    "Se han eliminado todos los usuarios (se guardó una copia de seguridad de users.json " +
                    "en la carpeta de la aplicación).\n\nLa aplicación se va a reiniciar para crear un " +
                    "administrador nuevo.",
                    "Restablecimiento completado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Application.Restart();
            }
            catch (Exception ex) {
                MessageBox.Show($"No se pudo restablecer el acceso:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

    // ── Diálogo de autenticación previa ──────────────────────────────────────

    /// <summary>
    /// Pide las credenciales de cualquier usuario existente antes
    /// de permitir abrir la gestión de usuarios.
    /// </summary>
    public class AutenticacionPrevia : Form {
        private TextBox _txtUser = null!, _txtPass = null!;
        private Label   _lblErr  = null!;

        public AutenticacionPrevia() {
            Text = "Verificar identidad"; ClientSize = new Size(320, 280);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = SystemColors.Control; ForeColor = SystemColors.ControlText;
            Font = new Font("Segoe UI", 9f);

            var lbl1 = new Label { Text = "Confirma tu usuario y contraseña", ForeColor = SystemColors.GrayText,
                AutoSize = true, Location = new Point(24, 20) };
            var lbl2 = new Label { Text = "Usuario",     ForeColor = SystemColors.GrayText, AutoSize = true, Location = new Point(24,  56) };
            var lbl3 = new Label { Text = "Contraseña",  ForeColor = SystemColors.GrayText, AutoSize = true, Location = new Point(24, 114) };

            _txtUser = new TextBox { Size = new Size(268, 26), Location = new Point(24, 76),
                BackColor = SystemColors.Window, ForeColor = SystemColors.WindowText, BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10f) };
            _txtPass = new TextBox { Size = new Size(268, 26), Location = new Point(24, 134),
                UseSystemPasswordChar = true,
                BackColor = SystemColors.Window, ForeColor = SystemColors.WindowText, BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10f) };

            _lblErr = new Label { Text = "Credenciales incorrectas", ForeColor = Color.Red,
                AutoSize = true, Location = new Point(24, 170), Visible = false };

            var btnOk = new Button { Text = "Verificar", Size = new Size(268, 36),
                Location = new Point(24, 196),
                ForeColor = SystemColors.ControlText,
                FlatStyle = FlatStyle.Standard, Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand, UseVisualStyleBackColor = true };
            btnOk.Click += (_, _) => Verificar();

            _txtUser.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; _txtPass.Focus(); } };
            _txtPass.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Verificar(); } };

            Controls.AddRange(new Control[] { lbl1, lbl2, lbl3, _txtUser, _txtPass, _lblErr, btnOk });
        }

        private void Verificar() {
            if (UserStore.VerificarLogin(_txtUser.Text, _txtPass.Text) is not null) {
                DialogResult = DialogResult.OK; Close();
            }
            else {
                _lblErr.Visible = true;
                _txtPass.Clear(); _txtPass.Focus();
            }
        }
    }

    // ── Diálogo de confirmación de restablecimiento de administrador ──────────

    /// <summary>
    /// Exige escribir la palabra "ELIMINAR" para confirmar el borrado de todos
    /// los usuarios. Un simple Sí/No sería demasiado fácil de pulsar sin querer
    /// para una acción tan destructiva.
    /// </summary>
    internal class ConfirmarRestablecerForm : Form {
        private TextBox _txtConfirmar = null!;
        private Button  _btnConfirmar = null!;

        public ConfirmarRestablecerForm() {
            Text = "Restablecer administrador"; ClientSize = new Size(380, 300);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = SystemColors.Control; ForeColor = SystemColors.ControlText;
            Font = new Font("Segoe UI", 9f);

            var lblAviso = new Label {
                Text = "Esto eliminará TODOS los usuarios de la aplicación (no solo los " +
                       "administradores). Se guardará una copia de seguridad de users.json, " +
                       "pero tendrás que crear un administrador nuevo al reiniciar.\n\n" +
                       "Usa esto solo si no hay ninguna otra forma de entrar.",
                ForeColor = Color.Red, AutoSize = false,
                Location = new Point(24, 16), Size = new Size(332, 130),
            };

            var lblInstruccion = new Label {
                Text = "Escribe ELIMINAR para confirmar:",
                ForeColor = SystemColors.GrayText, AutoSize = true,
                Location = new Point(24, 154),
            };

            _txtConfirmar = new TextBox {
                Size = new Size(332, 26), Location = new Point(24, 174),
                BackColor = SystemColors.Window, ForeColor = SystemColors.WindowText,
                BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10f),
            };
            _txtConfirmar.TextChanged += (_, _) => _btnConfirmar.Enabled =
                _txtConfirmar.Text.Trim().Equals("ELIMINAR", StringComparison.OrdinalIgnoreCase);

            _btnConfirmar = new Button {
                Text = "Eliminar todos los usuarios", Size = new Size(332, 36),
                Location = new Point(24, 210), Enabled = false,
                ForeColor = SystemColors.ControlText, FlatStyle = FlatStyle.Standard,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand, UseVisualStyleBackColor = true,
            };
            _btnConfirmar.Click += (_, _) => { DialogResult = DialogResult.OK; Close(); };

            var btnCancelar = new Button {
                Text = "Cancelar", Size = new Size(332, 30),
                Location = new Point(24, 252),
                ForeColor = SystemColors.ControlText, FlatStyle = FlatStyle.Standard,
                Cursor = Cursors.Hand, UseVisualStyleBackColor = true,
            };
            btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lblAviso, lblInstruccion, _txtConfirmar, _btnConfirmar, btnCancelar });
        }
    }
}
