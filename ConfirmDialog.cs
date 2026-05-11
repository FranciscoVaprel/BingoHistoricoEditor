using System;
using System.Windows.Forms;

namespace BingoHistoricoEditor {
    public partial class ConfirmDialog : Form {
        public ConfirmDialog(string titulo, string mensaje) {
            InitializeComponent();
            lblTitulo.Text = titulo;
            lblMensaje.Text = mensaje;
        }

        private void btnSi_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnNo_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.No;
            Close();
        }
    }
}
