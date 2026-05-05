namespace BingoHistoricoEditor {
    partial class MainForm {
        private System.ComponentModel.IContainer components = null;

        // ── Controles ─────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSalaConc;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.GroupBox grpEdicion;
        private System.Windows.Forms.NumericUpDown nudPartida;
        private System.Windows.Forms.NumericUpDown nudPrecio;
        private System.Windows.Forms.NumericUpDown nudVendidos;
        private System.Windows.Forms.NumericUpDown nudLinea;
        private System.Windows.Forms.NumericUpDown nudBingo;
        private System.Windows.Forms.Label lblVentaPreview;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnNueva;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Label lblConc;
        private System.Windows.Forms.ComboBox cmbConc;
        private System.Windows.Forms.Button btnVerificarHuecos;
        private System.Windows.Forms.TableLayoutPanel tlpMainRight;
        private System.Windows.Forms.TableLayoutPanel tlpTop;
        private System.Windows.Forms.FlowLayoutPanel pnlBusqueda;
        private System.Windows.Forms.Panel pnlBotones;
        private System.Windows.Forms.TableLayoutPanel tlpEdicion;
        private System.Windows.Forms.Label lblPartida;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.Label lblVendidos;
        private System.Windows.Forms.Label lblLinea;
        private System.Windows.Forms.Label lblBingo;
        private Panel pnlResultados;
        private TextBox txtResultados;
        private Label lblTituloResultados;
        private System.Windows.Forms.Button btnCerrarResultados;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            pnlTop = new Panel();
            tlpTop = new TableLayoutPanel();
            lblTitulo = new Label();
            lblSalaConc = new Label();
            lblEstado = new Label();
            pnlLeft = new Panel();
            grid = new DataGridView();
            pnlResultados = new Panel();
            btnCerrarResultados = new Button();
            txtResultados = new TextBox();
            lblTituloResultados = new Label();
            lblTotal = new Label();
            pnlBusqueda = new FlowLayoutPanel();
            lblConc = new Label();
            cmbConc = new ComboBox();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            btnBuscar = new Button();
            pnlBottom = new Panel();
            tlpMainRight = new TableLayoutPanel();
            grpEdicion = new GroupBox();
            tlpEdicion = new TableLayoutPanel();
            lblPartida = new Label();
            nudPartida = new NumericUpDown();
            lblPrecio = new Label();
            nudPrecio = new NumericUpDown();
            lblVendidos = new Label();
            nudVendidos = new NumericUpDown();
            lblLinea = new Label();
            nudLinea = new NumericUpDown();
            lblBingo = new Label();
            nudBingo = new NumericUpDown();
            lblVentaPreview = new Label();
            pnlBotones = new Panel();
            btnEliminar = new Button();
            btnVerificarHuecos = new Button();
            btnGuardar = new Button();
            btnNueva = new Button();
            pnlTop.SuspendLayout();
            tlpTop.SuspendLayout();
            pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            pnlResultados.SuspendLayout();
            pnlBusqueda.SuspendLayout();
            pnlBottom.SuspendLayout();
            tlpMainRight.SuspendLayout();
            grpEdicion.SuspendLayout();
            tlpEdicion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPartida).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVendidos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLinea).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudBingo).BeginInit();
            pnlBotones.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(26, 29, 40);
            pnlTop.Controls.Add(tlpTop);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1024, 60);
            pnlTop.TabIndex = 2;
            // 
            // tlpTop
            // 
            tlpTop.ColumnCount = 3;
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpTop.Controls.Add(lblTitulo, 0, 0);
            tlpTop.Controls.Add(lblSalaConc, 1, 0);
            tlpTop.Controls.Add(lblEstado, 2, 0);
            tlpTop.Dock = DockStyle.Fill;
            tlpTop.Location = new Point(0, 0);
            tlpTop.Name = "tlpTop";
            tlpTop.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpTop.Size = new Size(1024, 60);
            tlpTop.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(3, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(403, 60);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "HISTÓRICO BINGO";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSalaConc
            // 
            lblSalaConc.Dock = DockStyle.Fill;
            lblSalaConc.ForeColor = Color.FromArgb(180, 180, 200);
            lblSalaConc.Location = new Point(412, 0);
            lblSalaConc.Name = "lblSalaConc";
            lblSalaConc.Size = new Size(301, 60);
            lblSalaConc.TabIndex = 1;
            lblSalaConc.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEstado
            // 
            lblEstado.Dock = DockStyle.Fill;
            lblEstado.Location = new Point(719, 0);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(302, 60);
            lblEstado.TabIndex = 2;
            lblEstado.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(grid);
            pnlLeft.Controls.Add(pnlResultados);
            pnlLeft.Controls.Add(lblTotal);
            pnlLeft.Controls.Add(pnlBusqueda);
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.Location = new Point(0, 60);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Padding = new Padding(12);
            pnlLeft.Size = new Size(744, 590);
            pnlLeft.TabIndex = 0;
            // 
            // grid
            // 
            grid.BackgroundColor = Color.FromArgb(30, 34, 46);
            grid.BorderStyle = BorderStyle.None;
            grid.Dock = DockStyle.Fill;
            grid.Location = new Point(12, 57);
            grid.Name = "grid";
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new Size(720, 376);
            grid.TabIndex = 0;
            grid.SelectionChanged += grid_SelectionChanged;
            // 
            // pnlResultados
            // 
            pnlResultados.BackColor = Color.FromArgb(30, 34, 46);
            pnlResultados.Controls.Add(btnCerrarResultados);
            pnlResultados.Controls.Add(txtResultados);
            pnlResultados.Controls.Add(lblTituloResultados);
            pnlResultados.Dock = DockStyle.Bottom;
            pnlResultados.Location = new Point(12, 433);
            pnlResultados.Name = "pnlResultados";
            pnlResultados.Padding = new Padding(5);
            pnlResultados.Size = new Size(720, 120);
            pnlResultados.TabIndex = 1;
            pnlResultados.Visible = false;
            // 
            // btnCerrarResultados
            // 
            btnCerrarResultados.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrarResultados.BackColor = Color.FromArgb(180, 50, 50);
            btnCerrarResultados.Cursor = Cursors.Hand;
            btnCerrarResultados.FlatAppearance.BorderSize = 0;
            btnCerrarResultados.FlatStyle = FlatStyle.Flat;
            btnCerrarResultados.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnCerrarResultados.ForeColor = Color.White;
            btnCerrarResultados.Location = new Point(695, 1);
            btnCerrarResultados.Name = "btnCerrarResultados";
            btnCerrarResultados.Size = new Size(25, 20);
            btnCerrarResultados.TabIndex = 2;
            btnCerrarResultados.Text = "X";
            btnCerrarResultados.UseVisualStyleBackColor = false;
            btnCerrarResultados.Click += btnCerrarResultados_Click;
            // 
            // txtResultados
            // 
            txtResultados.BackColor = Color.FromArgb(20, 22, 30);
            txtResultados.BorderStyle = BorderStyle.None;
            txtResultados.Dock = DockStyle.Fill;
            txtResultados.Font = new Font("Consolas", 9F);
            txtResultados.ForeColor = Color.White;
            txtResultados.Location = new Point(5, 25);
            txtResultados.Multiline = true;
            txtResultados.Name = "txtResultados";
            txtResultados.ReadOnly = true;
            txtResultados.ScrollBars = ScrollBars.Vertical;
            txtResultados.Size = new Size(710, 90);
            txtResultados.TabIndex = 0;
            // 
            // lblTituloResultados
            // 
            lblTituloResultados.Dock = DockStyle.Top;
            lblTituloResultados.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblTituloResultados.ForeColor = Color.FromArgb(255, 193, 7);
            lblTituloResultados.Location = new Point(5, 5);
            lblTituloResultados.Name = "lblTituloResultados";
            lblTituloResultados.Size = new Size(710, 20);
            lblTituloResultados.TabIndex = 1;
            lblTituloResultados.Text = "PARTIDAS FALTANTES:";
            // 
            // lblTotal
            // 
            lblTotal.Dock = DockStyle.Bottom;
            lblTotal.ForeColor = Color.FromArgb(150, 160, 180);
            lblTotal.Location = new Point(12, 553);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(720, 25);
            lblTotal.TabIndex = 2;
            // 
            // pnlBusqueda
            // 
            pnlBusqueda.Controls.Add(lblConc);
            pnlBusqueda.Controls.Add(cmbConc);
            pnlBusqueda.Controls.Add(lblFecha);
            pnlBusqueda.Controls.Add(dtpFecha);
            pnlBusqueda.Controls.Add(btnBuscar);
            pnlBusqueda.Dock = DockStyle.Top;
            pnlBusqueda.Location = new Point(12, 12);
            pnlBusqueda.Name = "pnlBusqueda";
            pnlBusqueda.Size = new Size(720, 45);
            pnlBusqueda.TabIndex = 3;
            // 
            // lblConc
            // 
            lblConc.AutoSize = true;
            lblConc.Location = new Point(3, 0);
            lblConc.Name = "lblConc";
            lblConc.Padding = new Padding(0, 7, 5, 0);
            lblConc.Size = new Size(80, 26);
            lblConc.TabIndex = 0;
            lblConc.Text = "Concesión:";
            // 
            // cmbConc
            // 
            cmbConc.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbConc.Location = new Point(89, 3);
            cmbConc.Name = "cmbConc";
            cmbConc.Size = new Size(80, 27);
            cmbConc.TabIndex = 1;
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(175, 0);
            lblFecha.Name = "lblFecha";
            lblFecha.Padding = new Padding(0, 7, 5, 0);
            lblFecha.Size = new Size(52, 26);
            lblFecha.TabIndex = 2;
            lblFecha.Text = "Fecha:";
            // 
            // dtpFecha
            // 
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(233, 3);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(110, 26);
            dtpFecha.TabIndex = 3;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.FromArgb(0, 120, 215);
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(349, 3);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(75, 23);
            btnBuscar.TabIndex = 4;
            btnBuscar.Text = "Cargar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = Color.FromArgb(22, 24, 32);
            pnlBottom.Controls.Add(tlpMainRight);
            pnlBottom.Dock = DockStyle.Right;
            pnlBottom.Location = new Point(744, 60);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(280, 590);
            pnlBottom.TabIndex = 1;
            // 
            // tlpMainRight
            // 
            tlpMainRight.ColumnCount = 1;
            tlpMainRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMainRight.Controls.Add(grpEdicion, 0, 0);
            tlpMainRight.Controls.Add(pnlBotones, 0, 1);
            tlpMainRight.Dock = DockStyle.Fill;
            tlpMainRight.Location = new Point(0, 0);
            tlpMainRight.Name = "tlpMainRight";
            tlpMainRight.Padding = new Padding(10);
            tlpMainRight.RowCount = 2;
            tlpMainRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpMainRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpMainRight.Size = new Size(280, 590);
            tlpMainRight.TabIndex = 0;
            // 
            // grpEdicion
            // 
            grpEdicion.Controls.Add(tlpEdicion);
            grpEdicion.Dock = DockStyle.Fill;
            grpEdicion.ForeColor = Color.FromArgb(99, 179, 237);
            grpEdicion.Location = new Point(13, 13);
            grpEdicion.Name = "grpEdicion";
            grpEdicion.Size = new Size(254, 279);
            grpEdicion.TabIndex = 0;
            grpEdicion.TabStop = false;
            grpEdicion.Text = " Edición de Partida ";
            // 
            // tlpEdicion
            // 
            tlpEdicion.ColumnCount = 2;
            tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpEdicion.Controls.Add(lblPartida, 0, 0);
            tlpEdicion.Controls.Add(nudPartida, 1, 0);
            tlpEdicion.Controls.Add(lblPrecio, 0, 1);
            tlpEdicion.Controls.Add(nudPrecio, 1, 1);
            tlpEdicion.Controls.Add(lblVendidos, 0, 2);
            tlpEdicion.Controls.Add(nudVendidos, 1, 2);
            tlpEdicion.Controls.Add(lblLinea, 0, 3);
            tlpEdicion.Controls.Add(nudLinea, 1, 3);
            tlpEdicion.Controls.Add(lblBingo, 0, 4);
            tlpEdicion.Controls.Add(nudBingo, 1, 4);
            tlpEdicion.Controls.Add(lblVentaPreview, 0, 5);
            tlpEdicion.Dock = DockStyle.Fill;
            tlpEdicion.Location = new Point(3, 22);
            tlpEdicion.Name = "tlpEdicion";
            tlpEdicion.RowCount = 6;
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpEdicion.Size = new Size(248, 254);
            tlpEdicion.TabIndex = 0;
            // 
            // nudPartida
            // 
            nudPartida.Dock = DockStyle.Top;
            nudPartida.Location = new Point(102, 3);
            nudPartida.Name = "nudPartida";
            nudPartida.Size = new Size(143, 26);
            nudPartida.TabIndex = 0;
            // 
            // nudPrecio
            // 
            nudPrecio.DecimalPlaces = 2;
            nudPrecio.Dock = DockStyle.Top;
            nudPrecio.Location = new Point(102, 23);
            nudPrecio.Name = "nudPrecio";
            nudPrecio.Size = new Size(143, 26);
            nudPrecio.TabIndex = 1;
            // 
            // nudVendidos
            // 
            nudVendidos.Dock = DockStyle.Top;
            nudVendidos.Location = new Point(102, 43);
            nudVendidos.Name = "nudVendidos";
            nudVendidos.Size = new Size(143, 26);
            nudVendidos.TabIndex = 2;
            // 
            // nudLinea
            // 
            nudLinea.DecimalPlaces = 2;
            nudLinea.Dock = DockStyle.Top;
            nudLinea.Location = new Point(102, 63);
            nudLinea.Name = "nudLinea";
            nudLinea.Size = new Size(143, 26);
            nudLinea.TabIndex = 3;
            // 
            // nudBingo
            // 
            nudBingo.DecimalPlaces = 2;
            nudBingo.Dock = DockStyle.Top;
            nudBingo.Location = new Point(102, 83);
            nudBingo.Name = "nudBingo";
            nudBingo.Size = new Size(143, 26);
            nudBingo.TabIndex = 4;
            // 
            // lblPartida
            // 
            lblPartida.AutoSize = false;
            lblPartida.Dock = DockStyle.Fill;
            lblPartida.ForeColor = Color.White;
            lblPartida.Name = "lblPartida";
            lblPartida.Text = "Partida:";
            lblPartida.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPrecio
            // 
            lblPrecio.AutoSize = false;
            lblPrecio.Dock = DockStyle.Fill;
            lblPrecio.ForeColor = Color.White;
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Text = "Precio:";
            lblPrecio.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVendidos
            // 
            lblVendidos.AutoSize = false;
            lblVendidos.Dock = DockStyle.Fill;
            lblVendidos.ForeColor = Color.White;
            lblVendidos.Name = "lblVendidos";
            lblVendidos.Text = "Vendidos:";
            lblVendidos.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblLinea
            // 
            lblLinea.AutoSize = false;
            lblLinea.Dock = DockStyle.Fill;
            lblLinea.ForeColor = Color.White;
            lblLinea.Name = "lblLinea";
            lblLinea.Text = "Línea:";
            lblLinea.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBingo
            // 
            lblBingo.AutoSize = false;
            lblBingo.Dock = DockStyle.Fill;
            lblBingo.ForeColor = Color.White;
            lblBingo.Name = "lblBingo";
            lblBingo.Text = "Bingo:";
            lblBingo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVentaPreview
            // 
            tlpEdicion.SetColumnSpan(lblVentaPreview, 2);
            lblVentaPreview.Dock = DockStyle.Fill;
            lblVentaPreview.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblVentaPreview.ForeColor = Color.FromArgb(255, 193, 7);
            lblVentaPreview.Location = new Point(3, 100);
            lblVentaPreview.Name = "lblVentaPreview";
            lblVentaPreview.Size = new Size(242, 154);
            lblVentaPreview.TabIndex = 5;
            lblVentaPreview.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlBotones
            // 
            pnlBotones.Controls.Add(btnEliminar);
            pnlBotones.Controls.Add(btnVerificarHuecos);
            pnlBotones.Controls.Add(btnGuardar);
            pnlBotones.Controls.Add(btnNueva);
            pnlBotones.Dock = DockStyle.Fill;
            pnlBotones.Location = new Point(13, 298);
            pnlBotones.Name = "pnlBotones";
            pnlBotones.Padding = new Padding(0, 10, 0, 0);
            pnlBotones.Size = new Size(254, 279);
            pnlBotones.TabIndex = 1;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(180, 50, 50);
            btnEliminar.Dock = DockStyle.Bottom;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(0, 234);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(254, 45);
            btnEliminar.TabIndex = 0;
            btnEliminar.Text = "ELIMINAR";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnVerificarHuecos
            // 
            btnVerificarHuecos.BackColor = Color.FromArgb(210, 120, 0);
            btnVerificarHuecos.Dock = DockStyle.Top;
            btnVerificarHuecos.FlatStyle = FlatStyle.Flat;
            btnVerificarHuecos.ForeColor = Color.White;
            btnVerificarHuecos.Location = new Point(0, 110);
            btnVerificarHuecos.Name = "btnVerificarHuecos";
            btnVerificarHuecos.Size = new Size(254, 45);
            btnVerificarHuecos.TabIndex = 1;
            btnVerificarHuecos.Text = "VERIFICAR HUECOS";
            btnVerificarHuecos.UseVisualStyleBackColor = false;
            btnVerificarHuecos.Click += btnVerificarHuecos_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(40, 167, 69);
            btnGuardar.Dock = DockStyle.Top;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(0, 55);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(254, 55);
            btnGuardar.TabIndex = 2;
            btnGuardar.Text = "GUARDAR";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnNueva
            // 
            btnNueva.BackColor = Color.FromArgb(70, 80, 100);
            btnNueva.Dock = DockStyle.Top;
            btnNueva.FlatStyle = FlatStyle.Flat;
            btnNueva.ForeColor = Color.White;
            btnNueva.Location = new Point(0, 10);
            btnNueva.Name = "btnNueva";
            btnNueva.Size = new Size(254, 45);
            btnNueva.TabIndex = 3;
            btnNueva.Text = "NUEVA PARTIDA";
            btnNueva.UseVisualStyleBackColor = false;
            btnNueva.Click += btnNueva_Click;
            // 
            // MainForm
            // 
            ClientSize = new Size(1024, 650);
            Controls.Add(pnlLeft);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bingo — Editor de Histórico";
            pnlTop.ResumeLayout(false);
            tlpTop.ResumeLayout(false);
            pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            pnlResultados.ResumeLayout(false);
            pnlResultados.PerformLayout();
            pnlBusqueda.ResumeLayout(false);
            pnlBusqueda.PerformLayout();
            pnlBottom.ResumeLayout(false);
            tlpMainRight.ResumeLayout(false);
            grpEdicion.ResumeLayout(false);
            tlpEdicion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudPartida).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVendidos).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLinea).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudBingo).EndInit();
            pnlBotones.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void HistoricoLabel(TableLayoutPanel parent, string texto, int col, int row) {
            var lbl = new System.Windows.Forms.Label {
                Text = texto,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F)
            };
            parent.Controls.Add(lbl, col, row);
        }
    }
}