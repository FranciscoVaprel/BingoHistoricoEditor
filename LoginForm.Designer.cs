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
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label  lblError;
        private System.Windows.Forms.LinkLabel lnkRestablecer;

        protected override void Dispose(bool disposing) {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.SuspendLayout();

            // ── Formulario ────────────────────────────────────────────────────
            this.Text            = "Acceso — Histórico Bingo";
            this.ClientSize      = new System.Drawing.Size(360, 474);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor       = System.Drawing.SystemColors.Control;

            // ── Panel central ─────────────────────────────────────────────────
            this.pnlCentro = new System.Windows.Forms.Panel {
                Size        = new System.Drawing.Size(300, 414),
                Location    = new System.Drawing.Point(30, 30),
                BackColor   = System.Drawing.SystemColors.Control,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
            };

            this.lblTitulo = new System.Windows.Forms.Label {
                Text      = "BIENVENIDO",
                Font      = new System.Drawing.Font("Segoe UI", 16f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.SystemColors.ControlText,
                Location  = new System.Drawing.Point(24, 20),
                AutoSize  = true,
            };
            this.lblSubtitulo = new System.Windows.Forms.Label {
                Text      = "Identifícate para continuar",
                Font      = new System.Drawing.Font("Segoe UI", 9f),
                ForeColor = System.Drawing.SystemColors.GrayText,
                Location  = new System.Drawing.Point(24, 50),
                AutoSize  = true,
            };

            // Usuario
            this.lblUserLabel = new System.Windows.Forms.Label {
                Text      = "USUARIO",
                Font      = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.SystemColors.GrayText,
                Location  = new System.Drawing.Point(24, 85),
                AutoSize  = true,
            };
            this.txtUsuario = new System.Windows.Forms.TextBox {
                Size        = new System.Drawing.Size(252, 25),
                Location    = new System.Drawing.Point(24, 105),
                BackColor   = System.Drawing.SystemColors.Window,
                ForeColor   = System.Drawing.SystemColors.WindowText,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Font        = new System.Drawing.Font("Segoe UI", 10f),
                TabIndex    = 0,
            };
            this.txtUsuario.KeyDown += this.txtUsuario_KeyDown;

            // Contraseña
            this.lblPasswordLabel = new System.Windows.Forms.Label {
                Text      = "CONTRASEÑA",
                Font      = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.SystemColors.GrayText,
                Location  = new System.Drawing.Point(24, 145),
                AutoSize  = true,
            };
            this.txtPassword = new System.Windows.Forms.TextBox {
                Size         = new System.Drawing.Size(252, 25),
                Location     = new System.Drawing.Point(24, 165),
                BackColor    = System.Drawing.SystemColors.Window,
                ForeColor    = System.Drawing.SystemColors.WindowText,
                BorderStyle  = System.Windows.Forms.BorderStyle.FixedSingle,
                PasswordChar = '●',
                Font         = new System.Drawing.Font("Segoe UI", 10f),
                TabIndex     = 1,
            };
            this.txtPassword.KeyDown += this.txtPassword_KeyDown;

            // Error
            this.lblError = new System.Windows.Forms.Label {
                ForeColor = System.Drawing.Color.Red,
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
                ForeColor = System.Drawing.SystemColors.ControlText,
                FlatStyle = System.Windows.Forms.FlatStyle.Standard,
                Font      = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold),
                Cursor    = System.Windows.Forms.Cursors.Hand,
                TabIndex  = 2,
                UseVisualStyleBackColor = true,
            };
            this.btnEntrar.Click += this.btnEntrar_Click;

            // Botón Gestionar usuarios (discreto, debajo del login)
            this.btnGestionar = new System.Windows.Forms.Button {
                Text      = "⚙  Gestionar usuarios",
                Size      = new System.Drawing.Size(252, 30),
                Location  = new System.Drawing.Point(24, 282),
                ForeColor = System.Drawing.SystemColors.GrayText,
                FlatStyle = System.Windows.Forms.FlatStyle.Standard,
                Font      = new System.Drawing.Font("Segoe UI", 8.5f),
                Cursor    = System.Windows.Forms.Cursors.Hand,
                TabIndex  = 3,
                UseVisualStyleBackColor = true,
            };
            this.btnGestionar.Click += this.btnGestionar_Click;

            // Botón Cerrar
            this.btnCerrar = new System.Windows.Forms.Button {
                Text      = "✕  Cerrar",
                Size      = new System.Drawing.Size(252, 30),
                Location  = new System.Drawing.Point(24, 326),
                ForeColor = System.Drawing.SystemColors.ControlText,
                FlatStyle = System.Windows.Forms.FlatStyle.Standard,
                Font      = new System.Drawing.Font("Segoe UI", 8.5f),
                Cursor    = System.Windows.Forms.Cursors.Hand,
                TabIndex  = 4,
                UseVisualStyleBackColor = true,
            };
            this.btnCerrar.Click += this.btnCerrar_Click;

            // Enlace discreto de recuperación de acceso (NO es una puerta trasera:
            // requiere acceso físico al PC y confirmación explícita; borra todos
            // los usuarios y fuerza crear un admin nuevo en el próximo arranque).
            this.lnkRestablecer = new System.Windows.Forms.LinkLabel {
                Text      = "¿Olvidaste el acceso? Restablecer administrador",
                Font      = new System.Drawing.Font("Segoe UI", 8f),
                LinkColor = System.Drawing.SystemColors.GrayText,
                ActiveLinkColor = System.Drawing.SystemColors.HotTrack,
                LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline,
                AutoSize  = true,
                Location  = new System.Drawing.Point(24, 366),
                TabIndex  = 5,
            };
            this.lnkRestablecer.LinkClicked += this.lnkRestablecer_LinkClicked;

            this.pnlCentro.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblTitulo, this.lblSubtitulo,
                this.lblUserLabel, this.txtUsuario,
                this.lblPasswordLabel, this.txtPassword,
                this.lblError,
                this.btnEntrar,
                this.btnGestionar,
                this.btnCerrar,
                this.lnkRestablecer,
            });

            this.Controls.Add(this.pnlCentro);
            this.ResumeLayout(false);
        }
    }
}
