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
            var fntTitulo      = new Font("Segoe UI", 14f, FontStyle.Bold);
            var fntLabel       = new Font("Segoe UI", 9f);
            var fntLabelBold   = new Font("Segoe UI", 9f, FontStyle.Bold);
            var fntMono        = new Font("Consolas", 10f);
            var fntGrid        = new Font("Segoe UI", 9f);

            SuspendLayout();

            // ── Formulario ────────────────────────────────────────────────────
            Text            = "Histórico Bingo — Editor Manual";
            Size            = new Size(900, 640);
            MinimumSize     = new Size(820, 560);
            BackColor       = clrFondo;
            ForeColor       = clrTexto;
            Font            = fntLabel;
            StartPosition   = FormStartPosition.CenterScreen;

            // ── Panel superior ────────────────────────────────────────────────
            pnlTop = new Panel {
                Dock      = DockStyle.Top,
                Height    = 64,
                BackColor = clrPanel,
                Padding   = new Padding(16, 0, 16, 0),
            };

            lblTitulo = new Label {
                Text      = "HISTÓRICO",
                Font      = fntTitulo,
                ForeColor = clrAcento,
                AutoSize  = true,
                Location  = new Point(16, 18),
            };

            lblSalaConc = new Label {
                Text      = "Sala: —  |  Conc: —",
                Font      = fntLabel,
                ForeColor = clrSubtexto,
                AutoSize  = true,
                Location  = new Point(156, 23),
            };

            lblEstado = new Label {
                Text      = "Conectando…",
                Font      = fntLabel,
                ForeColor = clrSubtexto,
                AutoSize  = true,
                Anchor    = AnchorStyles.Top | AnchorStyles.Right,
                Location  = new Point(650, 23),
            };

            pnlTop.Controls.AddRange(new Control[] { lblTitulo, lblSalaConc, lblEstado });

            // ── Panel izquierdo: búsqueda + grid ──────────────────────────────
            pnlLeft = new Panel {
                Dock      = DockStyle.Fill,
                Padding   = new Padding(12),
                BackColor = clrFondo,
            };

            // Barra de búsqueda por fecha
            var pnlBusqueda = new Panel {
                Height    = 38,
                Dock      = DockStyle.Top,
                BackColor = clrFondo,
            };

            lblFecha = new Label {
                Text     = "Fecha sesión:",
                Font     = fntLabel,
                ForeColor= clrSubtexto,
                AutoSize = true,
                Location = new Point(0, 10),
            };

            dtpFecha = new DateTimePicker {
                Format     = DateTimePickerFormat.Short,
                Width      = 110,
                Location   = new Point(100, 6),
                BackColor  = clrGrupo,
                ForeColor  = clrTexto,
                CalendarMonthBackground = clrPanel,
            };

            btnBuscar = CrearBoton("Buscar", clrAcento, clrFondo, new Point(220, 4), new Size(80, 28));
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
                Width     = 260,
                BackColor = clrPanel,
                Padding   = new Padding(16),
            };

            grpEdicion = new GroupBox {
                Text      = "Edición de partida",
                Dock      = DockStyle.Top,
                Height    = 340,
                ForeColor = clrAcento,
                Font      = fntLabelBold,
                BackColor = clrPanel,
                Padding   = new Padding(10),
            };

            // Campos del formulario
            int y = 22;
            int lblW = 80; int ctrlX = 96; int ctrlW = 120;

            HistoricoLabel(grpEdicion, "Partida nº",    lblW, new Point(10, y), clrSubtexto);
            nudPartida = new NumericUpDown {
                Location = new Point(ctrlX, y - 3), Width = ctrlW,
                Minimum = 1, Maximum = 9999, Value = 1,
                BackColor = clrGrupo, ForeColor = clrTexto, BorderStyle = BorderStyle.FixedSingle,
            };
            grpEdicion.Controls.Add(nudPartida);
            y += 34;

            HistoricoLabel(grpEdicion, "Precio €",      lblW, new Point(10, y), clrSubtexto);
            nudPrecio = new NumericUpDown {
                Location = new Point(ctrlX, y - 3), Width = ctrlW,
                DecimalPlaces = 2, Minimum = 0, Maximum = 9999, Value = 0,
                BackColor = clrGrupo, ForeColor = clrTexto, BorderStyle = BorderStyle.FixedSingle,
            };
            nudPrecio.ValueChanged += nudPrecio_ValueChanged;
            grpEdicion.Controls.Add(nudPrecio);
            y += 34;

            HistoricoLabel(grpEdicion, "Vendidos",      lblW, new Point(10, y), clrSubtexto);
            nudVendidos = new NumericUpDown {
                Location = new Point(ctrlX, y - 3), Width = ctrlW,
                Minimum = 0, Maximum = 99999, Value = 0,
                BackColor = clrGrupo, ForeColor = clrTexto, BorderStyle = BorderStyle.FixedSingle,
            };
            nudVendidos.ValueChanged += nudVendidos_ValueChanged;
            grpEdicion.Controls.Add(nudVendidos);
            y += 34;

            HistoricoLabel(grpEdicion, "Premio línea €",lblW, new Point(10, y), clrSubtexto);
            nudLinea = new NumericUpDown {
                Location = new Point(ctrlX, y - 3), Width = ctrlW,
                DecimalPlaces = 2, Minimum = 0, Maximum = 999999, Value = 0,
                BackColor = clrGrupo, ForeColor = clrTexto, BorderStyle = BorderStyle.FixedSingle,
            };
            grpEdicion.Controls.Add(nudLinea);
            y += 34;

            HistoricoLabel(grpEdicion, "Premio bingo €",lblW, new Point(10, y), clrSubtexto);
            nudBingo = new NumericUpDown {
                Location = new Point(ctrlX, y - 3), Width = ctrlW,
                DecimalPlaces = 2, Minimum = 0, Maximum = 999999, Value = 0,
                BackColor = clrGrupo, ForeColor = clrTexto, BorderStyle = BorderStyle.FixedSingle,
            };
            grpEdicion.Controls.Add(nudBingo);
            y += 34;

            // Preview venta
            lblVentaPreview = new Label {
                Text      = "Venta: 0.00 €",
                Location  = new Point(ctrlX, y),
                AutoSize  = true,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = clrVerde,
            };
            grpEdicion.Controls.Add(lblVentaPreview);
            y += 30;

            // Nota: campos PK auto-rellenados
            var lblNota = new Label {
                Text      = $"Sala/Conc/Fecha: auto",
                Location  = new Point(10, y + 4),
                AutoSize  = true,
                Font      = new Font("Segoe UI", 7.5f, FontStyle.Italic),
                ForeColor = clrSubtexto,
            };
            grpEdicion.Controls.Add(lblNota);

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
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor    = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 },
            };
        }

        private static Label HistoricoLabel(Control parent, string texto, int ancho,
                                             Point loc, Color color) {
            var lbl = new Label {
                Text      = texto,
                Location  = loc,
                Width     = ancho,
                AutoSize  = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = color,
            };
            parent.Controls.Add(lbl);
            return lbl;
        }
    }
}
