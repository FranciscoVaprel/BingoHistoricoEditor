namespace BingoHistoricoEditor {
    partial class MainForm {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlTextos;
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
        private System.Windows.Forms.Label lblPartida;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.Label lblVendidos;
        private System.Windows.Forms.Label lblLinea;
        private System.Windows.Forms.Label lblBingo;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnImportarLog;
        private System.Windows.Forms.Button btnAuditoria;
        private System.Windows.Forms.Button btnConfiguracion;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.NumericUpDown nudPartida;
        private System.Windows.Forms.NumericUpDown nudPrecio;
        private System.Windows.Forms.NumericUpDown nudVendidos;
        private System.Windows.Forms.NumericUpDown nudLinea;
        private System.Windows.Forms.NumericUpDown nudBingo;
        private System.Windows.Forms.Button btnNuevaPartida;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEditarFicha;
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
        private System.Windows.Forms.Label lblIni;
        private System.Windows.Forms.Label lblFin;

        // ToolTip para todos los controles
        private System.Windows.Forms.ToolTip toolTip;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            tlpMain = new TableLayoutPanel();
            pnlTop = new Panel();
            tlpHeader = new TableLayoutPanel();
            pnlTextos = new Panel();
            lblTitulo = new Label();
            lblSalaConc = new Label();
            lblEstado = new Label();
            dtpFecha = new DateTimePicker();
            btnBuscar = new Button();
            btnImportarLog = new Button();
            btnAuditoria = new Button();
            btnConfiguracion = new Button();
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
            btnEditarFicha = new Button();
            btnEliminar = new Button();
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
            lblLoteVendidos = new Label();
            nudLoteVendidos = new NumericUpDown();
            lblLotePrecio = new Label();
            nudLotePrecio = new NumericUpDown();
            toolTip = new ToolTip(components);
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
            tlpEdicion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPartida).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVendidos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLinea).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudBingo).BeginInit();
            tabLote.SuspendLayout();
            grpLote.SuspendLayout();
            flpBotonesLote.SuspendLayout();
            tlpLoteCampos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPartidaInicial).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPartidaFinal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLoteVendidos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLotePrecio).BeginInit();
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
            pnlTop.BackColor = SystemColors.ControlLight;
            pnlTop.Controls.Add(tlpHeader);
            pnlTop.Dock = DockStyle.Fill;
            pnlTop.Location = new Point(3, 3);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1244, 74);
            pnlTop.TabIndex = 0;
            //
            // tlpHeader
            //
            tlpHeader.ColumnCount = 6;
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpHeader.Controls.Add(pnlTextos, 0, 0);
            tlpHeader.Controls.Add(dtpFecha, 1, 0);
            tlpHeader.Controls.Add(btnBuscar, 2, 0);
            tlpHeader.Controls.Add(btnImportarLog, 3, 0);
            tlpHeader.Controls.Add(btnAuditoria, 4, 0);
            tlpHeader.Controls.Add(btnConfiguracion, 5, 0);
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
            pnlTextos.Size = new Size(539, 68);
            pnlTextos.TabIndex = 0;
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = SystemColors.ControlText;
            lblTitulo.Location = new Point(0, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(237, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "HISTÓRICO DE PARTIDAS";
            //
            // lblSalaConc
            //
            lblSalaConc.AutoSize = true;
            lblSalaConc.ForeColor = SystemColors.GrayText;
            lblSalaConc.Location = new Point(2, 45);
            lblSalaConc.Name = "lblSalaConc";
            lblSalaConc.Size = new Size(192, 19);
            lblSalaConc.TabIndex = 1;
            lblSalaConc.Text = "SALA: 0 | CONCESIONARIO: 0";
            //
            // lblEstado
            //
            lblEstado.AutoSize = true;
            lblEstado.ForeColor = Color.DarkOrange;
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
            dtpFecha.Location = new Point(811, 24);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(110, 26);
            dtpFecha.TabIndex = 1;
            toolTip.SetToolTip(dtpFecha, "Selecciona la fecha para filtrar partidas.");
            //
            // btnBuscar
            //
            btnBuscar.Anchor = AnchorStyles.None;
            btnBuscar.FlatStyle = FlatStyle.Standard;
            btnBuscar.ForeColor = SystemColors.ControlText;
            btnBuscar.Location = new Point(939, 19);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(120, 35);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "🔍 BUSCAR";
            toolTip.SetToolTip(btnBuscar, "Buscar partidas por la fecha seleccionada.");
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            //
            // btnImportarLog
            //
            btnImportarLog.Anchor = AnchorStyles.None;
            btnImportarLog.FlatStyle = FlatStyle.Standard;
            btnImportarLog.ForeColor = SystemColors.ControlText;
            btnImportarLog.Location = new Point(1069, 19);
            btnImportarLog.Name = "btnImportarLog";
            btnImportarLog.Size = new Size(150, 35);
            btnImportarLog.TabIndex = 3;
            btnImportarLog.Text = "⇩ LOG DE GREGAL ▾";
            toolTip.SetToolTip(btnImportarLog, "Importar un log de Gregal a una tabla aparte, o pasar de esa tabla a HISTORICO.");
            btnImportarLog.UseVisualStyleBackColor = true;
            btnImportarLog.Click += btnImportarLog_Click;
            //
            // btnAuditoria
            //
            btnAuditoria.Anchor = AnchorStyles.None;
            btnAuditoria.FlatStyle = FlatStyle.Standard;
            btnAuditoria.ForeColor = SystemColors.ControlText;
            btnAuditoria.Location = new Point(1089, 19);
            btnAuditoria.Name = "btnAuditoria";
            btnAuditoria.Size = new Size(120, 35);
            btnAuditoria.TabIndex = 3;
            btnAuditoria.Text = "🛡 AUDITORÍA";
            toolTip.SetToolTip(btnAuditoria, "Ver el registro de auditoría (solo administradores).");
            btnAuditoria.UseVisualStyleBackColor = true;
            btnAuditoria.Visible = false;
            btnAuditoria.Click += btnAuditoria_Click;
            //
            // btnConfiguracion
            //
            btnConfiguracion.Anchor = AnchorStyles.None;
            btnConfiguracion.FlatStyle = FlatStyle.Standard;
            btnConfiguracion.ForeColor = SystemColors.ControlText;
            btnConfiguracion.Location = new Point(1239, 19);
            btnConfiguracion.Name = "btnConfiguracion";
            btnConfiguracion.Size = new Size(120, 35);
            btnConfiguracion.TabIndex = 4;
            btnConfiguracion.Text = "⚙ CONFIG";
            toolTip.SetToolTip(btnConfiguracion, "Abrir configuración de la aplicación.");
            btnConfiguracion.UseVisualStyleBackColor = true;
            btnConfiguracion.Click += btnConfiguracion_Click;
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
            grid.BackgroundColor = SystemColors.Window;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.Dock = DockStyle.Fill;
            grid.Location = new Point(3, 3);
            grid.Name = "grid";
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new Size(872, 522);
            grid.TabIndex = 0;
            grid.SelectionChanged += grid_SelectionChanged;
            grid.CellDoubleClick += grid_CellDoubleClick;
            //
            // pnlLog
            //
            pnlLog.BackColor = SystemColors.Window;
            pnlLog.Controls.Add(txtLog);
            pnlLog.Dock = DockStyle.Fill;
            pnlLog.Location = new Point(3, 531);
            pnlLog.Name = "pnlLog";
            pnlLog.Size = new Size(872, 124);
            pnlLog.TabIndex = 1;
            //
            // txtLog
            //
            txtLog.BackColor = SystemColors.Window;
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 9F);
            txtLog.ForeColor = SystemColors.WindowText;
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
            tabIndividual.BackColor = SystemColors.Control;
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
            grpEdicion.ForeColor = SystemColors.ControlText;
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
            flpBotones.Controls.Add(btnEditarFicha);
            flpBotones.Controls.Add(btnEliminar);
            flpBotones.Dock = DockStyle.Bottom;
            flpBotones.FlowDirection = FlowDirection.TopDown;
            flpBotones.Location = new Point(3, 412);
            flpBotones.Name = "flpBotones";
            flpBotones.Padding = new Padding(10, 5, 10, 5);
            flpBotones.Size = new Size(330, 201);
            flpBotones.TabIndex = 0;
            //
            // btnNuevaPartida
            //
            btnNuevaPartida.FlatStyle = FlatStyle.Standard;
            btnNuevaPartida.ForeColor = SystemColors.ControlText;
            btnNuevaPartida.Location = new Point(13, 8);
            btnNuevaPartida.Name = "btnNuevaPartida";
            btnNuevaPartida.Size = new Size(310, 35);
            btnNuevaPartida.TabIndex = 0;
            btnNuevaPartida.Text = "✨ NUEVA PARTIDA";
            toolTip.SetToolTip(btnNuevaPartida, "Crear una nueva partida. Sugiere automáticamente la primera partida faltante.");
            btnNuevaPartida.UseVisualStyleBackColor = true;
            btnNuevaPartida.Click += btnNuevaPartida_Click;
            //
            // btnGuardar
            //
            btnGuardar.FlatStyle = FlatStyle.Standard;
            btnGuardar.ForeColor = SystemColors.ControlText;
            btnGuardar.Location = new Point(13, 49);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(310, 35);
            btnGuardar.TabIndex = 1;
            btnGuardar.Text = "💾 GUARDAR";
            toolTip.SetToolTip(btnGuardar, "Guardar los cambios de la partida seleccionada.");
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnEditarFicha
            //
            btnEditarFicha.FlatStyle = FlatStyle.Standard;
            btnEditarFicha.ForeColor = SystemColors.ControlText;
            btnEditarFicha.Location = new Point(13, 90);
            btnEditarFicha.Name = "btnEditarFicha";
            btnEditarFicha.Size = new Size(310, 35);
            btnEditarFicha.TabIndex = 2;
            btnEditarFicha.Text = "📝 EDITAR TODOS LOS CAMPOS";
            toolTip.SetToolTip(btnEditarFicha, "Abre la ficha completa de la partida seleccionada, con todos sus campos (también con doble clic en la tabla).");
            btnEditarFicha.UseVisualStyleBackColor = true;
            btnEditarFicha.Click += btnEditarFicha_Click;
            //
            // btnEliminar
            //
            btnEliminar.FlatStyle = FlatStyle.Standard;
            btnEliminar.ForeColor = SystemColors.ControlText;
            btnEliminar.Location = new Point(13, 131);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(310, 35);
            btnEliminar.TabIndex = 3;
            btnEliminar.Text = "🗑 ELIMINAR";
            toolTip.SetToolTip(btnEliminar, "Eliminar la partida seleccionada de la base de datos.");
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
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
            tlpEdicion.Dock = DockStyle.Top;
            tlpEdicion.Location = new Point(3, 22);
            tlpEdicion.Name = "tlpEdicion";
            tlpEdicion.RowCount = 5;
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpEdicion.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpEdicion.Size = new Size(330, 250);
            tlpEdicion.TabIndex = 1;
            //
            // lblPartida
            //
            lblPartida.Dock = DockStyle.Fill;
            lblPartida.ForeColor = SystemColors.ControlText;
            lblPartida.Location = new Point(3, 0);
            lblPartida.Name = "lblPartida";
            lblPartida.Size = new Size(126, 50);
            lblPartida.TabIndex = 0;
            lblPartida.Text = "Partida:";
            lblPartida.TextAlign = ContentAlignment.MiddleLeft;
            //
            // nudPartida
            //
            nudPartida.Dock = DockStyle.Fill;
            nudPartida.Location = new Point(135, 3);
            nudPartida.Name = "nudPartida";
            nudPartida.Size = new Size(192, 26);
            nudPartida.TabIndex = 1;
            toolTip.SetToolTip(nudPartida, "Número de la partida del día.");
            //
            // lblPrecio
            //
            lblPrecio.Dock = DockStyle.Fill;
            lblPrecio.ForeColor = SystemColors.ControlText;
            lblPrecio.Location = new Point(3, 50);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(126, 50);
            lblPrecio.TabIndex = 2;
            lblPrecio.Text = "Precio:";
            lblPrecio.TextAlign = ContentAlignment.MiddleLeft;
            //
            // nudPrecio
            //
            nudPrecio.Dock = DockStyle.Fill;
            nudPrecio.Location = new Point(135, 53);
            nudPrecio.Name = "nudPrecio";
            nudPrecio.Size = new Size(192, 26);
            nudPrecio.TabIndex = 3;
            toolTip.SetToolTip(nudPrecio, "Precio de la partida en euros.");
            //
            // lblVendidos
            //
            lblVendidos.Dock = DockStyle.Fill;
            lblVendidos.ForeColor = SystemColors.ControlText;
            lblVendidos.Location = new Point(3, 100);
            lblVendidos.Name = "lblVendidos";
            lblVendidos.Size = new Size(126, 50);
            lblVendidos.TabIndex = 4;
            lblVendidos.Text = "Vendidos:";
            lblVendidos.TextAlign = ContentAlignment.MiddleLeft;
            //
            // nudVendidos
            //
            nudVendidos.Dock = DockStyle.Fill;
            nudVendidos.Location = new Point(135, 103);
            nudVendidos.Name = "nudVendidos";
            nudVendidos.Size = new Size(192, 26);
            nudVendidos.TabIndex = 5;
            toolTip.SetToolTip(nudVendidos, "Cantidad de cartones vendidos.");
            //
            // lblLinea
            //
            lblLinea.Dock = DockStyle.Fill;
            lblLinea.ForeColor = SystemColors.ControlText;
            lblLinea.Location = new Point(3, 150);
            lblLinea.Name = "lblLinea";
            lblLinea.Size = new Size(126, 50);
            lblLinea.TabIndex = 6;
            lblLinea.Text = "Línea:";
            lblLinea.TextAlign = ContentAlignment.MiddleLeft;
            //
            // nudLinea
            //
            nudLinea.Dock = DockStyle.Fill;
            nudLinea.Location = new Point(135, 153);
            nudLinea.Name = "nudLinea";
            nudLinea.Size = new Size(192, 26);
            nudLinea.TabIndex = 7;
            toolTip.SetToolTip(nudLinea, "Premio de línea.");
            //
            // lblBingo
            //
            lblBingo.Dock = DockStyle.Fill;
            lblBingo.ForeColor = SystemColors.ControlText;
            lblBingo.Location = new Point(3, 200);
            lblBingo.Name = "lblBingo";
            lblBingo.Size = new Size(126, 50);
            lblBingo.TabIndex = 8;
            lblBingo.Text = "Bingo:";
            lblBingo.TextAlign = ContentAlignment.MiddleLeft;
            //
            // nudBingo
            //
            nudBingo.Dock = DockStyle.Fill;
            nudBingo.Location = new Point(135, 203);
            nudBingo.Name = "nudBingo";
            nudBingo.Size = new Size(192, 26);
            nudBingo.TabIndex = 9;
            toolTip.SetToolTip(nudBingo, "Premio de bingo.");
            //
            // tabLote
            //
            tabLote.BackColor = SystemColors.Control;
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
            grpLote.ForeColor = SystemColors.ControlText;
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
            btnBuscarFaltante.FlatStyle = FlatStyle.Standard;
            btnBuscarFaltante.ForeColor = SystemColors.ControlText;
            btnBuscarFaltante.Location = new Point(13, 8);
            btnBuscarFaltante.Name = "btnBuscarFaltante";
            btnBuscarFaltante.Size = new Size(310, 35);
            btnBuscarFaltante.TabIndex = 0;
            btnBuscarFaltante.Text = "🔍 BUSCAR PRIMER FALTANTE";
            toolTip.SetToolTip(btnBuscarFaltante, "Busca todas las partidas faltantes en el rango y establece automáticamente el primer y último hueco.");
            btnBuscarFaltante.UseVisualStyleBackColor = true;
            btnBuscarFaltante.Click += btnBuscarFaltante_Click;
            //
            // btnGuardarLote
            //
            btnGuardarLote.FlatStyle = FlatStyle.Standard;
            btnGuardarLote.ForeColor = SystemColors.ControlText;
            btnGuardarLote.Location = new Point(13, 49);
            btnGuardarLote.Name = "btnGuardarLote";
            btnGuardarLote.Size = new Size(310, 35);
            btnGuardarLote.TabIndex = 1;
            btnGuardarLote.Text = "💾 GUARDAR LOTE";
            toolTip.SetToolTip(btnGuardarLote, "Guardar múltiples partidas del rango especificado.");
            btnGuardarLote.UseVisualStyleBackColor = true;
            btnGuardarLote.Click += btnGuardarLote_Click;
            //
            // btnEliminarLote
            //
            btnEliminarLote.FlatStyle = FlatStyle.Standard;
            btnEliminarLote.ForeColor = SystemColors.ControlText;
            btnEliminarLote.Location = new Point(13, 90);
            btnEliminarLote.Name = "btnEliminarLote";
            btnEliminarLote.Size = new Size(310, 35);
            btnEliminarLote.TabIndex = 2;
            btnEliminarLote.Text = "🗑 ELIMINAR LOTE";
            toolTip.SetToolTip(btnEliminarLote, "Eliminar todas las partidas en el rango especificado.");
            btnEliminarLote.UseVisualStyleBackColor = true;
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
            tlpLoteCampos.Controls.Add(lblLoteVendidos, 0, 2);
            tlpLoteCampos.Controls.Add(nudLoteVendidos, 1, 2);
            tlpLoteCampos.Controls.Add(lblLotePrecio, 0, 3);
            tlpLoteCampos.Controls.Add(nudLotePrecio, 1, 3);
            tlpLoteCampos.Dock = DockStyle.Top;
            tlpLoteCampos.Location = new Point(3, 22);
            tlpLoteCampos.Name = "tlpLoteCampos";
            tlpLoteCampos.RowCount = 4;
            tlpLoteCampos.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpLoteCampos.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpLoteCampos.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpLoteCampos.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpLoteCampos.Size = new Size(330, 180);
            tlpLoteCampos.TabIndex = 1;
            //
            // lblIni
            //
            lblIni.ForeColor = SystemColors.ControlText;
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
            toolTip.SetToolTip(nudPartidaInicial, "Primera partida del rango a procesar.");
            nudPartidaInicial.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lblFin
            //
            lblFin.ForeColor = SystemColors.ControlText;
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
            toolTip.SetToolTip(nudPartidaFinal, "Última partida del rango a procesar.");
            nudPartidaFinal.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lblLoteVendidos
            //
            lblLoteVendidos.Dock = DockStyle.Fill;
            lblLoteVendidos.ForeColor = SystemColors.ControlText;
            lblLoteVendidos.Location = new Point(3, 90);
            lblLoteVendidos.Name = "lblLoteVendidos";
            lblLoteVendidos.Size = new Size(126, 45);
            lblLoteVendidos.TabIndex = 4;
            lblLoteVendidos.Text = "Cartones Vendidos:";
            //
            // nudLoteVendidos
            //
            nudLoteVendidos.Dock = DockStyle.Fill;
            nudLoteVendidos.Location = new Point(135, 93);
            nudLoteVendidos.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            nudLoteVendidos.Name = "nudLoteVendidos";
            nudLoteVendidos.Size = new Size(192, 26);
            nudLoteVendidos.TabIndex = 5;
            toolTip.SetToolTip(nudLoteVendidos, "Cartones vendidos para todas las partidas del lote.");
            //
            // lblLotePrecio
            //
            lblLotePrecio.Dock = DockStyle.Fill;
            lblLotePrecio.ForeColor = SystemColors.ControlText;
            lblLotePrecio.Location = new Point(3, 135);
            lblLotePrecio.Name = "lblLotePrecio";
            lblLotePrecio.Size = new Size(126, 45);
            lblLotePrecio.TabIndex = 6;
            lblLotePrecio.Text = "Precio Partida:";
            //
            // nudLotePrecio
            //
            nudLotePrecio.DecimalPlaces = 2;
            nudLotePrecio.Dock = DockStyle.Fill;
            nudLotePrecio.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            nudLotePrecio.Location = new Point(135, 138);
            nudLotePrecio.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            nudLotePrecio.Name = "nudLotePrecio";
            nudLotePrecio.Size = new Size(192, 26);
            nudLotePrecio.TabIndex = 7;
            toolTip.SetToolTip(nudLotePrecio, "Precio por partida para todas las partidas del lote.");
            //
            // MainForm
            //
            BackColor = SystemColors.Control;
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
            tlpEdicion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudPartida).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVendidos).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLinea).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudBingo).EndInit();
            tabLote.ResumeLayout(false);
            grpLote.ResumeLayout(false);
            flpBotonesLote.ResumeLayout(false);
            tlpLoteCampos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudPartidaInicial).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPartidaFinal).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLoteVendidos).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLotePrecio).EndInit();
            ResumeLayout(false);
        }

        private void AgregarControlEdicion(string texto, System.Windows.Forms.Control ctrl, int row) {
            var lbl = new System.Windows.Forms.Label {
                Text = texto,
                ForeColor = System.Drawing.SystemColors.ControlText,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Dock = System.Windows.Forms.DockStyle.Fill
            };
            ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEdicion.Controls.Add(lbl, 0, row);
            this.tlpEdicion.Controls.Add(ctrl, 1, row);
        }

        #endregion
    }
}