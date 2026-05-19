namespace BingoHistoricoEditor {
    partial class MainForm {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TableLayoutPanel tlpHeader;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private System.Windows.Forms.TableLayoutPanel tlpIzquierda;
        private System.Windows.Forms.Panel pnlRight;

        private System.Windows.Forms.TabControl tabControlEdicion;
        private System.Windows.Forms.TabPage tabIndividual;
        private System.Windows.Forms.TabPage tabLote;

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

        // DECLARACIONES DE LOS NUEVOS CONTROLES DE LOTE
        private System.Windows.Forms.GroupBox grpLote;
        private System.Windows.Forms.TableLayoutPanel tlpLoteCampos;
        private System.Windows.Forms.FlowLayoutPanel flpBotonesLote;
        private System.Windows.Forms.NumericUpDown nudPartidaInicial;
        private System.Windows.Forms.NumericUpDown nudPartidaFinal;
        private System.Windows.Forms.Button btnBuscarFaltante;
        private System.Windows.Forms.Button btnGuardarLote;
        private System.Windows.Forms.Button btnEliminarLote;

        private System.Windows.Forms.NumericUpDown nudLoteVendidos;
        private System.Windows.Forms.NumericUpDown nudLotePrecio;
        private System.Windows.Forms.Label lblLoteVendidos;
        private System.Windows.Forms.Label lblLotePrecio;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

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
            tabControlEdicion = new TabControl();
            tabIndividual = new TabPage();
            grpEdicion = new GroupBox();
            flpBotones = new FlowLayoutPanel();
            btnNuevaPartida = new Button();
            btnGuardar = new Button();
            btnEliminar = new Button();
            tlpEdicion = new TableLayoutPanel();
            tabLote = new TabPage();
            grpLote = new GroupBox();
            flpBotonesLote = new FlowLayoutPanel();
            btnBuscarFaltante = new Button();
            btnGuardarLote = new Button();
            btnEliminarLote = new Button();
            tlpLoteCampos = new TableLayoutPanel();
            lblIni = new Label();
            nudPartidaInicial = new NumericUpDown();
            lblFin = new Label();
            nudPartidaFinal = new NumericUpDown();
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
            tabControlEdicion.SuspendLayout();
            tabIndividual.SuspendLayout();
            grpEdicion.SuspendLayout();
            flpBotones.SuspendLayout();
            tabLote.SuspendLayout();
            grpLote.SuspendLayout();
            flpBotonesLote.SuspendLayout();
            tlpLoteCampos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPartidaInicial).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPartidaFinal).BeginInit();
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
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpHeader.Controls.Add(pnlTextos, 0, 0);
            tlpHeader.Controls.Add(dtpFecha, 1, 0);
            tlpHeader.Controls.Add(btnBuscar, 2, 0);
            tlpHeader.Dock = DockStyle.Fill;
            tlpHeader.Location = new Point(0, 0);
            tlpHeader.Name = "tlpHeader";
            tlpHeader.Padding = new Padding(20, 0, 20, 0);
            tlpHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpHeader.Size = new Size(1244, 74);
            tlpHeader.TabIndex = 0;
            // 
            // pnlTextos
            // 
            pnlTextos.Controls.Add(lblTitulo);
            pnlTextos.Controls.Add(lblSalaConc);
            pnlTextos.Controls.Add(lblEstado);
            pnlTextos.Location = new Point(23, 3);
            pnlTextos.Name = "pnlTextos";
            pnlTextos.Size = new Size(200, 68);
            pnlTextos.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(237, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "HISTÓRICO DE PARTIDAS";
            // 
            // lblSalaConc
            // 
            lblSalaConc.AutoSize = true;
            lblSalaConc.ForeColor = Color.DarkGray;
            lblSalaConc.Location = new Point(2, 45);
            lblSalaConc.Name = "lblSalaConc";
            lblSalaConc.Size = new Size(192, 19);
            lblSalaConc.TabIndex = 1;
            lblSalaConc.Text = "SALA: 0 | CONCESIONARIO: 0";
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.ForeColor = Color.Orange;
            lblEstado.Location = new Point(220, 45);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(143, 19);
            lblEstado.TabIndex = 2;
            lblEstado.Text = "Estado: Desconectado";
            // 
            // dtpFecha
            // 
            dtpFecha.Anchor = AnchorStyles.Right;
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(961, 24);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(110, 26);
            dtpFecha.TabIndex = 1;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.None;
            btnBuscar.BackColor = Color.FromArgb(40, 167, 69);
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(1089, 19);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(120, 35);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "🔍 BUSCAR";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // tlpContent
            // 
            tlpContent.ColumnCount = 2;
            tlpContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpContent.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360F));
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
            tlpIzquierda.ColumnCount = 1;
            tlpIzquierda.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpIzquierda.Controls.Add(grid, 0, 0);
            tlpIzquierda.Controls.Add(pnlLog, 0, 1);
            tlpIzquierda.Dock = DockStyle.Fill;
            tlpIzquierda.Location = new Point(3, 3);
            tlpIzquierda.Name = "tlpIzquierda";
            tlpIzquierda.RowCount = 2;
            tlpIzquierda.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpIzquierda.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
            tlpIzquierda.Size = new Size(878, 658);
            tlpIzquierda.TabIndex = 0;
            // 
            // grid
            // 
            grid.BackgroundColor = Color.FromArgb(30, 30, 35);
            grid.BorderStyle = BorderStyle.None;
            grid.Dock = DockStyle.Fill;
            grid.Location = new Point(3, 3);
            grid.Name = "grid";
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new Size(872, 522);
            grid.TabIndex = 0;
            grid.SelectionChanged += grid_SelectionChanged;
            // 
            // pnlLog
            // 
            pnlLog.BackColor = Color.Black;
            pnlLog.Controls.Add(txtLog);
            pnlLog.Dock = DockStyle.Fill;
            pnlLog.Location = new Point(3, 531);
            pnlLog.Name = "pnlLog";
            pnlLog.Size = new Size(872, 124);
            pnlLog.TabIndex = 1;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.Black;
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 9F);
            txtLog.ForeColor = Color.LimeGreen;
            txtLog.Location = new Point(0, 0);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.Size = new Size(872, 124);
            txtLog.TabIndex = 0;
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(tabControlEdicion);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(887, 3);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(354, 658);
            pnlRight.TabIndex = 1;
            // 
            // tabControlEdicion
            // 
            tabControlEdicion.Controls.Add(tabIndividual);
            tabControlEdicion.Controls.Add(tabLote);
            tabControlEdicion.Dock = DockStyle.Fill;
            tabControlEdicion.Location = new Point(0, 0);
            tabControlEdicion.Name = "tabControlEdicion";
            tabControlEdicion.SelectedIndex = 0;
            tabControlEdicion.Size = new Size(354, 658);
            tabControlEdicion.TabIndex = 0;
            // 
            // tabIndividual
            // 
            tabIndividual.BackColor = Color.FromArgb(30, 30, 35);
            tabIndividual.Controls.Add(grpEdicion);
            tabIndividual.Location = new Point(4, 28);
            tabIndividual.Name = "tabIndividual";
            tabIndividual.Padding = new Padding(5);
            tabIndividual.Size = new Size(346, 626);
            tabIndividual.TabIndex = 0;
            tabIndividual.Text = "Edición Individual";
            // 
            // grpEdicion
            // 
            grpEdicion.Controls.Add(flpBotones);
            grpEdicion.Controls.Add(tlpEdicion);
            grpEdicion.Dock = DockStyle.Fill;
            grpEdicion.ForeColor = Color.Gold;
            grpEdicion.Location = new Point(5, 5);
            grpEdicion.Name = "grpEdicion";
            grpEdicion.Size = new Size(336, 616);
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
            flpBotones.Location = new Point(3, 453);
            flpBotones.Name = "flpBotones";
            flpBotones.Padding = new Padding(10, 5, 10, 5);
            flpBotones.Size = new Size(330, 160);
            flpBotones.TabIndex = 0;
            // 
            // btnNuevaPartida
            // 
            btnNuevaPartida.BackColor = Color.FromArgb(0, 122, 204);
            btnNuevaPartida.FlatStyle = FlatStyle.Flat;
            btnNuevaPartida.ForeColor = Color.White;
            btnNuevaPartida.Location = new Point(13, 8);
            btnNuevaPartida.Name = "btnNuevaPartida";
            btnNuevaPartida.Size = new Size(310, 35);
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
            btnGuardar.Location = new Point(13, 49);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(310, 35);
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
            btnEliminar.Location = new Point(13, 90);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(310, 35);
            btnEliminar.TabIndex = 2;
            btnEliminar.Text = "🗑 ELIMINAR";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // tlpEdicion
            // 
            tlpEdicion.ColumnCount = 2;
            tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpEdicion.Dock = DockStyle.Top;
            tlpEdicion.Location = new Point(3, 22);
            tlpEdicion.Name = "tlpEdicion";
            tlpEdicion.RowCount = 5;
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
            tlpEdicion.Size = new Size(330, 250);
            tlpEdicion.TabIndex = 1;
            // 
            // tabLote
            // 
            tabLote.BackColor = Color.FromArgb(30, 30, 35);
            tabLote.Controls.Add(grpLote);
            tabLote.Location = new Point(4, 28);
            tabLote.Name = "tabLote";
            tabLote.Padding = new Padding(5);
            tabLote.Size = new Size(346, 626);
            tabLote.TabIndex = 1;
            tabLote.Text = "Edición en Lote";
            // 
            // grpLote
            // 
            grpLote.Controls.Add(flpBotonesLote);
            grpLote.Controls.Add(tlpLoteCampos);
            grpLote.Dock = DockStyle.Fill;
            grpLote.ForeColor = Color.Gold;
            grpLote.Location = new Point(5, 5);
            grpLote.Name = "grpLote";
            grpLote.Size = new Size(336, 616);
            grpLote.TabIndex = 0;
            grpLote.TabStop = false;
            grpLote.Text = " ACCIONES MASIVAS ";
            // 
            // flpBotonesLote
            // 
            flpBotonesLote.Controls.Add(btnBuscarFaltante);
            flpBotonesLote.Controls.Add(btnGuardarLote);
            flpBotonesLote.Controls.Add(btnEliminarLote);
            flpBotonesLote.Dock = DockStyle.Bottom;
            flpBotonesLote.FlowDirection = FlowDirection.TopDown;
            flpBotonesLote.Location = new Point(3, 453);
            flpBotonesLote.Name = "flpBotonesLote";
            flpBotonesLote.Padding = new Padding(10, 5, 10, 5);
            flpBotonesLote.Size = new Size(330, 160);
            flpBotonesLote.TabIndex = 0;
            // 
            // btnBuscarFaltante
            // 
            btnBuscarFaltante.BackColor = Color.FromArgb(230, 126, 34);
            btnBuscarFaltante.FlatStyle = FlatStyle.Flat;
            btnBuscarFaltante.ForeColor = Color.White;
            btnBuscarFaltante.Location = new Point(13, 8);
            btnBuscarFaltante.Name = "btnBuscarFaltante";
            btnBuscarFaltante.Size = new Size(310, 35);
            btnBuscarFaltante.TabIndex = 0;
            btnBuscarFaltante.Text = "🔍 BUSCAR PRIMER FALTANTE";
            btnBuscarFaltante.UseVisualStyleBackColor = false;
            btnBuscarFaltante.Click += btnBuscarFaltante_Click;
            // 
            // btnGuardarLote
            // 
            btnGuardarLote.BackColor = Color.FromArgb(40, 167, 69);
            btnGuardarLote.FlatStyle = FlatStyle.Flat;
            btnGuardarLote.ForeColor = Color.White;
            btnGuardarLote.Location = new Point(13, 49);
            btnGuardarLote.Name = "btnGuardarLote";
            btnGuardarLote.Size = new Size(310, 35);
            btnGuardarLote.TabIndex = 1;
            btnGuardarLote.Text = "💾 GUARDAR LOTE";
            btnGuardarLote.UseVisualStyleBackColor = false;
            btnGuardarLote.Click += btnGuardarLote_Click;
            // 
            // btnEliminarLote
            // 
            btnEliminarLote.BackColor = Color.FromArgb(220, 53, 69);
            btnEliminarLote.FlatStyle = FlatStyle.Flat;
            btnEliminarLote.ForeColor = Color.White;
            btnEliminarLote.Location = new Point(13, 90);
            btnEliminarLote.Name = "btnEliminarLote";
            btnEliminarLote.Size = new Size(310, 35);
            btnEliminarLote.TabIndex = 2;
            btnEliminarLote.Text = "🗑 ELIMINAR LOTE";
            btnEliminarLote.UseVisualStyleBackColor = false;
            btnEliminarLote.Click += btnEliminarLote_Click;
            // 
            // tlpLoteCampos
            // 
            tlpLoteCampos.ColumnCount = 2;
            tlpLoteCampos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpLoteCampos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpLoteCampos.Controls.Add(lblIni, 0, 0);
            tlpLoteCampos.Controls.Add(nudPartidaInicial, 1, 0);
            tlpLoteCampos.Controls.Add(lblFin, 0, 1);
            tlpLoteCampos.Controls.Add(nudPartidaFinal, 1, 1);
            tlpLoteCampos.Dock = DockStyle.Top;
            tlpLoteCampos.Location = new Point(3, 22);
            tlpLoteCampos.Name = "tlpLoteCampos";
            tlpLoteCampos.RowCount = 2;
            tlpLoteCampos.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpLoteCampos.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpLoteCampos.Size = new Size(330, 90);
            tlpLoteCampos.TabIndex = 1;
            // 
            // lblIni
            // 
            lblIni.ForeColor = Color.White;
            lblIni.Location = new Point(3, 0);
            lblIni.Name = "lblIni";
            lblIni.Size = new Size(100, 23);
            lblIni.TabIndex = 0;
            lblIni.Text = "Partida Inicial";
            // 
            // nudPartidaInicial
            // 
            nudPartidaInicial.Dock = DockStyle.Fill;
            nudPartidaInicial.Location = new Point(135, 3);
            nudPartidaInicial.Name = "nudPartidaInicial";
            nudPartidaInicial.Size = new Size(192, 26);
            nudPartidaInicial.TabIndex = 1;
            nudPartidaInicial.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblFin
            // 
            lblFin.ForeColor = Color.White;
            lblFin.Location = new Point(3, 45);
            lblFin.Name = "lblFin";
            lblFin.Size = new Size(100, 23);
            lblFin.TabIndex = 2;
            lblFin.Text = "Partida Final";
            // 
            // nudPartidaFinal
            // 
            nudPartidaFinal.Dock = DockStyle.Fill;
            nudPartidaFinal.Location = new Point(135, 48);
            nudPartidaFinal.Name = "nudPartidaFinal";
            nudPartidaFinal.Size = new Size(192, 26);
            nudPartidaFinal.TabIndex = 3;
            nudPartidaFinal.Value = new decimal(new int[] { 1, 0, 0, 0 });
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
            tabControlEdicion.ResumeLayout(false);
            tabIndividual.ResumeLayout(false);
            grpEdicion.ResumeLayout(false);
            flpBotones.ResumeLayout(false);
            tabLote.ResumeLayout(false);
            grpLote.ResumeLayout(false);
            flpBotonesLote.ResumeLayout(false);
            tlpLoteCampos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudPartidaInicial).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPartidaFinal).EndInit();
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
            ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEdicion.Controls.Add(lbl, 0, row);
            this.tlpEdicion.Controls.Add(ctrl, 1, row);
        }

        #endregion

        private Panel pnlTextos;
        private Label lblIni;
        private Label lblFin;
    }
}