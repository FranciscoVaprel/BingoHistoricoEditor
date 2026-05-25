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

            // Validar Base de Datos - Conexión
            var (valid, error) = SettingsValidator.ValidateDbModo(cmbDbModo.Text);
            if (!valid) {
                lblMensaje.Text = error;
                tabControl.SelectedIndex = 0;
                return false;
            }

            (valid, error) = SettingsValidator.ValidateDbSource(txtDbSource.Text);
            if (!valid) {
                lblMensaje.Text = error;
                tabControl.SelectedIndex = 0;
                return false;
            }

            (valid, error) = SettingsValidator.ValidateDbPort(txtDbPort.Text);
            if (!valid) {
                lblMensaje.Text = error;
                tabControl.SelectedIndex = 0;
                return false;
            }

            // Validar Base de Datos - Rutas
            (valid, error) = SettingsValidator.ValidateFilePath(txtDbRelativePath.Text);
            if (!valid) {
                lblMensaje.Text = $"Ruta Relativa: {error}";
                tabControl.SelectedIndex = 0;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtDbAbsolutePath.Text)) {
                (valid, error) = SettingsValidator.ValidateFilePath(txtDbAbsolutePath.Text);
                if (!valid) {
                    lblMensaje.Text = $"Ruta Absoluta: {error}";
                    tabControl.SelectedIndex = 0;
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtDbRemotePath.Text)) {
                (valid, error) = SettingsValidator.ValidateFilePath(txtDbRemotePath.Text);
                if (!valid) {
                    lblMensaje.Text = $"Ruta Remota: {error}";
                    tabControl.SelectedIndex = 0;
                    return false;
                }
            }

            // Validar Base de Datos - Credenciales
            (valid, error) = SettingsValidator.ValidateDbUser(txtDbUser.Text);
            if (!valid) {
                lblMensaje.Text = error;
                tabControl.SelectedIndex = 0;
                return false;
            }

            (valid, error) = SettingsValidator.ValidateDbPassword(txtDbPass.Text);
            if (!valid) {
                lblMensaje.Text = error;
                tabControl.SelectedIndex = 0;
                return false;
            }

            (valid, error) = SettingsValidator.ValidateDbCharset(cmbDbCharset.Text);
            if (!valid) {
                lblMensaje.Text = error;
                tabControl.SelectedIndex = 0;
                return false;
            }

            (valid, error) = SettingsValidator.ValidateDbDialect(txtDbDialect.Text);
            if (!valid) {
                lblMensaje.Text = error;
                tabControl.SelectedIndex = 0;
                return false;
            }

            // Validar Aplicación
            (valid, error) = SettingsValidator.ValidateNumericRange(txtCodSala.Text, "Código Sala", 1, 9999);
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
                // Guardar todos los valores
                Program.GuardarConfiguracion(
                    dbModo: cmbDbModo.Text,
                    dbSource: txtDbSource.Text,
                    dbPort: txtDbPort.Text,
                    dbRelativePath: txtDbRelativePath.Text,
                    dbAbsolutePath: txtDbAbsolutePath.Text,
                    dbRemotePath: txtDbRemotePath.Text,
                    dbUser: txtDbUser.Text,
                    dbPass: txtDbPass.Text,
                    dbCharset: cmbDbCharset.Text,
                    dbDialect: txtDbDialect.Text,
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
