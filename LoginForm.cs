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
            Text = "Verificar identidad"; Size = new Size(320, 270);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = SystemColors.Control; ForeColor = SystemColors.ControlText;

            var lbl1 = new Label { Text = "Confirma tu usuario y contraseña", ForeColor = SystemColors.GrayText,
                AutoSize = true, Location = new Point(24, 20) };
            var lbl2 = new Label { Text = "Usuario",     ForeColor = SystemColors.GrayText, AutoSize = true, Location = new Point(24,  52) };
            var lbl3 = new Label { Text = "Contraseña",  ForeColor = SystemColors.GrayText, AutoSize = true, Location = new Point(24,  98) };

            _txtUser = new TextBox { Size = new Size(268, 26), Location = new Point(24, 70),
                BackColor = SystemColors.Window, ForeColor = SystemColors.WindowText, BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10f) };
            _txtPass = new TextBox { Size = new Size(268, 26), Location = new Point(24, 116),
                UseSystemPasswordChar = true,
                BackColor = SystemColors.Window, ForeColor = SystemColors.WindowText, BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10f) };

            _lblErr = new Label { Text = "Credenciales incorrectas", ForeColor = Color.Red,
                AutoSize = true, Location = new Point(24, 150), Visible = false };

            var btnOk = new Button { Text = "Verificar", Size = new Size(268, 36),
                Location = new Point(24, 172),
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
}
