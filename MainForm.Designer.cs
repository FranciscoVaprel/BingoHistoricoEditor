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

            var clrFondo = System.Drawing.Color.FromArgb(30, 30, 35);
            var clrPanel = System.Drawing.Color.FromArgb(45, 45, 50);
            var clrVerde = System.Drawing.Color.FromArgb(40, 167, 69);
            var clrRojo = System.Drawing.Color.FromArgb(220, 53, 69);
            var clrAzul = System.Drawing.Color.FromArgb(0, 122, 204);
            var clrTexto = System.Drawing.Color.White;

            // 1. INSTANCIACIÓN
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.tlpHeader = new System.Windows.Forms.TableLayoutPanel(); // NUEVO: Para organizar la cabecera
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.tlpIzquierda = new System.Windows.Forms.TableLayoutPanel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.grpEdicion = new System.Windows.Forms.GroupBox();
            this.tlpEdicion = new System.Windows.Forms.TableLayoutPanel();
            this.flpBotones = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlLog = new System.Windows.Forms.Panel();

            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblSalaConc = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.grid = new System.Windows.Forms.DataGridView();
            this.txtLog = new System.Windows.Forms.TextBox();

            this.nudPartida = new System.Windows.Forms.NumericUpDown();
            this.nudPrecio = new System.Windows.Forms.NumericUpDown();
            this.nudVendidos = new System.Windows.Forms.NumericUpDown();
            this.nudLinea = new System.Windows.Forms.NumericUpDown();
            this.nudBingo = new System.Windows.Forms.NumericUpDown();

            this.btnNuevaPartida = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();

            // --- LAYOUT RAÍZ ---
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80f));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));

            // --- CABECERA MEJORADA (tlpHeader) ---
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.BackColor = clrPanel;

            this.tlpHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHeader.ColumnCount = 3;
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f)); // Títulos
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150f)); // Fecha
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150f)); // Botón Buscar
            this.tlpHeader.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);

            // Contenedor para textos (Izquierda)
            var pnlTextos = new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Fill };
            this.lblTitulo.Text = "HISTÓRICO DE PARTIDAS";
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = clrTexto;
            this.lblTitulo.Location = new System.Drawing.Point(0, 5);
            this.lblTitulo.AutoSize = true;

            this.lblSalaConc.Text = "SALA: 0 | CONCESIONARIO: 0";
            this.lblSalaConc.ForeColor = System.Drawing.Color.DarkGray;
            this.lblSalaConc.Location = new System.Drawing.Point(2, 35);
            this.lblSalaConc.AutoSize = true;

            this.lblEstado.Text = "Estado: Desconectado";
            this.lblEstado.ForeColor = System.Drawing.Color.Orange;
            this.lblEstado.Location = new System.Drawing.Point(220, 35);
            this.lblEstado.AutoSize = true;
            pnlTextos.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblTitulo, this.lblSalaConc, this.lblEstado });

            // Selector de Fecha (Centro-Derecha)
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dtpFecha.Width = 120;

            // Botón Buscar (Derecha)
            this.btnBuscar.Text = "🔍 BUSCAR";
            this.btnBuscar.BackColor = clrVerde;
            this.btnBuscar.ForeColor = clrTexto;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);

            this.tlpHeader.Controls.Add(pnlTextos, 0, 0);
            this.tlpHeader.Controls.Add(this.dtpFecha, 1, 0);
            this.tlpHeader.Controls.Add(this.btnBuscar, 2, 0);
            this.pnlTop.Controls.Add(this.tlpHeader);

            // --- CUERPO (Grid + Log izquierda, Edición derecha total) ---
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.ColumnCount = 2;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350f));

            // Sub-layout Izquierdo
            this.tlpIzquierda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpIzquierda.RowCount = 2;
            this.tlpIzquierda.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
            this.tlpIzquierda.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130f));

            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.BackgroundColor = clrFondo;
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.pnlLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Multiline = true;
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.ForeColor = System.Drawing.Color.LimeGreen;
            this.txtLog.ReadOnly = true;
            this.pnlLog.Controls.Add(this.txtLog);

            this.tlpIzquierda.Controls.Add(this.grid, 0, 0);
            this.tlpIzquierda.Controls.Add(this.pnlLog, 0, 1);

            // Panel Derecho (Llega hasta el final)
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEdicion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEdicion.Text = " DATOS PARTIDA ";
            this.grpEdicion.ForeColor = System.Drawing.Color.Gold;

            this.tlpEdicion.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpEdicion.Height = 250;
            this.tlpEdicion.RowCount = 5;
            for (int i = 0; i < 5; i++)
                this.tlpEdicion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));

            this.AgregarControlEdicion("Partida:", this.nudPartida, 0);
            this.AgregarControlEdicion("Precio:", this.nudPrecio, 1);
            this.AgregarControlEdicion("Vendidos:", this.nudVendidos, 2);
            this.AgregarControlEdicion("Línea:", this.nudLinea, 3);
            this.AgregarControlEdicion("Bingo:", this.nudBingo, 4);

            this.flpBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpBotones.Height = 160;
            this.flpBotones.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpBotones.Padding = new System.Windows.Forms.Padding(15);

            this.btnNuevaPartida.Text = "✨ NUEVA PARTIDA";
            this.btnNuevaPartida.Size = new System.Drawing.Size(300, 35);
            this.btnNuevaPartida.BackColor = clrAzul;
            this.btnNuevaPartida.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevaPartida.ForeColor = clrTexto;
            this.btnNuevaPartida.Click += new System.EventHandler(this.btnNuevaPartida_Click);

            this.btnGuardar.Text = "💾 GUARDAR";
            this.btnGuardar.Size = new System.Drawing.Size(300, 35);
            this.btnGuardar.BackColor = clrVerde;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.ForeColor = clrTexto;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);

            this.btnEliminar.Text = "🗑 ELIMINAR";
            this.btnEliminar.Size = new System.Drawing.Size(300, 35);
            this.btnEliminar.BackColor = clrRojo;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.ForeColor = clrTexto;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            this.flpBotones.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnNuevaPartida, this.btnGuardar, this.btnEliminar
            });

            this.grpEdicion.Controls.Add(this.flpBotones);
            this.grpEdicion.Controls.Add(this.tlpEdicion);
            this.pnlRight.Controls.Add(this.grpEdicion);

            // ENSAMBLAJE
            this.tlpContent.Controls.Add(this.tlpIzquierda, 0, 0);
            this.tlpContent.Controls.Add(this.pnlRight, 1, 0);

            this.tlpMain.Controls.Add(this.pnlTop, 0, 0);
            this.tlpMain.Controls.Add(this.tlpContent, 0, 1);

            this.Controls.Add(this.tlpMain);
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.Text = "Bingo Histórico - Editor";
            this.BackColor = clrFondo;
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
    }
}