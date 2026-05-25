using Microsoft.Extensions.Configuration;
using System;
using System.Windows.Forms;

namespace BingoHistoricoEditor {
    public partial class SettingsForm : Form {
        private readonly IConfiguration _config;

        public SettingsForm(IConfiguration config) {
            InitializeComponent();
            _config = config;
            CargarValoresActuales();
        }

        private void CargarValoresActuales() {
            // Base de Datos - Conexión
            cmbDbModo.Text = _config["BingoSettings:DbModo"] ?? "";
            txtDbSource.Text = _config["BingoSettings:DbSource"] ?? "";
            txtDbPort.Text = _config["BingoSettings:DbPort"] ?? "";

            // Base de Datos - Rutas
            txtDbRelativePath.Text = _config["BingoSettings:DbRelativePath"] ?? "";
            txtDbAbsolutePath.Text = _config["BingoSettings:DbAbsolutePath"] ?? "";
            txtDbRemotePath.Text = _config["BingoSettings:DbRemotePath"] ?? "";

            // Base de Datos - Credenciales
            txtDbUser.Text = _config["BingoSettings:DbUser"] ?? "";
            txtDbPass.Text = _config["BingoSettings:DbPass"] ?? "";
            cmbDbCharset.Text = _config["BingoSettings:DbCharset"] ?? "";
            txtDbDialect.Text = _config["BingoSettings:DbDialect"] ?? "";

            // Aplicación
            txtCodSala.Text = _config["BingoSettings:CodSala"] ?? "";
            txtCodConc.Text = _config["BingoSettings:CodConc"] ?? "";
            txtAppUser.Text = _config["BingoSettings:AppUser"] ?? "";
            txtPrecioPartida.Text = _config["BingoSettings:PrecioPartida"] ?? "0.50";
        }

        private bool ValidarTodosLosValores() {
            lblMensaje.Text = "";

            // Validar solo los campos de Aplicación (BD es solo lectura)

            // Validar Aplicación
            var (valid, error) = SettingsValidator.ValidateNumericRange(txtCodSala.Text, "Código Sala", 1, 9999);
            if (!valid) {
                lblMensaje.Text = error;
                tabControl.SelectedIndex = 1;
                return false;
            }

            (valid, error) = SettingsValidator.ValidateNumericRange(txtCodConc.Text, "Código Conc", 1, 9999);
            if (!valid) {
                lblMensaje.Text = error;
                tabControl.SelectedIndex = 1;
                return false;
            }

            (valid, error) = SettingsValidator.ValidateUsername(txtAppUser.Text);
            if (!valid) {
                lblMensaje.Text = error;
                tabControl.SelectedIndex = 1;
                return false;
            }

            (valid, error) = SettingsValidator.ValidateDecimalRange(txtPrecioPartida.Text, "Precio Partida", 0, 9999);
            if (!valid) {
                lblMensaje.Text = error;
                tabControl.SelectedIndex = 1;
                return false;
            }

            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e) {
            if (!ValidarTodosLosValores()) {
                MessageBox.Show("Por favor, corrija los errores antes de guardar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {
                // Guardar solo los campos de Aplicación (BD no es editable)
                Program.GuardarConfiguracion(
                    dbModo: null,
                    dbSource: null,
                    dbPort: null,
                    dbRelativePath: null,
                    dbAbsolutePath: null,
                    dbRemotePath: null,
                    dbUser: null,
                    dbPass: null,
                    dbCharset: null,
                    dbDialect: null,
                    codSala: txtCodSala.Text,
                    codConc: txtCodConc.Text,
                    appUser: txtAppUser.Text,
                    precioPartida: txtPrecioPartida.Text
                );

                MessageBox.Show("Configuración guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex) {
                lblMensaje.Text = $"Error al guardar: {ex.Message}";
                MessageBox.Show($"Error al guardar la configuración:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
