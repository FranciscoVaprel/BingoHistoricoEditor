namespace BingoHistoricoEditor {
    partial class SetPasswordForm {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label   lblTitulo;
        private System.Windows.Forms.Label   lblSubtitulo;
        private System.Windows.Forms.Label   lblUsuarioLabel;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label   lblNuevaLabel;
        private System.Windows.Forms.TextBox txtNueva;
        private System.Windows.Forms.Label   lblRepetirLabel;
        private System.Windows.Forms.TextBox txtRepetir;
        private System.Windows.Forms.Label   lblError;
        private System.Windows.Forms.Button  btnAceptar;
        private System.Windows.Forms.Button  btnCancelar;

        protected override void Dispose(bool disposing) {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            var fntNormal   = new System.Drawing.Font("Segoe UI", 9f);
            var fntInput    = new System.Drawing.Font("Segoe UI", 10f);

            this.SuspendLayout();

            this.Text            = "Configurar credenciales";
            this.Size            = new System.Drawing.Size(380, 420);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor       = System.Drawing.SystemColors.Control;
            this.ForeColor       = System.Drawing.SystemColors.ControlText;
            this.Font            = fntNormal;

            int cx = 40; int cw = 300;

            lblTitulo = new System.Windows.Forms.Label {
                Text = "Configurar acceso", AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 13f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.SystemColors.HotTrack,
                Location = new System.Drawing.Point(cx, 24),
            };
            lblSubtitulo = new System.Windows.Forms.Label {
                Text = "Introduce el usuario y contraseña de acceso.",
                AutoSize = true, Font = fntNormal, ForeColor = System.Drawing.SystemColors.GrayText,
                Location = new System.Drawing.Point(cx, 52),
            };

            lblUsuarioLabel = new System.Windows.Forms.Label {
                Text = "Usuario", AutoSize = true, ForeColor = System.Drawing.SystemColors.GrayText,
                Location = new System.Drawing.Point(cx, 90),
            };
            txtUsuario = new System.Windows.Forms.TextBox {
                Size = new System.Drawing.Size(cw, 28), Location = new System.Drawing.Point(cx, 110),
                BackColor = System.Drawing.SystemColors.Window, ForeColor = System.Drawing.SystemColors.WindowText,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle, Font = fntInput,
            };

            lblNuevaLabel = new System.Windows.Forms.Label {
                Text = "Nueva contraseña", AutoSize = true, ForeColor = System.Drawing.SystemColors.GrayText,
                Location = new System.Drawing.Point(cx, 152),
            };
            txtNueva = new System.Windows.Forms.TextBox {
                UseSystemPasswordChar = true,
                Size = new System.Drawing.Size(cw, 28), Location = new System.Drawing.Point(cx, 172),
                BackColor = System.Drawing.SystemColors.Window, ForeColor = System.Drawing.SystemColors.WindowText,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle, Font = fntInput,
            };

            lblRepetirLabel = new System.Windows.Forms.Label {
                Text = "Repite la contraseña", AutoSize = true, ForeColor = System.Drawing.SystemColors.GrayText,
                Location = new System.Drawing.Point(cx, 214),
            };
            txtRepetir = new System.Windows.Forms.TextBox {
                UseSystemPasswordChar = true,
                Size = new System.Drawing.Size(cw, 28), Location = new System.Drawing.Point(cx, 234),
                BackColor = System.Drawing.SystemColors.Window, ForeColor = System.Drawing.SystemColors.WindowText,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle, Font = fntInput,
            };

            lblError = new System.Windows.Forms.Label {
                Text = "", ForeColor = System.Drawing.Color.Red, Font = fntNormal, Visible = false,
                Size = new System.Drawing.Size(cw, 20), Location = new System.Drawing.Point(cx, 272),
            };

            btnAceptar = new System.Windows.Forms.Button {
                Text = "Guardar", Size = new System.Drawing.Size(144, 36),
                Location = new System.Drawing.Point(cx, 300),
                ForeColor = System.Drawing.SystemColors.ControlText,
                FlatStyle = System.Windows.Forms.FlatStyle.Standard,
                Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand,
                UseVisualStyleBackColor = true,
            };
            btnAceptar.Click += btnAceptar_Click;

            btnCancelar = new System.Windows.Forms.Button {
                Text = "Cancelar", Size = new System.Drawing.Size(100, 36),
                Location = new System.Drawing.Point(cx + 154, 300),
                ForeColor = System.Drawing.SystemColors.ControlText,
                FlatStyle = System.Windows.Forms.FlatStyle.Standard,
                Font = fntNormal, Cursor = System.Windows.Forms.Cursors.Hand,
                UseVisualStyleBackColor = true,
            };
            btnCancelar.Click += btnCancelar_Click;

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTitulo, lblSubtitulo,
                lblUsuarioLabel, txtUsuario,
                lblNuevaLabel, txtNueva,
                lblRepetirLabel, txtRepetir,
                lblError, btnAceptar, btnCancelar
            });
            this.ResumeLayout(false);
        }
    }
}
