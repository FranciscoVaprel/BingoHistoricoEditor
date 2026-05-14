namespace BingoHistoricoEditor {
    partial class LoginForm {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel  pnlCentro;
        private System.Windows.Forms.Label  lblTitulo;
        private System.Windows.Forms.Label  lblSubtitulo;
        private System.Windows.Forms.Label  lblUserLabel;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label  lblPasswordLabel;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnEntrar;
        private System.Windows.Forms.Button btnGestionar;
        private System.Windows.Forms.Label  lblError;

        protected override void Dispose(bool disposing) {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            var clrFondo    = System.Drawing.Color.FromArgb(18,  20,  28);
            var clrPanel    = System.Drawing.Color.FromArgb(26,  29,  40);
            var clrTexto    = System.Drawing.Color.FromArgb(220, 225, 240);
            var clrSubtexto = System.Drawing.Color.FromArgb(130, 140, 165);
            var clrVerde    = System.Drawing.Color.FromArgb( 72, 187, 120);
            var clrGris     = System.Drawing.Color.FromArgb( 48,  54,  72);
            var clrRojo     = System.Drawing.Color.FromArgb(220,  80,  80);

            this.SuspendLayout();

            // ── Formulario ────────────────────────────────────────────────────
            this.Text            = "Acceso — Histórico Bingo";
            this.Size            = new System.Drawing.Size(360, 400);  // un poco más alto
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor       = clrFondo;

            // ── Panel central ─────────────────────────────────────────────────
            this.pnlCentro = new System.Windows.Forms.Panel {
                Size      = new System.Drawing.Size(300, 340),
                Location  = new System.Drawing.Point(30, 30),
                BackColor = clrPanel,
            };

            this.lblTitulo = new System.Windows.Forms.Label {
                Text      = "BIENVENIDO",
                Font      = new System.Drawing.Font("Segoe UI", 16f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location  = new System.Drawing.Point(24, 20),
                AutoSize  = true,
            };
            this.lblSubtitulo = new System.Windows.Forms.Label {
                Text      = "Identifícate para continuar",
                Font      = new System.Drawing.Font("Segoe UI", 9f),
                ForeColor = clrSubtexto,
                Location  = new System.Drawing.Point(24, 50),
                AutoSize  = true,
            };

            // Usuario
            this.lblUserLabel = new System.Windows.Forms.Label {
                Text      = "USUARIO",
                Font      = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold),
                ForeColor = clrSubtexto,
                Location  = new System.Drawing.Point(24, 85),
                AutoSize  = true,
            };
            this.txtUsuario = new System.Windows.Forms.TextBox {
                Size        = new System.Drawing.Size(252, 25),
                Location    = new System.Drawing.Point(24, 105),
                BackColor   = System.Drawing.Color.FromArgb(32, 36, 50),
                ForeColor   = clrTexto,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Font        = new System.Drawing.Font("Segoe UI", 10f),
                TabIndex    = 0,
            };
            this.txtUsuario.KeyDown += this.txtUsuario_KeyDown;

            // Contraseña
            this.lblPasswordLabel = new System.Windows.Forms.Label {
                Text      = "CONTRASEÑA",
                Font      = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold),
                ForeColor = clrSubtexto,
                Location  = new System.Drawing.Point(24, 145),
                AutoSize  = true,
            };
            this.txtPassword = new System.Windows.Forms.TextBox {
                Size         = new System.Drawing.Size(252, 25),
                Location     = new System.Drawing.Point(24, 165),
                BackColor    = System.Drawing.Color.FromArgb(32, 36, 50),
                ForeColor    = clrTexto,
                BorderStyle  = System.Windows.Forms.BorderStyle.FixedSingle,
                PasswordChar = '●',
                Font         = new System.Drawing.Font("Segoe UI", 10f),
                TabIndex     = 1,
            };
            this.txtPassword.KeyDown += this.txtPassword_KeyDown;

            // Error
            this.lblError = new System.Windows.Forms.Label {
                ForeColor = clrRojo,
                Font      = new System.Drawing.Font("Segoe UI", 8f),
                Location  = new System.Drawing.Point(24, 200),
                Size      = new System.Drawing.Size(252, 20),
                Visible   = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
            };

            // Botón Entrar
            this.btnEntrar = new System.Windows.Forms.Button {
                Text      = "ENTRAR",
                Size      = new System.Drawing.Size(252, 40),
                Location  = new System.Drawing.Point(24, 228),
                BackColor = clrVerde,
                ForeColor = clrFondo,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font      = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold),
                Cursor    = System.Windows.Forms.Cursors.Hand,
                TabIndex  = 2,
            };
            this.btnEntrar.FlatAppearance.BorderSize = 0;
            this.btnEntrar.Click += this.btnEntrar_Click;

            // Botón Gestionar usuarios (discreto, debajo del login)
            this.btnGestionar = new System.Windows.Forms.Button {
                Text      = "⚙  Gestionar usuarios",
                Size      = new System.Drawing.Size(252, 30),
                Location  = new System.Drawing.Point(24, 282),
                BackColor = clrGris,
                ForeColor = clrSubtexto,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font      = new System.Drawing.Font("Segoe UI", 8.5f),
                Cursor    = System.Windows.Forms.Cursors.Hand,
                TabIndex  = 3,
            };
            this.btnGestionar.FlatAppearance.BorderSize = 0;
            this.btnGestionar.Click += this.btnGestionar_Click;

            this.pnlCentro.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblTitulo, this.lblSubtitulo,
                this.lblUserLabel, this.txtUsuario,
                this.lblPasswordLabel, this.txtPassword,
                this.lblError,
                this.btnEntrar,
                this.btnGestionar,
            });

            this.Controls.Add(this.pnlCentro);
            this.ResumeLayout(false);
        }
    }
}
