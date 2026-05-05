namespace BingoHistoricoEditor {
    partial class MainForm {
        private System.ComponentModel.IContainer components = null;

        // ── Controles ─────────────────────────────────────────────────────────
        private Panel pnlTop;
        private Panel pnlLeft;
        private Panel pnlBottom;
        private Label lblTitulo;
        private Label lblSalaConc;
        private Label lblEstado;
        private Label lblFecha;
        private DateTimePicker dtpFecha;
        private Button btnBuscar;
        private DataGridView grid;
        private Label lblTotal;
        private GroupBox grpEdicion;
        private NumericUpDown nudPartida;
        private NumericUpDown nudPrecio;
        private NumericUpDown nudVendidos;
        private NumericUpDown nudLinea;
        private NumericUpDown nudBingo;
        private Label lblVentaPreview;
        private Button btnGuardar;
        private Button btnNueva;
        private Button btnEliminar;
        private Label lblConc;
        private ComboBox cmbConc;
        private Button btnVerificarHuecos;
        private TableLayoutPanel tlpMainRight; // Contenedor principal de la derecha
        private TableLayoutPanel tlpTop;
        private FlowLayoutPanel pnlBusqueda;
        private Panel pnlBotones;
        private TableLayoutPanel tlpEdicion;

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
            pnlBusqueda = new FlowLayoutPanel();
            lblConc = new Label();
            cmbConc = new ComboBox();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            btnBuscar = new Button();
            lblTotal = new Label();
            pnlBottom = new Panel();
            tlpMainRight = new TableLayoutPanel();
            grpEdicion = new GroupBox();
            tlpEdicion = new TableLayoutPanel();
            nudPartida = new NumericUpDown();
            nudPrecio = new NumericUpDown();
            nudVendidos = new NumericUpDown();
            nudLinea = new NumericUpDown();
            nudBingo = new NumericUpDown();
            lblVentaPreview = new Label();
            pnlBotones = new Panel();
            btnVerificarHuecos = new Button();
            btnEliminar = new Button();
            btnGuardar = new Button();
            btnNueva = new Button();

            pnlTop.SuspendLayout();
            tlpTop.SuspendLayout();
            pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
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

            // ── pnlTop ──────────────────────────────────────────────────────────
            pnlTop.BackColor = Color.FromArgb(26, 29, 40);
            pnlTop.Controls.Add(tlpTop);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 60;

            tlpTop.ColumnCount = 3;
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpTop.Controls.Add(lblTitulo, 0, 0);
            tlpTop.Controls.Add(lblSalaConc, 1, 0);
            tlpTop.Controls.Add(lblEstado, 2, 0);
            tlpTop.Dock = DockStyle.Fill;

            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Text = "HISTÓRICO BINGO";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblTitulo.Dock = DockStyle.Fill;

            lblSalaConc.ForeColor = Color.FromArgb(180, 180, 200);
            lblSalaConc.TextAlign = ContentAlignment.MiddleCenter;
            lblSalaConc.Dock = DockStyle.Fill;

            lblEstado.TextAlign = ContentAlignment.MiddleRight;
            lblEstado.Dock = DockStyle.Fill;

            // ── pnlLeft (Grid) ──────────────────────────────────────────────────
            pnlLeft.Controls.Add(grid);
            pnlLeft.Controls.Add(pnlBusqueda);
            pnlLeft.Controls.Add(lblTotal);
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.Padding = new Padding(12);

            grid.BackgroundColor = Color.FromArgb(30, 34, 46);
            grid.BorderStyle = BorderStyle.None;
            grid.Dock = DockStyle.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.SelectionChanged += grid_SelectionChanged;

            pnlBusqueda.Controls.Add(lblConc);
            pnlBusqueda.Controls.Add(cmbConc);
            pnlBusqueda.Controls.Add(lblFecha);
            pnlBusqueda.Controls.Add(dtpFecha);
            pnlBusqueda.Controls.Add(btnBuscar);
            pnlBusqueda.Dock = DockStyle.Top;
            pnlBusqueda.Height = 45;

            lblConc.Text = "Concesión:";
            lblConc.Padding = new Padding(0, 7, 5, 0);
            lblConc.AutoSize = true;

            cmbConc.Size = new Size(80, 27);
            cmbConc.DropDownStyle = ComboBoxStyle.DropDownList;

            lblFecha.Text = "Fecha:";
            lblFecha.Padding = new Padding(0, 7, 5, 0);
            lblFecha.AutoSize = true;

            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Width = 110;

            btnBuscar.BackColor = Color.FromArgb(0, 120, 215);
            btnBuscar.ForeColor = Color.White;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            // Ajustes para que el texto "Cargar" quepa correctamente dentro del botón
            btnBuscar.AutoSize = true;
            btnBuscar.AutoSizeMode = AutoSizeMode.GrowOnly;
            btnBuscar.Padding = new Padding(12, 6, 12, 6);
            btnBuscar.Text = "Cargar";
            btnBuscar.Click += btnBuscar_Click;

            lblTotal.Dock = DockStyle.Bottom;
            lblTotal.Height = 25;
            lblTotal.ForeColor = Color.FromArgb(150, 160, 180);

            // ── pnlBottom (Panel Derecho Rediseñado) ───────────────────────────
            pnlBottom.BackColor = Color.FromArgb(22, 24, 32);
            pnlBottom.Controls.Add(tlpMainRight); // Usamos TableLayoutPanel para dividir 50/50
            pnlBottom.Dock = DockStyle.Right;
            pnlBottom.Width = 280;

            tlpMainRight.ColumnCount = 1;
            tlpMainRight.RowCount = 2;
            tlpMainRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMainRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50F)); // Panel Edición (Arriba)
            tlpMainRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50F)); // Panel Botones (Abajo)
            tlpMainRight.Controls.Add(grpEdicion, 0, 0);
            tlpMainRight.Controls.Add(pnlBotones, 0, 1);
            tlpMainRight.Dock = DockStyle.Fill;
            tlpMainRight.Padding = new Padding(10);

            // --- Grupo Edición (50% superior) ---
            grpEdicion.Controls.Add(tlpEdicion);
            grpEdicion.Dock = DockStyle.Fill;
            grpEdicion.ForeColor = Color.FromArgb(99, 179, 237);
            grpEdicion.Text = " Edición de Partida ";

            tlpEdicion.ColumnCount = 2;
            tlpEdicion.RowCount = 6;
            tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpEdicion.Dock = DockStyle.Fill;

            // Labels y Controles (Se añaden pares Label/Numeric)
            HistoricoLabel(tlpEdicion, "Partida:", 0, 0);
            tlpEdicion.Controls.Add(nudPartida, 1, 0);
            HistoricoLabel(tlpEdicion, "Precio:", 0, 1);
            tlpEdicion.Controls.Add(nudPrecio, 1, 1);
            HistoricoLabel(tlpEdicion, "Vendidos:", 0, 2);
            tlpEdicion.Controls.Add(nudVendidos, 1, 2);
            HistoricoLabel(tlpEdicion, "Línea:", 0, 3);
            tlpEdicion.Controls.Add(nudLinea, 1, 3);
            HistoricoLabel(tlpEdicion, "Bingo:", 0, 4);
            tlpEdicion.Controls.Add(nudBingo, 1, 4);
            tlpEdicion.Controls.Add(lblVentaPreview, 0, 5);
            tlpEdicion.SetColumnSpan(lblVentaPreview, 2);

            nudPartida.Dock = nudPrecio.Dock = nudVendidos.Dock = nudLinea.Dock = nudBingo.Dock = DockStyle.Top;
            nudPrecio.DecimalPlaces = 2;
            nudLinea.DecimalPlaces = 2;
            nudBingo.DecimalPlaces = 2;

            lblVentaPreview.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblVentaPreview.ForeColor = Color.FromArgb(255, 193, 7);
            lblVentaPreview.TextAlign = ContentAlignment.MiddleCenter;
            lblVentaPreview.Dock = DockStyle.Fill;

            // --- Panel Botones (50% inferior) ---
            pnlBotones.Controls.Add(btnEliminar);
            pnlBotones.Controls.Add(btnVerificarHuecos);
            pnlBotones.Controls.Add(btnGuardar);
            pnlBotones.Controls.Add(btnNueva);
            pnlBotones.Dock = DockStyle.Fill;
            pnlBotones.Padding = new Padding(0, 10, 0, 0);

            btnNueva.Dock = DockStyle.Top;
            btnNueva.Height = 45;
            btnNueva.Text = "NUEVA PARTIDA";
            btnNueva.BackColor = Color.FromArgb(70, 80, 100);
            btnNueva.ForeColor = Color.White;
            btnNueva.FlatStyle = FlatStyle.Flat;
            btnNueva.Click += btnNueva_Click;

            btnGuardar.Dock = DockStyle.Top;
            btnGuardar.Height = 55;
            btnGuardar.Text = "GUARDAR";
            btnGuardar.BackColor = Color.FromArgb(40, 167, 69);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Margin = new Padding(0, 5, 0, 5);
            btnGuardar.Click += btnGuardar_Click;

            btnVerificarHuecos.Dock = DockStyle.Top;
            btnVerificarHuecos.Height = 45;
            btnVerificarHuecos.Text = "VERIFICAR HUECOS";
            btnVerificarHuecos.BackColor = Color.FromArgb(210, 120, 0);
            btnVerificarHuecos.ForeColor = Color.White;
            btnVerificarHuecos.FlatStyle = FlatStyle.Flat;
            btnVerificarHuecos.Click += btnVerificarHuecos_Click;

            btnEliminar.Dock = DockStyle.Bottom;
            btnEliminar.Height = 45;
            btnEliminar.Text = "ELIMINAR";
            btnEliminar.BackColor = Color.FromArgb(180, 50, 50);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Click += btnEliminar_Click;

            // ── MainForm ────────────────────────────────────────────────────────
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

        // Helper para crear labels de edición rápidamente
        private void HistoricoLabel(TableLayoutPanel parent, string texto, int col, int row) {
            var lbl = new Label {
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