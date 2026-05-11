namespace BingoHistoricoEditor {
    partial class ConfirmDialog {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Button btnSi;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlButtons;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            pnlHeader = new System.Windows.Forms.Panel();
            lblTitulo = new System.Windows.Forms.Label();
            lblMensaje = new System.Windows.Forms.Label();
            pnlButtons = new System.Windows.Forms.Panel();
            btnSi = new System.Windows.Forms.Button();
            btnNo = new System.Windows.Forms.Button();
            pnlHeader.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();

            // pnlHeader
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(26, 29, 40);
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new System.Windows.Forms.Padding(15, 15, 15, 10);
            pnlHeader.Size = new System.Drawing.Size(400, 60);
            pnlHeader.TabIndex = 0;

            // lblTitulo
            lblTitulo.AutoSize = false;
            lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Location = new System.Drawing.Point(15, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(370, 35);
            lblTitulo.TabIndex = 0;
            lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // lblMensaje
            lblMensaje.BackColor = System.Drawing.Color.FromArgb(30, 34, 46);
            lblMensaje.Dock = System.Windows.Forms.DockStyle.Fill;
            lblMensaje.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblMensaje.ForeColor = System.Drawing.Color.FromArgb(200, 200, 200);
            lblMensaje.Location = new System.Drawing.Point(0, 60);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Padding = new System.Windows.Forms.Padding(15);
            lblMensaje.Size = new System.Drawing.Size(400, 90);
            lblMensaje.TabIndex = 1;
            lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // pnlButtons
            pnlButtons.BackColor = System.Drawing.Color.FromArgb(22, 24, 32);
            pnlButtons.Controls.Add(btnNo);
            pnlButtons.Controls.Add(btnSi);
            pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlButtons.Location = new System.Drawing.Point(0, 150);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new System.Windows.Forms.Padding(10);
            pnlButtons.Size = new System.Drawing.Size(400, 60);
            pnlButtons.TabIndex = 2;

            // btnSi
            btnSi.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            btnSi.Cursor = System.Windows.Forms.Cursors.Hand;
            btnSi.FlatAppearance.BorderSize = 0;
            btnSi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnSi.ForeColor = System.Drawing.Color.White;
            btnSi.Location = new System.Drawing.Point(210, 10);
            btnSi.Name = "btnSi";
            btnSi.Size = new System.Drawing.Size(85, 40);
            btnSi.TabIndex = 0;
            btnSi.Text = "SÍ";
            btnSi.UseVisualStyleBackColor = false;
            btnSi.Click += new System.EventHandler(btnSi_Click);

            // btnNo
            btnNo.BackColor = System.Drawing.Color.FromArgb(180, 50, 50);
            btnNo.Cursor = System.Windows.Forms.Cursors.Hand;
            btnNo.FlatAppearance.BorderSize = 0;
            btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnNo.ForeColor = System.Drawing.Color.White;
            btnNo.Location = new System.Drawing.Point(105, 10);
            btnNo.Name = "btnNo";
            btnNo.Size = new System.Drawing.Size(85, 40);
            btnNo.TabIndex = 1;
            btnNo.Text = "NO";
            btnNo.UseVisualStyleBackColor = false;
            btnNo.Click += new System.EventHandler(btnNo_Click);

            // ConfirmDialog
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 34, 46);
            ClientSize = new System.Drawing.Size(400, 210);
            Controls.Add(lblMensaje);
            Controls.Add(pnlButtons);
            Controls.Add(pnlHeader);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConfirmDialog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Confirmar";
            TopMost = false;
            pnlHeader.ResumeLayout(false);
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
