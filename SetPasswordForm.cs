namespace BingoHistoricoEditor {
    /// <summary>
    /// Diálogo para establecer una nueva contraseña.
    /// Se muestra en el primer arranque (sin hash configurado)
    /// o desde el menú "Cambiar contraseña".
    /// Devuelve el hash generado en la propiedad HashGenerado.
    /// </summary>
    public partial class SetPasswordForm : Form {
        public string HashGenerado { get; private set; } = "";

        public SetPasswordForm(bool primerArranque = false) {
            InitializeComponent();
            if (primerArranque) {
                lblTitulo.Text    = "Primera ejecución";
                lblSubtitulo.Text = "Establece una contraseña para proteger el acceso.";
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e) {
            string pwd1 = txtNueva.Text;
            string pwd2 = txtRepetir.Text;

            if (pwd1.Length < 4) {
                MostrarError("La contraseña debe tener al menos 4 caracteres.");
                txtNueva.Focus(); return;
            }
            if (pwd1 != pwd2) {
                MostrarError("Las contraseñas no coinciden.");
                txtRepetir.Clear(); txtRepetir.Focus(); return;
            }

            HashGenerado = PasswordHelper.Hash(pwd1);
            DialogResult = DialogResult.OK;
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
