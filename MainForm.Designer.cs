namespace BingoHistoricoEditor {
    partial class MainForm {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TableLayoutPanel tlpHeader;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private System.Windows.Forms.TableLayoutPanel tlpIzquierda;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.GroupBox grpEdicion;
        private System.Windows.Forms.TableLayoutPanel tlpEdicion;
        private System.Windows.Forms.FlowLayoutPanel flpBotones;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSalaConc;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.NumericUpDown nudPartida;
        private System.Windows.Forms.NumericUpDown nudPrecio;
        private System.Windows.Forms.NumericUpDown nudVendidos;
        private System.Windows.Forms.NumericUpDown nudLinea;
        private System.Windows.Forms.NumericUpDown nudBingo;
        private System.Windows.Forms.Button btnNuevaPartida;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEliminar;

        private void InitializeComponent() {
            tlpMain = new TableLayoutPanel();
            pnlTop = new Panel();
            tlpHeader = new TableLayoutPanel();
            pnlTextos = new Panel();
            lblTitulo = new Label();
            lblSalaConc = new Label();
            lblEstado = new Label();
            dtpFecha = new DateTimePicker();
            btnBuscar = new Button();
            tlpContent = new TableLayoutPanel();
            tlpIzquierda = new TableLayoutPanel();
            grid = new DataGridView();
            pnlLog = new Panel();
            txtLog = new TextBox();
            pnlRight = new Panel();
            grpEdicion = new GroupBox();
            flpBotones = new FlowLayoutPanel();
            btnNuevaPartida = new Button();
            btnGuardar = new Button();
            btnEliminar = new Button();
            tlpEdicion = new TableLayoutPanel();
            nudPartida = new NumericUpDown();
            nudPrecio = new NumericUpDown();
            nudVendidos = new NumericUpDown();
            nudLinea = new NumericUpDown();
            nudBingo = new NumericUpDown();
            tlpMain.SuspendLayout();
            pnlTop.SuspendLayout();
            tlpHeader.SuspendLayout();
            pnlTextos.SuspendLayout();
            tlpContent.SuspendLayout();
            tlpIzquierda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            pnlLog.SuspendLayout();
            pnlRight.SuspendLayout();
            grpEdicion.SuspendLayout();
            flpBotones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPartida).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVendidos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLinea).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudBingo).BeginInit();
            SuspendLayout();
            // 
            // tlpMain
            // 
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpMain.Controls.Add(pnlTop, 0, 0);
            tlpMain.Controls.Add(tlpContent, 0, 1);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(0, 0);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 2;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMain.Size = new Size(1250, 750);
            tlpMain.TabIndex = 0;
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(45, 45, 50);
            pnlTop.Controls.Add(tlpHeader);
            pnlTop.Dock = DockStyle.Fill;
            pnlTop.Location = new Point(3, 3);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1244, 74);
            pnlTop.TabIndex = 0;
            // 
            // tlpHeader
            // 
            tlpHeader.ColumnCount = 3;
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpHeader.Controls.Add(pnlTextos, 0, 0);
            tlpHeader.Controls.Add(dtpFecha, 1, 0);
            tlpHeader.Controls.Add(btnBuscar, 2, 0);
            tlpHeader.Dock = DockStyle.Fill;
            tlpHeader.Location = new Point(0, 0);
            tlpHeader.Name = "tlpHeader";
            tlpHeader.Padding = new Padding(20, 10, 20, 10);
            tlpHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpHeader.Size = new Size(1244, 74);
            tlpHeader.TabIndex = 0;
            // 
            // pnlTextos
            // 
            pnlTextos.Controls.Add(lblTitulo);
            pnlTextos.Controls.Add(lblSalaConc);
            pnlTextos.Controls.Add(lblEstado);
            pnlTextos.Location = new Point(23, 13);
            pnlTextos.Name = "pnlTextos";
            pnlTextos.Size = new Size(200, 48);
            pnlTextos.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 5);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(237, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "HISTÓRICO DE PARTIDAS";
            // 
            // lblSalaConc
            // 
            lblSalaConc.AutoSize = true;
            lblSalaConc.ForeColor = Color.DarkGray;
            lblSalaConc.Location = new Point(2, 35);
            lblSalaConc.Name = "lblSalaConc";
            lblSalaConc.Size = new Size(192, 19);
            lblSalaConc.TabIndex = 1;
            lblSalaConc.Text = "SALA: 0 | CONCESIONARIO: 0";
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.ForeColor = Color.Orange;
            lblEstado.Location = new Point(220, 35);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(143, 19);
            lblEstado.TabIndex = 2;
            lblEstado.Text = "Estado: Desconectado";
            // 
            // dtpFecha
            // 
            dtpFecha.Anchor = AnchorStyles.Right;
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(951, 24);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(120, 26);
            dtpFecha.TabIndex = 1;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.FromArgb(40, 167, 69);
            btnBuscar.Dock = DockStyle.Fill;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(1079, 20);
            btnBuscar.Margin = new Padding(5, 10, 5, 10);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(140, 34);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "🔍 BUSCAR";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // tlpContent
            // 
            tlpContent.ColumnCount = 2;
            tlpContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpContent.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 350F));
            tlpContent.Controls.Add(tlpIzquierda, 0, 0);
            tlpContent.Controls.Add(pnlRight, 1, 0);
            tlpContent.Dock = DockStyle.Fill;
            tlpContent.Location = new Point(3, 83);
            tlpContent.Name = "tlpContent";
            tlpContent.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpContent.Size = new Size(1244, 664);
            tlpContent.TabIndex = 1;
            // 
            // tlpIzquierda
            // 
            tlpIzquierda.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpIzquierda.Controls.Add(grid, 0, 0);
            tlpIzquierda.Controls.Add(pnlLog, 0, 1);
            tlpIzquierda.Dock = DockStyle.Fill;
            tlpIzquierda.Location = new Point(3, 3);
            tlpIzquierda.Name = "tlpIzquierda";
            tlpIzquierda.RowCount = 2;
            tlpIzquierda.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpIzquierda.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
            tlpIzquierda.Size = new Size(888, 658);
            tlpIzquierda.TabIndex = 0;
            // 
            // grid
            // 
            grid.BackgroundColor = Color.FromArgb(30, 30, 35);
            grid.BorderStyle = BorderStyle.None;
            grid.Dock = DockStyle.Fill;
            grid.Location = new Point(3, 3);
            grid.Name = "grid";
            grid.Size = new Size(882, 522);
            grid.TabIndex = 0;
            // 
            // pnlLog
            // 
            pnlLog.Controls.Add(txtLog);
            pnlLog.Dock = DockStyle.Fill;
            pnlLog.Location = new Point(3, 531);
            pnlLog.Name = "pnlLog";
            pnlLog.Size = new Size(882, 124);
            pnlLog.TabIndex = 1;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.Black;
            txtLog.Dock = DockStyle.Fill;
            txtLog.ForeColor = Color.LimeGreen;
            txtLog.Location = new Point(0, 0);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.Size = new Size(882, 124);
            txtLog.TabIndex = 0;
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(grpEdicion);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(897, 3);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(344, 658);
            pnlRight.TabIndex = 1;
            // 
            // grpEdicion
            // 
            grpEdicion.Controls.Add(flpBotones);
            grpEdicion.Controls.Add(tlpEdicion);
            grpEdicion.Dock = DockStyle.Fill;
            grpEdicion.ForeColor = Color.Gold;
            grpEdicion.Location = new Point(0, 0);
            grpEdicion.Name = "grpEdicion";
            grpEdicion.Size = new Size(344, 658);
            grpEdicion.TabIndex = 0;
            grpEdicion.TabStop = false;
            grpEdicion.Text = " DATOS PARTIDA ";
            // 
            // flpBotones
            // 
            flpBotones.Controls.Add(btnNuevaPartida);
            flpBotones.Controls.Add(btnGuardar);
            flpBotones.Controls.Add(btnEliminar);
            flpBotones.Dock = DockStyle.Bottom;
            flpBotones.FlowDirection = FlowDirection.TopDown;
            flpBotones.Location = new Point(3, 495);
            flpBotones.Name = "flpBotones";
            flpBotones.Padding = new Padding(15);
            flpBotones.Size = new Size(338, 160);
            flpBotones.TabIndex = 0;
            // 
            // btnNuevaPartida
            // 
            btnNuevaPartida.BackColor = Color.FromArgb(0, 122, 204);
            btnNuevaPartida.FlatStyle = FlatStyle.Flat;
            btnNuevaPartida.ForeColor = Color.White;
            btnNuevaPartida.Location = new Point(18, 18);
            btnNuevaPartida.Name = "btnNuevaPartida";
            btnNuevaPartida.Size = new Size(300, 35);
            btnNuevaPartida.TabIndex = 0;
            btnNuevaPartida.Text = "✨ NUEVA PARTIDA";
            btnNuevaPartida.UseVisualStyleBackColor = false;
            btnNuevaPartida.Click += btnNuevaPartida_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(40, 167, 69);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(18, 59);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(300, 35);
            btnGuardar.TabIndex = 1;
            btnGuardar.Text = "💾 GUARDAR";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(220, 53, 69);
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(18, 100);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(300, 35);
            btnEliminar.TabIndex = 2;
            btnEliminar.Text = "🗑 ELIMINAR";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // tlpEdicion
            // 
            tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpEdicion.Dock = DockStyle.Top;
            tlpEdicion.Location = new Point(3, 22);
            tlpEdicion.Name = "tlpEdicion";
            tlpEdicion.RowCount = 5;
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpEdicion.Size = new Size(338, 250);
            tlpEdicion.TabIndex = 1;
            // 
            // nudPartida
            // 
            nudPartida.Location = new Point(0, 0);
            nudPartida.Name = "nudPartida";
            nudPartida.Size = new Size(120, 26);
            nudPartida.TabIndex = 0;
            // 
            // nudPrecio
            // 
            nudPrecio.Location = new Point(0, 0);
            nudPrecio.Name = "nudPrecio";
            nudPrecio.Size = new Size(120, 26);
            nudPrecio.TabIndex = 0;
            // 
            // nudVendidos
            // 
            nudVendidos.Location = new Point(0, 0);
            nudVendidos.Name = "nudVendidos";
            nudVendidos.Size = new Size(120, 26);
            nudVendidos.TabIndex = 0;
            // 
            // nudLinea
            // 
            nudLinea.Location = new Point(0, 0);
            nudLinea.Name = "nudLinea";
            nudLinea.Size = new Size(120, 26);
            nudLinea.TabIndex = 0;
            // 
            // nudBingo
            // 
            nudBingo.Location = new Point(0, 0);
            nudBingo.Name = "nudBingo";
            nudBingo.Size = new Size(120, 26);
            nudBingo.TabIndex = 0;
            // 
            // MainForm
            // 
            BackColor = Color.FromArgb(30, 30, 35);
            ClientSize = new Size(1250, 750);
            Controls.Add(tlpMain);
            Name = "MainForm";
            Text = "Bingo Histórico - Editor";
            tlpMain.ResumeLayout(false);
            pnlTop.ResumeLayout(false);
            tlpHeader.ResumeLayout(false);
            pnlTextos.ResumeLayout(false);
            pnlTextos.PerformLayout();
            tlpContent.ResumeLayout(false);
            tlpIzquierda.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            pnlLog.ResumeLayout(false);
            pnlLog.PerformLayout();
            pnlRight.ResumeLayout(false);
            grpEdicion.ResumeLayout(false);
            flpBotones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudPartida).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVendidos).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLinea).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudBingo).EndInit();
            ResumeLayout(false);
        }

        private void AgregarControlEdicion(string texto, System.Windows.Forms.Control ctrl, int row) {
            var lbl = new System.Windows.Forms.Label {
                Text = texto,
                ForeColor = System.Drawing.Color.LightGray,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Dock = System.Windows.Forms.DockStyle.Fill
            };
            this.tlpEdicion.Controls.Add(lbl, 0, row);
            this.tlpEdicion.Controls.Add(ctrl, 1, row);
        }
        #endregion

        private Panel pnlTextos;
    }
}