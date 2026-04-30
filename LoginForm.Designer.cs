namespace BingoHistoricoEditor {
    partial class LoginForm {
        private System.ComponentModel.IContainer components = null;

        private Panel        pnlCentro;
        private Label        lblTitulo;
        private Label        lblSubtitulo;
        private Label        lblPasswordLabel;
        private TextBox      txtPassword;
        private Button       btnEntrar;
        private Label        lblError;

        protected override void Dispose(bool disposing) {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            var clrFondo    = Color.FromArgb(18,  20,  28);
            var clrPanel    = Color.FromArgb(26,  29,  40);
            var clrBorde    = Color.FromArgb(48,  54,  72);
            var clrTexto    = Color.FromArgb(220, 225, 240);
            var clrSubtexto = Color.FromArgb(130, 140, 165);
            var clrAcento   = Color.FromArgb( 99, 179, 237);
            var clrVerde    = Color.FromArgb( 72, 187, 120);
            var clrRojo     = Color.FromArgb(220,  80,  80);

            SuspendLayout();

            // ── Formulario ────────────────────────────────────────────────────
            Text            = "Acceso — Histórico Bingo";
            Size            = new Size(360, 300);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            StartPosition   = FormStartPosition.CenterScreen;
            BackColor       = clrFondo;
            ForeColor       = clrTexto;
            Font            = new Font("Segoe UI", 9f);

            // ── Panel central ─────────────────────────────────────────────────
            pnlCentro = new Panel {
                Size      = new Size(300, 220),
                Location  = new Point(30, 30),
                BackColor = clrPanel,
                Padding   = new Padding(24),
            };

            // Borde redondeado visual con Paint
            pnlCentro.Paint += (s, e) => {
                using var pen = new Pen(clrBorde, 1);
                e.Graphics.DrawRectangle(pen, 0, 0, pnlCentro.Width - 1, pnlCentro.Height - 1);
            };

            lblTitulo = new Label {
                Text      = "HISTÓRICO BINGO",
                Font      = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = clrAcento,
                AutoSize  = false,
                Width     = 252,
                Height    = 28,
                Location  = new Point(24, 24),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            lblSubtitulo = new Label {
                Text      = "Introduce la contraseña para continuar",
                Font      = new Font("Segoe UI", 8f),
                ForeColor = clrSubtexto,
                AutoSize  = false,
                Width     = 252,
                Height    = 20,
                Location  = new Point(24, 54),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            lblPasswordLabel = new Label {
                Text      = "Contraseña",
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = clrSubtexto,
                AutoSize  = true,
                Location  = new Point(24, 90),
            };

            txtPassword = new TextBox {
                UseSystemPasswordChar = true,
                Width      = 252,
                Location   = new Point(24, 110),
                BackColor  = Color.FromArgb(32, 36, 50),
                ForeColor  = clrTexto,
                BorderStyle= BorderStyle.FixedSingle,
                Font       = new Font("Segoe UI", 10f),
            };
            txtPassword.KeyDown += txtPassword_KeyDown;

            lblError = new Label {
                Text      = "",
                Font      = new Font("Segoe UI", 8f),
                ForeColor = clrRojo,
                AutoSize  = false,
                Width     = 252,
                Height    = 18,
                Location  = new Point(24, 140),
                Visible   = false,
            };

            btnEntrar = new Button {
                Text      = "Entrar",
                Width     = 252,
                Height    = 36,
                Location  = new Point(24, 162),
                BackColor = clrVerde,
                ForeColor = clrFondo,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor    = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 },
            };
            btnEntrar.Click += btnEntrar_Click;

            pnlCentro.Controls.AddRange(new Control[] {
                lblTitulo, lblSubtitulo, lblPasswordLabel,
                txtPassword, lblError, btnEntrar
            });

            Controls.Add(pnlCentro);

            ResumeLayout(false);
        }
    }
}
