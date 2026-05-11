namespace BingoHistoricoEditor {
    partial class LoginForm {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlCentro;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblUserLabel;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblPasswordLabel;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnEntrar;
        private System.Windows.Forms.Label lblError;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            var clrFondo = System.Drawing.Color.FromArgb(18, 20, 28);
            var clrPanel = System.Drawing.Color.FromArgb(26, 29, 40);
            var clrTexto = System.Drawing.Color.FromArgb(220, 225, 240);
            var clrSubtexto = System.Drawing.Color.FromArgb(130, 140, 165);
            var clrVerde = System.Drawing.Color.FromArgb(72, 187, 120);
            var clrRojo = System.Drawing.Color.FromArgb(220, 80, 80);

            this.pnlCentro = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblUserLabel = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblPasswordLabel = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnEntrar = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // ── Formulario ────────────────────────────────────────────────────
            this.Text = "Acceso — Histórico Bingo";
            this.Size = new System.Drawing.Size(360, 360);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = clrFondo;

            // ── Panel Central ─────────────────────────────────────────────────
            this.pnlCentro.Size = new System.Drawing.Size(300, 300);
            this.pnlCentro.Location = new System.Drawing.Point(30, 30);
            this.pnlCentro.BackColor = clrPanel;

            // Títulos
            this.lblTitulo.Text = "BIENVENIDO";
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16f, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(24, 20);
            this.lblTitulo.AutoSize = true;

            this.lblSubtitulo.Text = "Identifícate para continuar";
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9f);
            this.lblSubtitulo.ForeColor = clrSubtexto;
            this.lblSubtitulo.Location = new System.Drawing.Point(24, 50);
            this.lblSubtitulo.AutoSize = true;

            // Campo Usuario
            this.lblUserLabel.Text = "USUARIO";
            this.lblUserLabel.Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold);
            this.lblUserLabel.ForeColor = clrSubtexto;
            this.lblUserLabel.Location = new System.Drawing.Point(24, 85);
            this.lblUserLabel.AutoSize = true;

            this.txtUsuario.Size = new System.Drawing.Size(252, 25);
            this.txtUsuario.Location = new System.Drawing.Point(24, 105);
            this.txtUsuario.BackColor = System.Drawing.Color.FromArgb(32, 36, 50);
            this.txtUsuario.ForeColor = clrTexto;
            this.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsuario.Font = new System.Drawing.Font("Segoe UI", 10f);
            this.txtUsuario.TabIndex = 0;
            this.txtUsuario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUsuario_KeyDown);

            // Campo Password
            this.lblPasswordLabel.Text = "CONTRASEÑA";
            this.lblPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold);
            this.lblPasswordLabel.ForeColor = clrSubtexto;
            this.lblPasswordLabel.Location = new System.Drawing.Point(24, 145);
            this.lblPasswordLabel.AutoSize = true;

            this.txtPassword.Size = new System.Drawing.Size(252, 25);
            this.txtPassword.Location = new System.Drawing.Point(24, 165);
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(32, 36, 50);
            this.txtPassword.ForeColor = clrTexto;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10f);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);

            // Mensaje de Error
            this.lblError.ForeColor = clrRojo;
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 8f);
            this.lblError.Location = new System.Drawing.Point(24, 200);
            this.lblError.Size = new System.Drawing.Size(252, 20);
            this.lblError.Visible = false;
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Botón Entrar
            this.btnEntrar.Text = "ENTRAR";
            this.btnEntrar.Size = new System.Drawing.Size(252, 40);
            this.btnEntrar.Location = new System.Drawing.Point(24, 230);
            this.btnEntrar.BackColor = clrVerde;
            this.btnEntrar.ForeColor = clrFondo;
            this.btnEntrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEntrar.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
            this.btnEntrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEntrar.TabIndex = 2;
            this.btnEntrar.FlatAppearance.BorderSize = 0;
            this.btnEntrar.Click += new System.EventHandler(this.btnEntrar_Click);

            this.pnlCentro.Controls.Add(this.lblTitulo);
            this.pnlCentro.Controls.Add(this.lblSubtitulo);
            this.pnlCentro.Controls.Add(this.lblUserLabel);
            this.pnlCentro.Controls.Add(this.txtUsuario);
            this.pnlCentro.Controls.Add(this.lblPasswordLabel);
            this.pnlCentro.Controls.Add(this.txtPassword);
            this.pnlCentro.Controls.Add(this.lblError);
            this.pnlCentro.Controls.Add(this.btnEntrar);

            this.Controls.Add(this.pnlCentro);
            this.ResumeLayout(false);
        }
    }
}