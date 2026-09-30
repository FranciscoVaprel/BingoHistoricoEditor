namespace BingoHistoricoEditor {
    partial class SettingsForm {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));

            btnGuardar = new System.Windows.Forms.Button();
            btnCancelar = new System.Windows.Forms.Button();
            tabControl = new System.Windows.Forms.TabControl();
            tabBaseDatos = new System.Windows.Forms.TabPage();
            grpConexion = new System.Windows.Forms.GroupBox();
            lblDbSource = new System.Windows.Forms.Label();
            txtDbSource = new System.Windows.Forms.TextBox();
            lblDbPort = new System.Windows.Forms.Label();
            txtDbPort = new System.Windows.Forms.TextBox();
            lblDbModo = new System.Windows.Forms.Label();
            cmbDbModo = new System.Windows.Forms.ComboBox();
            grpRutas = new System.Windows.Forms.GroupBox();
            lblDbAbsolutePath = new System.Windows.Forms.Label();
            txtDbAbsolutePath = new System.Windows.Forms.TextBox();
            lblDbRemotePath = new System.Windows.Forms.Label();
            txtDbRemotePath = new System.Windows.Forms.TextBox();
            lblDbRelativePath = new System.Windows.Forms.Label();
            txtDbRelativePath = new System.Windows.Forms.TextBox();
            btnBrowseRelativePath = new System.Windows.Forms.Button();
            btnBrowseAbsolutePath = new System.Windows.Forms.Button();
            grpCredenciales = new System.Windows.Forms.GroupBox();
            lblDbPass = new System.Windows.Forms.Label();
            txtDbPass = new System.Windows.Forms.TextBox();
            lblDbUser = new System.Windows.Forms.Label();
            txtDbUser = new System.Windows.Forms.TextBox();
            lblDbCharset = new System.Windows.Forms.Label();
            cmbDbCharset = new System.Windows.Forms.ComboBox();
            lblDbDialect = new System.Windows.Forms.Label();
            txtDbDialect = new System.Windows.Forms.TextBox();

            tabAplicacion = new System.Windows.Forms.TabPage();
            grpAplicacion = new System.Windows.Forms.GroupBox();
            lblCodSala = new System.Windows.Forms.Label();
            txtCodSala = new System.Windows.Forms.TextBox();
            lblCodConc = new System.Windows.Forms.Label();
            txtCodConc = new System.Windows.Forms.TextBox();
            lblAppUser = new System.Windows.Forms.Label();
            txtAppUser = new System.Windows.Forms.TextBox();
            lblPrecioPartida = new System.Windows.Forms.Label();
            txtPrecioPartida = new System.Windows.Forms.TextBox();

            lblMensaje = new System.Windows.Forms.Label();

            components = new System.ComponentModel.Container();
            toolTip = new System.Windows.Forms.ToolTip(components);

            tabControl.SuspendLayout();
            tabBaseDatos.SuspendLayout();
            grpConexion.SuspendLayout();
            grpRutas.SuspendLayout();
            grpCredenciales.SuspendLayout();
            tabAplicacion.SuspendLayout();
            grpAplicacion.SuspendLayout();
            SuspendLayout();

            // ─────────────────────────────────────────────────────────────
            // btnGuardar
            btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            btnGuardar.ForeColor = System.Drawing.SystemColors.ControlText;
            btnGuardar.Location = new System.Drawing.Point(308, 525);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new System.Drawing.Size(110, 35);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "💾 Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;

            // btnCancelar
            btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            btnCancelar.ForeColor = System.Drawing.SystemColors.ControlText;
            btnCancelar.Location = new System.Drawing.Point(424, 525);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new System.Drawing.Size(110, 35);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "✕ Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;

            // tabControl
            tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl.Controls.Add(tabBaseDatos);
            tabControl.Controls.Add(tabAplicacion);
            tabControl.Location = new System.Drawing.Point(12, 12);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(522, 505);
            tabControl.TabIndex = 2;

            // ─────────────────────────────────────────────────────────────
            // tabBaseDatos
            tabBaseDatos.BackColor = System.Drawing.SystemColors.Control;
            tabBaseDatos.Controls.Add(grpConexion);
            tabBaseDatos.Controls.Add(grpRutas);
            tabBaseDatos.Controls.Add(grpCredenciales);
            tabBaseDatos.ForeColor = System.Drawing.SystemColors.ControlText;
            tabBaseDatos.Location = new System.Drawing.Point(4, 24);
            tabBaseDatos.Name = "tabBaseDatos";
            tabBaseDatos.Padding = new System.Windows.Forms.Padding(10);
            tabBaseDatos.Size = new System.Drawing.Size(514, 477);
            tabBaseDatos.TabIndex = 0;
            tabBaseDatos.Text = "Base de Datos";

            // grpConexion
            grpConexion.Controls.Add(lblDbModo);
            grpConexion.Controls.Add(cmbDbModo);
            grpConexion.Controls.Add(lblDbSource);
            grpConexion.Controls.Add(txtDbSource);
            grpConexion.Controls.Add(lblDbPort);
            grpConexion.Controls.Add(txtDbPort);
            grpConexion.ForeColor = System.Drawing.SystemColors.ControlText;
            grpConexion.Location = new System.Drawing.Point(13, 13);
            grpConexion.Name = "grpConexion";
            grpConexion.Size = new System.Drawing.Size(488, 120);
            grpConexion.TabIndex = 0;
            grpConexion.TabStop = false;
            grpConexion.Text = " Conexión ";

            // lblDbModo
            lblDbModo.ForeColor = System.Drawing.SystemColors.ControlText;
            lblDbModo.Location = new System.Drawing.Point(10, 25);
            lblDbModo.Name = "lblDbModo";
            lblDbModo.Size = new System.Drawing.Size(100, 20);
            lblDbModo.TabIndex = 0;
            lblDbModo.Text = "Modo BD:";

            cmbDbModo.BackColor = System.Drawing.SystemColors.Window;
            cmbDbModo.ForeColor = System.Drawing.SystemColors.WindowText;
            cmbDbModo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbDbModo.Items.AddRange(new object[] { "Local", "Remoto", "Alias" });
            cmbDbModo.Location = new System.Drawing.Point(120, 25);
            cmbDbModo.Name = "cmbDbModo";
            cmbDbModo.Size = new System.Drawing.Size(350, 23);
            cmbDbModo.TabIndex = 1;
            toolTip.SetToolTip(cmbDbModo, "Modo de conexión. 'Local': usa Ruta Absoluta si no está vacía; si no, Ruta Relativa.\n'Remoto'/'Alias': usa solo Ruta Remota, ignorando Ruta Relativa y Ruta Absoluta.");

            // lblDbSource
            lblDbSource.ForeColor = System.Drawing.SystemColors.ControlText;
            lblDbSource.Location = new System.Drawing.Point(10, 55);
            lblDbSource.Name = "lblDbSource";
            lblDbSource.Size = new System.Drawing.Size(100, 20);
            lblDbSource.TabIndex = 2;
            lblDbSource.Text = "Servidor:";

            txtDbSource.BackColor = System.Drawing.SystemColors.Window;
            txtDbSource.ForeColor = System.Drawing.SystemColors.WindowText;
            txtDbSource.Location = new System.Drawing.Point(120, 55);
            txtDbSource.Name = "txtDbSource";
            txtDbSource.Size = new System.Drawing.Size(350, 23);
            txtDbSource.TabIndex = 3;
            toolTip.SetToolTip(txtDbSource, "Host o IP del servidor Firebird. Se usa siempre, sea cual sea el Modo BD.");

            // lblDbPort
            lblDbPort.ForeColor = System.Drawing.SystemColors.ControlText;
            lblDbPort.Location = new System.Drawing.Point(10, 85);
            lblDbPort.Name = "lblDbPort";
            lblDbPort.Size = new System.Drawing.Size(100, 20);
            lblDbPort.TabIndex = 4;
            lblDbPort.Text = "Puerto:";

            txtDbPort.BackColor = System.Drawing.SystemColors.Window;
            txtDbPort.ForeColor = System.Drawing.SystemColors.WindowText;
            txtDbPort.Location = new System.Drawing.Point(120, 85);
            txtDbPort.Name = "txtDbPort";
            txtDbPort.Size = new System.Drawing.Size(350, 23);
            txtDbPort.TabIndex = 5;
            toolTip.SetToolTip(txtDbPort, "Puerto del servicio Firebird (por defecto 3050).");

            // grpRutas
            grpRutas.Controls.Add(lblDbRelativePath);
            grpRutas.Controls.Add(txtDbRelativePath);
            grpRutas.Controls.Add(btnBrowseRelativePath);
            grpRutas.Controls.Add(lblDbRemotePath);
            grpRutas.Controls.Add(txtDbRemotePath);
            grpRutas.Controls.Add(lblDbAbsolutePath);
            grpRutas.Controls.Add(txtDbAbsolutePath);
            grpRutas.Controls.Add(btnBrowseAbsolutePath);
            grpRutas.ForeColor = System.Drawing.SystemColors.ControlText;
            grpRutas.Location = new System.Drawing.Point(13, 140);
            grpRutas.Name = "grpRutas";
            grpRutas.Size = new System.Drawing.Size(488, 130);
            grpRutas.TabIndex = 1;
            grpRutas.TabStop = false;
            grpRutas.Text = " Rutas ";

            // lblDbRelativePath
            lblDbRelativePath.ForeColor = System.Drawing.SystemColors.ControlText;
            lblDbRelativePath.Location = new System.Drawing.Point(10, 25);
            lblDbRelativePath.Name = "lblDbRelativePath";
            lblDbRelativePath.Size = new System.Drawing.Size(100, 20);
            lblDbRelativePath.TabIndex = 0;
            lblDbRelativePath.Text = "Ruta Relativa:";

            txtDbRelativePath.BackColor = System.Drawing.SystemColors.Window;
            txtDbRelativePath.ForeColor = System.Drawing.SystemColors.WindowText;
            txtDbRelativePath.Location = new System.Drawing.Point(120, 25);
            txtDbRelativePath.Name = "txtDbRelativePath";
            txtDbRelativePath.Size = new System.Drawing.Size(320, 23);
            txtDbRelativePath.TabIndex = 1;
            toolTip.SetToolTip(txtDbRelativePath, "Ruta relativa al fichero .fdb, tomada desde la carpeta de la aplicación.\nSolo se usa en modo Local, y solo si Ruta Absoluta está vacía.");

            btnBrowseRelativePath.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            btnBrowseRelativePath.ForeColor = System.Drawing.SystemColors.ControlText;
            btnBrowseRelativePath.Location = new System.Drawing.Point(445, 25);
            btnBrowseRelativePath.Name = "btnBrowseRelativePath";
            btnBrowseRelativePath.Size = new System.Drawing.Size(33, 23);
            btnBrowseRelativePath.TabIndex = 8;
            btnBrowseRelativePath.Text = "…";
            btnBrowseRelativePath.UseVisualStyleBackColor = true;
            toolTip.SetToolTip(btnBrowseRelativePath, "Buscar un fichero .fdb y calcular su ruta relativa a la carpeta de la aplicación.");
            btnBrowseRelativePath.Click += btnBrowseRelativePath_Click;

            // lblDbAbsolutePath
            lblDbAbsolutePath.ForeColor = System.Drawing.SystemColors.ControlText;
            lblDbAbsolutePath.Location = new System.Drawing.Point(10, 55);
            lblDbAbsolutePath.Name = "lblDbAbsolutePath";
            lblDbAbsolutePath.Size = new System.Drawing.Size(100, 20);
            lblDbAbsolutePath.TabIndex = 2;
            lblDbAbsolutePath.Text = "Ruta Absoluta:";

            txtDbAbsolutePath.BackColor = System.Drawing.SystemColors.Window;
            txtDbAbsolutePath.ForeColor = System.Drawing.SystemColors.WindowText;
            txtDbAbsolutePath.Location = new System.Drawing.Point(120, 55);
            txtDbAbsolutePath.Name = "txtDbAbsolutePath";
            txtDbAbsolutePath.Size = new System.Drawing.Size(320, 23);
            txtDbAbsolutePath.TabIndex = 3;
            toolTip.SetToolTip(txtDbAbsolutePath, "Ruta absoluta al fichero .fdb. En modo Local tiene prioridad sobre la Ruta Relativa:\nsi no está vacía, se usa esta y la Ruta Relativa se ignora por completo.");

            btnBrowseAbsolutePath.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            btnBrowseAbsolutePath.ForeColor = System.Drawing.SystemColors.ControlText;
            btnBrowseAbsolutePath.Location = new System.Drawing.Point(445, 55);
            btnBrowseAbsolutePath.Name = "btnBrowseAbsolutePath";
            btnBrowseAbsolutePath.Size = new System.Drawing.Size(33, 23);
            btnBrowseAbsolutePath.TabIndex = 9;
            btnBrowseAbsolutePath.Text = "…";
            btnBrowseAbsolutePath.UseVisualStyleBackColor = true;
            toolTip.SetToolTip(btnBrowseAbsolutePath, "Buscar un fichero .fdb y usar su ruta completa.");
            btnBrowseAbsolutePath.Click += btnBrowseAbsolutePath_Click;

            // lblDbRemotePath
            lblDbRemotePath.ForeColor = System.Drawing.SystemColors.ControlText;
            lblDbRemotePath.Location = new System.Drawing.Point(10, 85);
            lblDbRemotePath.Name = "lblDbRemotePath";
            lblDbRemotePath.Size = new System.Drawing.Size(100, 20);
            lblDbRemotePath.TabIndex = 4;
            lblDbRemotePath.Text = "Ruta Remota:";

            txtDbRemotePath.BackColor = System.Drawing.SystemColors.Window;
            txtDbRemotePath.ForeColor = System.Drawing.SystemColors.WindowText;
            txtDbRemotePath.Location = new System.Drawing.Point(120, 85);
            txtDbRemotePath.Name = "txtDbRemotePath";
            txtDbRemotePath.Size = new System.Drawing.Size(350, 23);
            txtDbRemotePath.TabIndex = 5;
            toolTip.SetToolTip(txtDbRemotePath, "Ruta o alias de la base de datos remota. Solo se usa cuando el Modo BD es\n'Remoto' o 'Alias'; en ese caso se ignoran la Ruta Relativa y la Ruta Absoluta.");

            // grpCredenciales
            grpCredenciales.Controls.Add(lblDbDialect);
            grpCredenciales.Controls.Add(txtDbDialect);
            grpCredenciales.Controls.Add(lblDbCharset);
            grpCredenciales.Controls.Add(cmbDbCharset);
            grpCredenciales.Controls.Add(lblDbUser);
            grpCredenciales.Controls.Add(txtDbUser);
            grpCredenciales.Controls.Add(lblDbPass);
            grpCredenciales.Controls.Add(txtDbPass);
            grpCredenciales.ForeColor = System.Drawing.SystemColors.ControlText;
            grpCredenciales.Location = new System.Drawing.Point(13, 277);
            grpCredenciales.Name = "grpCredenciales";
            grpCredenciales.Size = new System.Drawing.Size(488, 160);
            grpCredenciales.TabIndex = 2;
            grpCredenciales.TabStop = false;
            grpCredenciales.Text = " Credenciales BD ";

            // lblDbUser
            lblDbUser.ForeColor = System.Drawing.SystemColors.ControlText;
            lblDbUser.Location = new System.Drawing.Point(10, 25);
            lblDbUser.Name = "lblDbUser";
            lblDbUser.Size = new System.Drawing.Size(100, 20);
            lblDbUser.TabIndex = 0;
            lblDbUser.Text = "Usuario:";

            txtDbUser.BackColor = System.Drawing.SystemColors.Window;
            txtDbUser.ForeColor = System.Drawing.SystemColors.WindowText;
            txtDbUser.Location = new System.Drawing.Point(120, 25);
            txtDbUser.Name = "txtDbUser";
            txtDbUser.Size = new System.Drawing.Size(350, 23);
            txtDbUser.TabIndex = 1;
            toolTip.SetToolTip(txtDbUser, "Usuario de conexión a Firebird (por defecto SYSDBA).\nNo es el mismo usuario que se usa para entrar en la aplicación.");

            // lblDbPass
            lblDbPass.ForeColor = System.Drawing.SystemColors.ControlText;
            lblDbPass.Location = new System.Drawing.Point(10, 55);
            lblDbPass.Name = "lblDbPass";
            lblDbPass.Size = new System.Drawing.Size(100, 20);
            lblDbPass.TabIndex = 2;
            lblDbPass.Text = "Contraseña:";

            txtDbPass.BackColor = System.Drawing.SystemColors.Window;
            txtDbPass.ForeColor = System.Drawing.SystemColors.WindowText;
            txtDbPass.Location = new System.Drawing.Point(120, 55);
            txtDbPass.Name = "txtDbPass";
            txtDbPass.Size = new System.Drawing.Size(350, 23);
            txtDbPass.TabIndex = 3;
            txtDbPass.UseSystemPasswordChar = true;
            toolTip.SetToolTip(txtDbPass, "Contraseña de conexión a Firebird. Se guarda en texto plano en appsettings.json.");

            // lblDbCharset
            lblDbCharset.ForeColor = System.Drawing.SystemColors.ControlText;
            lblDbCharset.Location = new System.Drawing.Point(10, 85);
            lblDbCharset.Name = "lblDbCharset";
            lblDbCharset.Size = new System.Drawing.Size(100, 20);
            lblDbCharset.TabIndex = 4;
            lblDbCharset.Text = "Charset:";

            cmbDbCharset.BackColor = System.Drawing.SystemColors.Window;
            cmbDbCharset.ForeColor = System.Drawing.SystemColors.WindowText;
            cmbDbCharset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbDbCharset.Items.AddRange(new object[] { "UTF8", "ISO8859_1", "WIN1252" });
            cmbDbCharset.Location = new System.Drawing.Point(120, 85);
            cmbDbCharset.Name = "cmbDbCharset";
            cmbDbCharset.Size = new System.Drawing.Size(350, 23);
            cmbDbCharset.TabIndex = 5;
            toolTip.SetToolTip(cmbDbCharset, "Juego de caracteres de la conexión; debe coincidir con el de la base de datos\n(normalmente ISO8859_1).");

            // lblDbDialect
            lblDbDialect.ForeColor = System.Drawing.SystemColors.ControlText;
            lblDbDialect.Location = new System.Drawing.Point(10, 115);
            lblDbDialect.Name = "lblDbDialect";
            lblDbDialect.Size = new System.Drawing.Size(100, 20);
            lblDbDialect.TabIndex = 6;
            lblDbDialect.Text = "Dialecto:";

            txtDbDialect.BackColor = System.Drawing.SystemColors.Window;
            txtDbDialect.ForeColor = System.Drawing.SystemColors.WindowText;
            txtDbDialect.Location = new System.Drawing.Point(120, 115);
            txtDbDialect.Name = "txtDbDialect";
            txtDbDialect.Size = new System.Drawing.Size(350, 23);
            txtDbDialect.TabIndex = 7;
            toolTip.SetToolTip(txtDbDialect, "Dialecto SQL de Firebird: '1' (heredado) o '3' (el habitual en bases modernas).");

            // ─────────────────────────────────────────────────────────────
            // tabAplicacion
            tabAplicacion.BackColor = System.Drawing.SystemColors.Control;
            tabAplicacion.Controls.Add(grpAplicacion);
            tabAplicacion.ForeColor = System.Drawing.SystemColors.ControlText;
            tabAplicacion.Location = new System.Drawing.Point(4, 24);
            tabAplicacion.Name = "tabAplicacion";
            tabAplicacion.Padding = new System.Windows.Forms.Padding(10);
            tabAplicacion.Size = new System.Drawing.Size(514, 477);
            tabAplicacion.TabIndex = 1;
            tabAplicacion.Text = "Aplicación";

            // grpAplicacion
            grpAplicacion.Controls.Add(lblPrecioPartida);
            grpAplicacion.Controls.Add(txtPrecioPartida);
            grpAplicacion.Controls.Add(lblAppUser);
            grpAplicacion.Controls.Add(txtAppUser);
            grpAplicacion.Controls.Add(lblCodConc);
            grpAplicacion.Controls.Add(txtCodConc);
            grpAplicacion.Controls.Add(lblCodSala);
            grpAplicacion.Controls.Add(txtCodSala);
            grpAplicacion.ForeColor = System.Drawing.SystemColors.ControlText;
            grpAplicacion.Location = new System.Drawing.Point(13, 13);
            grpAplicacion.Name = "grpAplicacion";
            grpAplicacion.Size = new System.Drawing.Size(488, 180);
            grpAplicacion.TabIndex = 0;
            grpAplicacion.TabStop = false;
            grpAplicacion.Text = " Configuración ";

            // lblCodSala
            lblCodSala.ForeColor = System.Drawing.SystemColors.ControlText;
            lblCodSala.Location = new System.Drawing.Point(10, 25);
            lblCodSala.Name = "lblCodSala";
            lblCodSala.Size = new System.Drawing.Size(100, 20);
            lblCodSala.TabIndex = 0;
            lblCodSala.Text = "Código Sala:";

            txtCodSala.BackColor = System.Drawing.SystemColors.Window;
            txtCodSala.ForeColor = System.Drawing.SystemColors.WindowText;
            txtCodSala.Location = new System.Drawing.Point(120, 25);
            txtCodSala.Name = "txtCodSala";
            txtCodSala.Size = new System.Drawing.Size(350, 23);
            txtCodSala.TabIndex = 1;
            toolTip.SetToolTip(txtCodSala, "Código de sala (COD_SALA) usado para filtrar y guardar todas las partidas en HISTORICO.");

            // lblCodConc
            lblCodConc.ForeColor = System.Drawing.SystemColors.ControlText;
            lblCodConc.Location = new System.Drawing.Point(10, 55);
            lblCodConc.Name = "lblCodConc";
            lblCodConc.Size = new System.Drawing.Size(100, 20);
            lblCodConc.TabIndex = 2;
            lblCodConc.Text = "Código Conc:";

            txtCodConc.BackColor = System.Drawing.SystemColors.Window;
            txtCodConc.ForeColor = System.Drawing.SystemColors.WindowText;
            txtCodConc.Location = new System.Drawing.Point(120, 55);
            txtCodConc.Name = "txtCodConc";
            txtCodConc.Size = new System.Drawing.Size(350, 23);
            txtCodConc.TabIndex = 3;
            toolTip.SetToolTip(txtCodConc, "Código de concesionario (COD_CONC) usado para filtrar y guardar todas las partidas en HISTORICO.");

            // lblAppUser
            lblAppUser.ForeColor = System.Drawing.SystemColors.ControlText;
            lblAppUser.Location = new System.Drawing.Point(10, 85);
            lblAppUser.Name = "lblAppUser";
            lblAppUser.Size = new System.Drawing.Size(100, 20);
            lblAppUser.TabIndex = 4;
            lblAppUser.Text = "Usuario App:";

            txtAppUser.BackColor = System.Drawing.SystemColors.Window;
            txtAppUser.ForeColor = System.Drawing.SystemColors.WindowText;
            txtAppUser.Location = new System.Drawing.Point(120, 85);
            txtAppUser.Name = "txtAppUser";
            txtAppUser.Size = new System.Drawing.Size(350, 23);
            txtAppUser.TabIndex = 5;
            toolTip.SetToolTip(txtAppUser, "Usuario heredado de la configuración inicial. Ya no se usa para iniciar sesión\n(los usuarios se gestionan desde 'Gestionar usuarios' en el login); cambiarlo aquí\nno afecta a ningún usuario existente.");

            // lblPrecioPartida
            lblPrecioPartida.ForeColor = System.Drawing.SystemColors.ControlText;
            lblPrecioPartida.Location = new System.Drawing.Point(10, 115);
            lblPrecioPartida.Name = "lblPrecioPartida";
            lblPrecioPartida.Size = new System.Drawing.Size(100, 20);
            lblPrecioPartida.TabIndex = 6;
            lblPrecioPartida.Text = "Precio Partida:";

            txtPrecioPartida.BackColor = System.Drawing.SystemColors.Window;
            txtPrecioPartida.ForeColor = System.Drawing.SystemColors.WindowText;
            txtPrecioPartida.Location = new System.Drawing.Point(120, 115);
            txtPrecioPartida.Name = "txtPrecioPartida";
            txtPrecioPartida.Size = new System.Drawing.Size(350, 23);
            txtPrecioPartida.TabIndex = 7;
            toolTip.SetToolTip(txtPrecioPartida, "Precio de partida propuesto por defecto al crear una partida nueva o un lote;\nse puede cambiar manualmente en cada pantalla.");

            // lblMensaje
            lblMensaje.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblMensaje.ForeColor = System.Drawing.Color.Red;
            lblMensaje.Location = new System.Drawing.Point(12, 525);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new System.Drawing.Size(290, 35);
            lblMensaje.TabIndex = 3;
            lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // SettingsForm
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            CancelButton = btnCancelar;
            ClientSize = new System.Drawing.Size(546, 572);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            Controls.Add(tabControl);
            Controls.Add(lblMensaje);
            ForeColor = System.Drawing.SystemColors.ControlText;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            // Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Configuración";
            tabControl.ResumeLayout(false);
            tabBaseDatos.ResumeLayout(false);
            grpConexion.ResumeLayout(false);
            grpConexion.PerformLayout();
            grpRutas.ResumeLayout(false);
            grpRutas.PerformLayout();
            grpCredenciales.ResumeLayout(false);
            grpCredenciales.PerformLayout();
            tabAplicacion.ResumeLayout(false);
            grpAplicacion.ResumeLayout(false);
            grpAplicacion.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabBaseDatos;
        private System.Windows.Forms.TabPage tabAplicacion;
        private System.Windows.Forms.GroupBox grpConexion;
        private System.Windows.Forms.GroupBox grpRutas;
        private System.Windows.Forms.GroupBox grpCredenciales;
        private System.Windows.Forms.GroupBox grpAplicacion;
        private System.Windows.Forms.Label lblDbModo;
        private System.Windows.Forms.ComboBox cmbDbModo;
        private System.Windows.Forms.Label lblDbSource;
        private System.Windows.Forms.TextBox txtDbSource;
        private System.Windows.Forms.Label lblDbPort;
        private System.Windows.Forms.TextBox txtDbPort;
        private System.Windows.Forms.Label lblDbRelativePath;
        private System.Windows.Forms.TextBox txtDbRelativePath;
        private System.Windows.Forms.Button btnBrowseRelativePath;
        private System.Windows.Forms.Button btnBrowseAbsolutePath;
        private System.Windows.Forms.Label lblDbAbsolutePath;
        private System.Windows.Forms.TextBox txtDbAbsolutePath;
        private System.Windows.Forms.Label lblDbRemotePath;
        private System.Windows.Forms.TextBox txtDbRemotePath;
        private System.Windows.Forms.Label lblDbUser;
        private System.Windows.Forms.TextBox txtDbUser;
        private System.Windows.Forms.Label lblDbPass;
        private System.Windows.Forms.TextBox txtDbPass;
        private System.Windows.Forms.Label lblDbCharset;
        private System.Windows.Forms.ComboBox cmbDbCharset;
        private System.Windows.Forms.Label lblDbDialect;
        private System.Windows.Forms.TextBox txtDbDialect;
        private System.Windows.Forms.Label lblCodSala;
        private System.Windows.Forms.TextBox txtCodSala;
        private System.Windows.Forms.Label lblCodConc;
        private System.Windows.Forms.TextBox txtCodConc;
        private System.Windows.Forms.Label lblAppUser;
        private System.Windows.Forms.TextBox txtAppUser;
        private System.Windows.Forms.Label lblPrecioPartida;
        private System.Windows.Forms.TextBox txtPrecioPartida;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
