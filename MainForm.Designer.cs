namespace BingoHistoricoEditor {
    partial class MainForm {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();

            var clrFondo = System.Drawing.Color.FromArgb(30,  30,  35);
            var clrPanel = System.Drawing.Color.FromArgb(45,  45,  50);
            var clrVerde = System.Drawing.Color.FromArgb(40, 167,  69);
            var clrRojo  = System.Drawing.Color.FromArgb(220, 53,  69);
            var clrAzul  = System.Drawing.Color.FromArgb(  0,122, 204);
            var clrTexto = System.Drawing.Color.White;

            // ── Instanciación ─────────────────────────────────────────────────
            this.tlpMain       = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop        = new System.Windows.Forms.Panel();
            this.tlpContent    = new System.Windows.Forms.TableLayoutPanel();
            this.pnlRight      = new System.Windows.Forms.Panel();
            this.grpEdicion    = new System.Windows.Forms.GroupBox();
            this.tlpEdicion    = new System.Windows.Forms.TableLayoutPanel();
            this.flpBotones    = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlLog        = new System.Windows.Forms.Panel();
            this.lblTitulo     = new System.Windows.Forms.Label();
            this.lblSalaConc   = new System.Windows.Forms.Label();
            this.lblEstado     = new System.Windows.Forms.Label();
            this.dtpFecha      = new System.Windows.Forms.DateTimePicker();
            this.btnBuscar     = new System.Windows.Forms.Button();
            this.grid          = new System.Windows.Forms.DataGridView();
            this.txtLog        = new System.Windows.Forms.TextBox();
            this.nudPartida    = new System.Windows.Forms.NumericUpDown();
            this.nudPrecio     = new System.Windows.Forms.NumericUpDown();
            this.nudVendidos   = new System.Windows.Forms.NumericUpDown();
            this.nudLinea      = new System.Windows.Forms.NumericUpDown();
            this.nudBingo      = new System.Windows.Forms.NumericUpDown();
            this.btnNuevaPartida = new System.Windows.Forms.Button();
            this.btnGuardar    = new System.Windows.Forms.Button();
            this.btnEliminar   = new System.Windows.Forms.Button();

            // ── Layout principal: 3 filas (cabecera / contenido / log) ─────────
            this.tlpMain.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.RowCount    = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(
                System.Windows.Forms.SizeType.Percent, 100f));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(
                System.Windows.Forms.SizeType.Absolute, 80f));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(
                System.Windows.Forms.SizeType.Percent, 100f));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(
                System.Windows.Forms.SizeType.Absolute, 120f));

            // ── Cabecera ──────────────────────────────────────────────────────
            this.pnlTop.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.BackColor = clrPanel;

            this.lblTitulo.Text      = "HISTÓRICO DE PARTIDAS";
            this.lblTitulo.Font      = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = clrTexto;
            this.lblTitulo.Location  = new System.Drawing.Point(20, 15);
            this.lblTitulo.AutoSize  = true;

            this.lblSalaConc.Text      = "SALA: 0 | CONCESIONARIO: 0";
            this.lblSalaConc.ForeColor = System.Drawing.Color.DarkGray;
            this.lblSalaConc.Location  = new System.Drawing.Point(22, 48);
            this.lblSalaConc.AutoSize  = true;

            // FIX 1: lblEstado sin Dock, posición fija antes del dtpFecha
            // (DockStyle.Right tapaba el DateTimePicker y el botón Buscar)
            this.lblEstado.Text      = "Conectando...";
            this.lblEstado.ForeColor = System.Drawing.Color.Orange;
            this.lblEstado.Font      = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Italic);
            this.lblEstado.Location  = new System.Drawing.Point(700, 30);
            this.lblEstado.Size      = new System.Drawing.Size(220, 20);
            this.lblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.dtpFecha.Format   = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Size     = new System.Drawing.Size(130, 28);
            this.dtpFecha.Location = new System.Drawing.Point(935, 24);
            this.dtpFecha.Font     = new System.Drawing.Font("Segoe UI", 10f);

            this.btnBuscar.Text      = "🔍 BUSCAR";
            this.btnBuscar.Size      = new System.Drawing.Size(120, 33);
            this.btnBuscar.Location  = new System.Drawing.Point(1075, 22);
            this.btnBuscar.BackColor = clrVerde;
            this.btnBuscar.ForeColor = clrTexto;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font      = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            this.btnBuscar.Click    += new System.EventHandler(this.btnBuscar_Click);

            this.pnlTop.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblTitulo, this.lblSalaConc,
                this.lblEstado, this.dtpFecha, this.btnBuscar
            });

            // ── Cuerpo: grid (izq) + edición (der) ───────────────────────────
            this.tlpContent.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.ColumnCount = 2;
            this.tlpContent.RowCount    = 1;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(
                System.Windows.Forms.SizeType.Percent, 75f));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(
                System.Windows.Forms.SizeType.Absolute, 350f));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(
                System.Windows.Forms.SizeType.Percent, 100f));

            this.grid.Dock           = System.Windows.Forms.DockStyle.Fill;
            this.grid.BackgroundColor = clrFondo;
            this.grid.BorderStyle    = System.Windows.Forms.BorderStyle.None;
            this.grid.SelectionMode  = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);

            // ── Panel derecho ────────────────────────────────────────────────
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;

            this.grpEdicion.Text      = " DATOS PARTIDA ";
            this.grpEdicion.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.grpEdicion.ForeColor = System.Drawing.Color.Gold;
            this.grpEdicion.Font      = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);

            // FIX 2: tlpEdicion con RowStyles explícitos 20%×5 y ColumnStyles
            // (sin ellos las filas son AutoSize y Bingo queda fuera del área visible)
            this.tlpEdicion.Dock        = System.Windows.Forms.DockStyle.Top;
            this.tlpEdicion.Height      = 200;
            this.tlpEdicion.ColumnCount = 2;
            this.tlpEdicion.RowCount    = 5;
            this.tlpEdicion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(
                System.Windows.Forms.SizeType.Percent, 40f));
            this.tlpEdicion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(
                System.Windows.Forms.SizeType.Percent, 60f));
            

            this.nudPrecio.DecimalPlaces   = 2;
            this.nudLinea.DecimalPlaces    = 2;
            this.nudBingo.DecimalPlaces    = 2;

            this.AgregarControlEdicion("Partida:",  this.nudPartida,  0);
            this.AgregarControlEdicion("Precio:",   this.nudPrecio,   1);
            this.AgregarControlEdicion("Vendidos:", this.nudVendidos, 2);
            this.AgregarControlEdicion("Línea:",    this.nudLinea,    3);
            this.AgregarControlEdicion("Bingo:",    this.nudBingo,    4);

            // ── Botones ───────────────────────────────────────────────────────
            this.flpBotones.Dock          = System.Windows.Forms.DockStyle.Bottom;
            this.flpBotones.Height        = 165;
            this.flpBotones.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpBotones.Padding       = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.flpBotones.BackColor     = System.Drawing.Color.Transparent;

            this.btnNuevaPartida.Text      = "✨  NUEVA PARTIDA";
            this.btnNuevaPartida.Size      = new System.Drawing.Size(306, 38);
            this.btnNuevaPartida.BackColor = clrAzul;
            this.btnNuevaPartida.ForeColor = clrTexto;
            this.btnNuevaPartida.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevaPartida.Font      = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            this.btnNuevaPartida.Margin    = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.btnNuevaPartida.Click    += new System.EventHandler(this.btnNuevaPartida_Click);

            this.btnGuardar.Text      = "💾  GUARDAR";
            this.btnGuardar.Size      = new System.Drawing.Size(306, 38);
            this.btnGuardar.BackColor = clrVerde;
            this.btnGuardar.ForeColor = clrTexto;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font      = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            this.btnGuardar.Margin    = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.btnGuardar.Click    += new System.EventHandler(this.btnGuardar_Click);

            this.btnEliminar.Text      = "🗑  ELIMINAR";
            this.btnEliminar.Size      = new System.Drawing.Size(306, 38);
            this.btnEliminar.BackColor = clrRojo;
            this.btnEliminar.ForeColor = clrTexto;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font      = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            this.btnEliminar.Click    += new System.EventHandler(this.btnEliminar_Click);

            this.flpBotones.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnNuevaPartida, this.btnGuardar, this.btnEliminar
            });

            // Orden de añadir a grpEdicion: Bottom primero, luego Top
            // (WinForms procesa Dock de atrás hacia adelante en Controls)
            this.grpEdicion.Controls.Add(this.flpBotones);
            this.grpEdicion.Controls.Add(this.tlpEdicion);
            this.pnlRight.Controls.Add(this.grpEdicion);

            this.tlpContent.Controls.Add(this.grid,     0, 0);
            this.tlpContent.Controls.Add(this.pnlRight, 1, 0);

            // ── Log inferior ──────────────────────────────────────────────────
            this.pnlLog.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.pnlLog.BackColor = System.Drawing.Color.Black;

            this.txtLog.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Multiline   = true;
            this.txtLog.ReadOnly    = true;
            this.txtLog.ScrollBars  = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.BackColor   = System.Drawing.Color.Black;
            this.txtLog.ForeColor   = System.Drawing.Color.LimeGreen;
            this.txtLog.Font        = new System.Drawing.Font("Consolas", 9f);
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pnlLog.Controls.Add(this.txtLog);

            // ── Ensamblaje final ──────────────────────────────────────────────
            this.tlpMain.Controls.Add(this.pnlTop,     0, 0);
            this.tlpMain.Controls.Add(this.tlpContent, 0, 1);
            this.tlpMain.Controls.Add(this.pnlLog,     0, 2);
            this.Controls.Add(this.tlpMain);

            this.ClientSize     = new System.Drawing.Size(1250, 750);
            this.Text           = "Bingo Histórico - Editor";
            this.BackColor      = clrFondo;
            this.StartPosition  = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void AgregarControlEdicion(string texto, System.Windows.Forms.Control ctrl, int row) {
            var lbl = new System.Windows.Forms.Label {
                Text      = texto,
                ForeColor = System.Drawing.Color.LightGray,
                TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                Dock      = System.Windows.Forms.DockStyle.Fill,
                Padding   = new System.Windows.Forms.Padding(0, 0, 8, 0),
            };
            ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEdicion.Controls.Add(lbl,  0, row);
            this.tlpEdicion.Controls.Add(ctrl, 1, row);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel  tlpMain;
        private System.Windows.Forms.Panel             pnlTop;
        private System.Windows.Forms.TableLayoutPanel  tlpContent;
        private System.Windows.Forms.Panel             pnlRight;
        private System.Windows.Forms.GroupBox          grpEdicion;
        private System.Windows.Forms.TableLayoutPanel  tlpEdicion;
        private System.Windows.Forms.FlowLayoutPanel   flpBotones;
        private System.Windows.Forms.Panel             pnlLog;
        private System.Windows.Forms.Label             lblTitulo;
        private System.Windows.Forms.Label             lblSalaConc;
        private System.Windows.Forms.Label             lblEstado;
        private System.Windows.Forms.DateTimePicker    dtpFecha;
        private System.Windows.Forms.Button            btnBuscar;
        private System.Windows.Forms.DataGridView      grid;
        private System.Windows.Forms.TextBox           txtLog;
        private System.Windows.Forms.NumericUpDown     nudPartida;
        private System.Windows.Forms.NumericUpDown     nudPrecio;
        private System.Windows.Forms.NumericUpDown     nudVendidos;
        private System.Windows.Forms.NumericUpDown     nudLinea;
        private System.Windows.Forms.NumericUpDown     nudBingo;
        private System.Windows.Forms.Button            btnNuevaPartida;
        private System.Windows.Forms.Button            btnGuardar;
        private System.Windows.Forms.Button            btnEliminar;
    }
}
