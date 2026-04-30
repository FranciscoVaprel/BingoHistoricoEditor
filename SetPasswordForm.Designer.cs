namespace BingoHistoricoEditor {
    partial class SetPasswordForm {
        private System.ComponentModel.IContainer components = null;

        private Label   lblTitulo;
        private Label   lblSubtitulo;
        private Label   lblNuevaLabel;
        private TextBox txtNueva;
        private Label   lblRepetirLabel;
        private TextBox txtRepetir;
        private Label   lblError;
        private Button  btnAceptar;
        private Button  btnCancelar;

        protected override void Dispose(bool disposing) {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            var clrFondo    = Color.FromArgb(18,  20,  28);
            var clrPanel    = Color.FromArgb(26,  29,  40);
            var clrTexto    = Color.FromArgb(220, 225, 240);
            var clrSubtexto = Color.FromArgb(130, 140, 165);
            var clrAcento   = Color.FromArgb( 99, 179, 237);
            var clrVerde    = Color.FromArgb( 72, 187, 120);
            var clrRojo     = Color.FromArgb(220,  80,  80);
            var clrInput    = Color.FromArgb(32,   36,  50);

            SuspendLayout();

            Text            = "Establecer contraseña";
            Size            = new Size(360, 320);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            StartPosition   = FormStartPosition.CenterParent;
            BackColor       = clrFondo;
            ForeColor       = clrTexto;
            Font            = new Font("Segoe UI", 9f);

            int cx = 30; int cw = 290;

            lblTitulo = new Label {
                Text      = "Cambiar contraseña",
                Font      = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = clrAcento,
                Location  = new Point(cx, 24),
                AutoSize  = true,
            };

            lblSubtitulo = new Label {
                Text      = "Introduce y confirma la nueva contraseña.",
                Font      = new Font("Segoe UI", 8f),
                ForeColor = clrSubtexto,
                Location  = new Point(cx, 52),
                AutoSize  = true,
            };

            lblNuevaLabel = new Label {
                Text      = "Nueva contraseña",
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = clrSubtexto,
                Location  = new Point(cx, 88),
                AutoSize  = true,
            };

            txtNueva = new TextBox {
                UseSystemPasswordChar = true,
                Width      = cw, Location   = new Point(cx, 108),
                BackColor  = clrInput, ForeColor  = clrTexto,
                BorderStyle= BorderStyle.FixedSingle,
                Font       = new Font("Segoe UI", 10f),
            };

            lblRepetirLabel = new Label {
                Text      = "Repite la contraseña",
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = clrSubtexto,
                Location  = new Point(cx, 142),
                AutoSize  = true,
            };

            txtRepetir = new TextBox {
                UseSystemPasswordChar = true,
                Width      = cw, Location   = new Point(cx, 162),
                BackColor  = clrInput, ForeColor  = clrTexto,
                BorderStyle= BorderStyle.FixedSingle,
                Font       = new Font("Segoe UI", 10f),
            };

            lblError = new Label {
                Text      = "",
                Font      = new Font("Segoe UI", 8f),
                ForeColor = clrRojo,
                Location  = new Point(cx, 192),
                AutoSize  = false,
                Width     = cw, Height = 18,
                Visible   = false,
            };

            btnAceptar = new Button {
                Text      = "Guardar contraseña",
                Size      = new Size(140, 34),
                Location  = new Point(cx, 220),
                BackColor = clrVerde, ForeColor = clrFondo,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor    = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 },
            };
            btnAceptar.Click += btnAceptar_Click;

            btnCancelar = new Button {
                Text      = "Cancelar",
                Size      = new Size(90, 34),
                Location  = new Point(cx + 150, 220),
                BackColor = Color.FromArgb(48, 54, 72), ForeColor = clrTexto,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9f),
                Cursor    = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 },
            };
            btnCancelar.Click += btnCancelar_Click;

            Controls.AddRange(new Control[] {
                lblTitulo, lblSubtitulo,
                lblNuevaLabel, txtNueva,
                lblRepetirLabel, txtRepetir,
                lblError, btnAceptar, btnCancelar
            });

            ResumeLayout(false);
        }
    }
}
