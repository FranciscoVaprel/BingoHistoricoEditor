namespace BingoHistoricoEditor {
    partial class MainForm {
        private System.ComponentModel.IContainer components = null;

        // ── Controles ─────────────────────────────────────────────────────────
        private Panel        pnlTop;
        private Panel        pnlLeft;
        private Panel        pnlBottom;
        private Label        lblTitulo;
        private Label        lblSalaConc;
        private Label        lblEstado;
        private Label        lblFecha;
        private DateTimePicker dtpFecha;
        private Button       btnBuscar;
        private DataGridView grid;
        private Label        lblTotal;
        private GroupBox     grpEdicion;
        private Label        lblPartida;
        private NumericUpDown nudPartida;
        private Label        lblPrecio;
        private NumericUpDown nudPrecio;
        private Label        lblVendidos;
        private NumericUpDown nudVendidos;
        private Label        lblLinea;
        private NumericUpDown nudLinea;
        private Label        lblBingo;
        private NumericUpDown nudBingo;
        private Label        lblVentaPreview;
        private Button       btnGuardar;
        private Button       btnNueva;
        private Button       btnEliminar;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            components = new System.ComponentModel.Container();

            // ── Paleta de colores ─────────────────────────────────────────────
            var clrFondo       = Color.FromArgb(18,  20,  28);
            var clrPanel       = Color.FromArgb(26,  29,  40);
            var clrGrupo       = Color.FromArgb(32,  36,  50);
            var clrBorde       = Color.FromArgb(48,  54,  72);
            var clrTexto       = Color.FromArgb(220, 225, 240);
            var clrSubtexto    = Color.FromArgb(130, 140, 165);
            var clrAcento      = Color.FromArgb( 99, 179, 237);   // azul
            var clrVerde       = Color.FromArgb( 72, 187, 120);
            var clrRojo        = Color.FromArgb(220,  80,  80);
            var clrNaranja     = Color.FromArgb(237, 137,  54);
            var fntTitulo      = new Font("Segoe UI", 15.5f, FontStyle.Bold);
            var fntLabel       = new Font("Segoe UI", 10f);
            var fntLabelBold   = new Font("Segoe UI", 10f, FontStyle.Bold);
            var fntMono        = new Font("Consolas", 11f);
            var fntGrid        = new Font("Segoe UI", 10f);

            SuspendLayout();

            // ── Formulario ────────────────────────────────────────────────────
            Text            = "Histórico Bingo — Editor Manual";
            Size            = new Size(900, 640);
            MinimumSize     = new Size(820, 560);
            BackColor       = clrFondo;
            ForeColor       = clrTexto;
            Font            = fntLabel;
            AutoScaleMode   = AutoScaleMode.Dpi;
            StartPosition   = FormStartPosition.CenterScreen;

            // ── Panel superior ────────────────────────────────────────────────
            pnlTop = new Panel {
                Dock      = DockStyle.Top,
                Height    = 70,
                BackColor = clrPanel,
                Padding   = new Padding(16, 0, 16, 0),
            };

            var tlpTop = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
            };
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            pnlTop.Controls.Add(tlpTop);

            lblTitulo = new Label {
                Text      = "HISTÓRICO",
                Font      = fntTitulo,
                ForeColor = clrAcento,
                AutoSize  = true,
                Anchor    = AnchorStyles.Left,
            };

            lblSalaConc = new Label {
                Text      = "Sala: —  |  Conc: —",
                Font      = fntLabel,
                ForeColor = clrSubtexto,
                AutoSize  = true,
                Anchor    = AnchorStyles.Left,
                Margin    = new Padding(20, 0, 0, 0),
            };

            lblEstado = new Label {
                Text      = "Conectando…",
                Font      = fntLabel,
                ForeColor = clrSubtexto,
                AutoSize  = true,
                Anchor    = AnchorStyles.Right,
            };

            tlpTop.Controls.Add(lblTitulo, 0, 0);
            tlpTop.Controls.Add(lblSalaConc, 1, 0);
            tlpTop.Controls.Add(lblEstado, 2, 0);

            // ── Panel izquierdo: búsqueda + grid ──────────────────────────────
            pnlLeft = new Panel {
                Dock      = DockStyle.Fill,
                Padding   = new Padding(12),
                BackColor = clrFondo,
            };

            // Barra de búsqueda por fecha
            var pnlBusqueda = new FlowLayoutPanel {
                Height    = 45,
                Dock      = DockStyle.Top,
                BackColor = clrFondo,
                Padding   = new Padding(0, 8, 0, 0),
            };

            lblFecha = new Label {
                Text     = "Fecha sesión:",
                Font     = fntLabel,
                ForeColor= clrSubtexto,
                AutoSize = true,
                Margin   = new Padding(0, 6, 8, 0),
            };

            dtpFecha = new DateTimePicker {
                Format     = DateTimePickerFormat.Short,
                Width      = 120,
                BackColor  = clrGrupo,
                ForeColor  = clrTexto,
                CalendarMonthBackground = clrPanel,
            };

            btnBuscar = CrearBoton("Buscar", clrAcento, clrFondo, Point.Empty, new Size(90, 30));
            btnBuscar.Click += btnBuscar_Click;

            pnlBusqueda.Controls.AddRange(new Control[] { lblFecha, dtpFecha, btnBuscar });

            // Grid
            grid = new DataGridView {
                Dock                  = DockStyle.Fill,
                BackgroundColor       = clrPanel,
                BorderStyle           = BorderStyle.None,
                CellBorderStyle       = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                GridColor             = clrBorde,
                SelectionMode         = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect           = false,
                ReadOnly              = true,
                AllowUserToAddRows    = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible     = false,
                AutoSizeColumnsMode   = DataGridViewAutoSizeColumnsMode.Fill,
                Font                  = fntGrid,
                RowTemplate           = { Height = 28 },
            };
            grid.DefaultCellStyle.BackColor            = clrPanel;
            grid.DefaultCellStyle.ForeColor            = clrTexto;
            grid.DefaultCellStyle.SelectionBackColor   = Color.FromArgb(40, 80, 120);
            grid.DefaultCellStyle.SelectionForeColor   = clrTexto;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(30, 34, 46);
            grid.ColumnHeadersDefaultCellStyle.BackColor   = clrGrupo;
            grid.ColumnHeadersDefaultCellStyle.ForeColor   = clrAcento;
            grid.ColumnHeadersDefaultCellStyle.Font        = fntLabelBold;
            grid.ColumnHeadersHeight                        = 32;
            grid.EnableHeadersVisualStyles                  = false;
            grid.SelectionChanged += grid_SelectionChanged;

            lblTotal = new Label {
                Dock      = DockStyle.Bottom,
                Height    = 24,
                TextAlign = ContentAlignment.MiddleRight,
                Font      = fntLabel,
                ForeColor = clrSubtexto,
                BackColor = clrPanel,
                Padding   = new Padding(0, 0, 8, 0),
            };

            pnlLeft.Controls.Add(grid);
            pnlLeft.Controls.Add(lblTotal);
            pnlLeft.Controls.Add(pnlBusqueda);

            // ── Panel derecho: formulario de edición ─────────────────────────
            pnlBottom = new Panel {
                Dock      = DockStyle.Right,
                Width     = 280,
                BackColor = clrPanel,
                Padding   = new Padding(16),
                AutoScroll = true,
            };

            grpEdicion = new GroupBox {
                Text      = "Edición de partida",
                Dock      = DockStyle.Top,
                AutoSize  = true,
                ForeColor = clrAcento,
                Font      = fntLabelBold,
                BackColor = clrPanel,
                Padding   = new Padding(12),
            };

            var tlpEdicion = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 7,
            };
            tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            tlpEdicion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            grpEdicion.Controls.Add(tlpEdicion);

            // Campos del formulario
            lblPartida = HistoricoLabel(null, "Partida nº", 0, Point.Empty, clrSubtexto);
            tlpEdicion.Controls.Add(lblPartida, 0, 0);
            nudPartida = new NumericUpDown {
                Minimum = 1, Maximum = 9999, Value = 1,
                BackColor = clrGrupo, ForeColor = clrTexto, BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
            };
            tlpEdicion.Controls.Add(nudPartida, 1, 0);

            lblPrecio = HistoricoLabel(null, "Precio €", 0, Point.Empty, clrSubtexto);
            tlpEdicion.Controls.Add(lblPrecio, 0, 1);
            nudPrecio = new NumericUpDown {
                DecimalPlaces = 2, Minimum = 0, Maximum = 9999, Value = 0,
                BackColor = clrGrupo, ForeColor = clrTexto, BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
            };
            nudPrecio.ValueChanged += nudPrecio_ValueChanged;
            tlpEdicion.Controls.Add(nudPrecio, 1, 1);

            lblVendidos = HistoricoLabel(null, "Vendidos", 0, Point.Empty, clrSubtexto);
            tlpEdicion.Controls.Add(lblVendidos, 0, 2);
            nudVendidos = new NumericUpDown {
                Minimum = 0, Maximum = 99999, Value = 0,
                BackColor = clrGrupo, ForeColor = clrTexto, BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
            };
            nudVendidos.ValueChanged += nudVendidos_ValueChanged;
            tlpEdicion.Controls.Add(nudVendidos, 1, 2);

            lblLinea = HistoricoLabel(null, "Premio línea €", 0, Point.Empty, clrSubtexto);
            tlpEdicion.Controls.Add(lblLinea, 0, 3);
            nudLinea = new NumericUpDown {
                DecimalPlaces = 2, Minimum = 0, Maximum = 999999, Value = 0,
                BackColor = clrGrupo, ForeColor = clrTexto, BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
            };
            tlpEdicion.Controls.Add(nudLinea, 1, 3);

            lblBingo = HistoricoLabel(null, "Premio bingo €", 0, Point.Empty, clrSubtexto);
            tlpEdicion.Controls.Add(lblBingo, 0, 4);
            nudBingo = new NumericUpDown {
                DecimalPlaces = 2, Minimum = 0, Maximum = 999999, Value = 0,
                BackColor = clrGrupo, ForeColor = clrTexto, BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
            };
            tlpEdicion.Controls.Add(nudBingo, 1, 4);

            // Preview venta
            lblVentaPreview = new Label {
                Text      = "Venta: 0.00 €",
                AutoSize  = true,
                Font      = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = clrVerde,
                Anchor    = AnchorStyles.Left,
                Margin    = new Padding(0, 10, 0, 0)
            };
            tlpEdicion.Controls.Add(lblVentaPreview, 1, 5);

            // Nota: campos PK auto-rellenados
            var lblNota = new Label {
                Text      = $"Sala/Conc/Fecha: auto",
                AutoSize  = true,
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                ForeColor = clrSubtexto,
                Anchor    = AnchorStyles.Left,
                Margin    = new Padding(0, 10, 0, 0)
            };
            tlpEdicion.Controls.Add(lblNota, 0, 6);
            tlpEdicion.SetColumnSpan(lblNota, 2);

            // Botones de acción
            btnGuardar  = CrearBoton("💾  Guardar",  clrVerde,   clrFondo, Point.Empty, new Size(228, 36));
            btnNueva    = CrearBoton("＋  Nueva",    clrAcento,  clrFondo, Point.Empty, new Size(228, 36));
            btnEliminar = CrearBoton("✕  Eliminar", clrRojo,    clrFondo, Point.Empty, new Size(228, 36));
            btnGuardar.Dock  = DockStyle.Top;
            btnNueva.Dock    = DockStyle.Top;
            btnEliminar.Dock = DockStyle.Top;
            btnGuardar.Click  += btnGuardar_Click;
            btnNueva.Click    += btnNueva_Click;
            btnEliminar.Click += btnEliminar_Click;

            var pnlBotones = new Panel {
                Dock      = DockStyle.Top,
                Height    = 120,
                BackColor = clrPanel,
                Padding   = new Padding(0, 8, 0, 0),
            };
            // apilados de abajo a arriba (DockStyle.Top invierte)
            pnlBotones.Controls.Add(btnEliminar);
            pnlBotones.Controls.Add(btnNueva);
            pnlBotones.Controls.Add(btnGuardar);

            pnlBottom.Controls.Add(pnlBotones);
            pnlBottom.Controls.Add(grpEdicion);

            // ── Ensamblar ─────────────────────────────────────────────────────
            Controls.Add(pnlLeft);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);

            ResumeLayout(false);
            PerformLayout();
        }

        // ── Helpers de construcción de UI ──────────────────────────────────────

        private static Button CrearBoton(string texto, Color fondo, Color texto2,
                                          Point loc, Size size) {
            return new Button {
                Text      = texto,
                Location  = loc,
                Size      = size,
                BackColor = fondo,
                ForeColor = texto2,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor    = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 },
            };
        }

        private static Label HistoricoLabel(Control parent, string texto, int ancho,
                                             Point loc, Color color) {
            var lbl = new Label {
                Text      = texto,
                AutoSize  = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Font      = new Font("Segoe UI", 9.5f),
                ForeColor = color,
                Anchor    = AnchorStyles.Left,
            };
            if (parent != null) parent.Controls.Add(lbl);
            return lbl;
        }
    }
}
