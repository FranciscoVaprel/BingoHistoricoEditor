namespace BingoHistoricoEditor {
    /// <summary>
    /// Diálogo para establecer o cambiar usuario y contraseña.
    /// Se muestra en el primer arranque o desde ajustes.
    /// Devuelve UsuarioGenerado y HashGenerado tras aceptar.
    /// </summary>
    public partial class SetPasswordForm : Form {
        public string UsuarioGenerado { get; private set; } = "";
        public string HashGenerado    { get; private set; } = "";

        public SetPasswordForm(bool primerArranque = false) {
            InitializeComponent();
            if (primerArranque) {
                lblTitulo.Text    = "Primera ejecución";
                lblSubtitulo.Text = "Crea las credenciales de acceso a la aplicación.";
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e) {
            string usuario = txtUsuario.Text.Trim();
            string pwd1    = txtNueva.Text;
            string pwd2    = txtRepetir.Text;

            if (usuario.Length < 2) {
                MostrarError("El nombre de usuario debe tener al menos 2 caracteres.");
                txtUsuario.Focus(); return;
            }
            if (pwd1.Length < 4) {
                MostrarError("La contraseña debe tener al menos 4 caracteres.");
                txtNueva.Focus(); return;
            }
            if (pwd1 != pwd2) {
                MostrarError("Las contraseñas no coinciden.");
                txtRepetir.Clear(); txtRepetir.Focus(); return;
            }

            UsuarioGenerado = usuario;
            HashGenerado    = PasswordHelper.Hash(pwd1);
            DialogResult    = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void MostrarError(string msg) {
            lblError.Text    = msg;
            lblError.Visible = true;
        }
    }
}
