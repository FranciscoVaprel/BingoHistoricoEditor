using Microsoft.Extensions.Configuration;

namespace BingoHistoricoEditor {
    public partial class LoginForm : Form {
        private readonly string _hashAlmacenado;
        private int _intentosFallidos = 0;
        private const int MaxIntentos = 3;

        public LoginForm(IConfiguration config) {
            _hashAlmacenado = config["BingoSettings:AppPasswordHash"] ?? "";
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            txtPassword.Focus();
        }

        private void btnEntrar_Click(object sender, EventArgs e) => IntentarLogin();

        private void txtPassword_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) IntentarLogin();
        }

        private void IntentarLogin() {
            if (PasswordHelper.Verify(txtPassword.Text, _hashAlmacenado)) {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            _intentosFallidos++;
            txtPassword.Clear();
            txtPassword.Focus();

            int restantes = MaxIntentos - _intentosFallidos;
            if (restantes <= 0) {
                MessageBox.Show("Demasiados intentos fallidos. La aplicacion se cerrara.",
                    "Acceso bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            lblError.Text    = $"Contrasena incorrecta. Intentos restantes: {restantes}";
            lblError.Visible = true;
            AnimarError();
        }

        private async void AnimarError() {
            int x = txtPassword.Left;
            foreach (int offset in new[] { -6, 6, -4, 4, -2, 2, 0 }) {
                txtPassword.Left = x + offset;
                await Task.Delay(30);
            }
            txtPassword.Left = x;
        }
    }
}
