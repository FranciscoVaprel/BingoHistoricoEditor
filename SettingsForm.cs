using Microsoft.Extensions.Configuration;
using System;
using System.IO;
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

            // Validar Base de Datos
            var (valid, error) = SettingsValidator.ValidateDbModo(cmbDbModo.Text);
            if (!valid) return MostrarErrorBD(error);

            (valid, error) = SettingsValidator.ValidateDbSource(txtDbSource.Text);
            if (!valid) return MostrarErrorBD(error);

            (valid, error) = SettingsValidator.ValidateDbPort(txtDbPort.Text);
            if (!valid) return MostrarErrorBD(error);

            bool esRemoto = cmbDbModo.Text.Equals("Remoto", StringComparison.OrdinalIgnoreCase)
                         || cmbDbModo.Text.Equals("Alias", StringComparison.OrdinalIgnoreCase);

            if (esRemoto) {
                if (string.IsNullOrWhiteSpace(txtDbRemotePath.Text))
                    return MostrarErrorBD("La Ruta Remota no puede estar vacía cuando el Modo BD es Remoto o Alias.");
            }
            else if (string.IsNullOrWhiteSpace(txtDbAbsolutePath.Text) && string.IsNullOrWhiteSpace(txtDbRelativePath.Text)) {
                return MostrarErrorBD("Debes indicar una Ruta Absoluta o una Ruta Relativa cuando el Modo BD es Local.");
            }

            if (!string.IsNullOrWhiteSpace(txtDbRelativePath.Text)) {
                (valid, error) = SettingsValidator.ValidateFilePath(txtDbRelativePath.Text);
                if (!valid) return MostrarErrorBD(error);
            }
            if (!string.IsNullOrWhiteSpace(txtDbAbsolutePath.Text)) {
                (valid, error) = SettingsValidator.ValidateFilePath(txtDbAbsolutePath.Text);
                if (!valid) return MostrarErrorBD(error);
            }

            (valid, error) = SettingsValidator.ValidateDbUser(txtDbUser.Text);
            if (!valid) return MostrarErrorBD(error);

            (valid, error) = SettingsValidator.ValidateDbPassword(txtDbPass.Text);
            if (!valid) return MostrarErrorBD(error);

            (valid, error) = SettingsValidator.ValidateDbCharset(cmbDbCharset.Text);
            if (!valid) return MostrarErrorBD(error);

            (valid, error) = SettingsValidator.ValidateDbDialect(txtDbDialect.Text);
            if (!valid) return MostrarErrorBD(error);

            // Validar Aplicación
            (valid, error) = SettingsValidator.ValidateNumericRange(txtCodSala.Text, "Código Sala", 1, 9999);
            if (!valid) return MostrarErrorAplicacion(error);

            (valid, error) = SettingsValidator.ValidateNumericRange(txtCodConc.Text, "Código Conc", 1, 9999);
            if (!valid) return MostrarErrorAplicacion(error);

            (valid, error) = SettingsValidator.ValidateUsername(txtAppUser.Text);
            if (!valid) return MostrarErrorAplicacion(error);

            (valid, error) = SettingsValidator.ValidateDecimalRange(txtPrecioPartida.Text, "Precio Partida", 0, 9999);
            if (!valid) return MostrarErrorAplicacion(error);

            return true;
        }

        private bool MostrarErrorBD(string error) {
            lblMensaje.Text = error;
            tabControl.SelectedIndex = 0;
            return false;
        }

        private bool MostrarErrorAplicacion(string error) {
            lblMensaje.Text = error;
            tabControl.SelectedIndex = 1;
            return false;
        }

        private void btnBrowseRelativePath_Click(object sender, EventArgs e) {
            string? seleccionado = SeleccionarFicheroFdb();
            if (seleccionado is null) return;

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            txtDbRelativePath.Text = Path.GetRelativePath(baseDir, seleccionado);
        }

        private void btnBrowseAbsolutePath_Click(object sender, EventArgs e) {
            string? seleccionado = SeleccionarFicheroFdb();
            if (seleccionado is null) return;

            txtDbAbsolutePath.Text = seleccionado;
        }

        /// <summary>
        /// Abre un diálogo para elegir un fichero .fdb. Permite seleccionar rutas que
        /// todavía no existan, ya que Firebird crea el fichero la primera vez que se conecta.
        /// </summary>
        private string? SeleccionarFicheroFdb() {
            using var dlg = new OpenFileDialog {
                Title = "Seleccionar base de datos Firebird",
                Filter = "Bases de datos Firebird (*.fdb)|*.fdb|Todos los archivos (*.*)|*.*",
                CheckFileExists = false,
                CheckPathExists = true,
            };
            return dlg.ShowDialog(this) == DialogResult.OK ? dlg.FileName : null;
        }

        private void btnGuardar_Click(object sender, EventArgs e) {
            if (!ValidarTodosLosValores()) {
                MessageBox.Show("Por favor, corrija los errores antes de guardar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {
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

                MessageBox.Show("Configuración guardada correctamente.\n\nLos cambios de base de datos requieren reiniciar la aplicación.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
