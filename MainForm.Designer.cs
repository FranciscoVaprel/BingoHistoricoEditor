namespace BingoHistoricoEditor {
    partial class MainForm {
        private System.ComponentModel.IContainer components = null;

        // Estructura de Paneles
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TableLayoutPanel tlpMainLayout;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private System.Windows.Forms.Panel pnlRight;

        // Controles Superiores
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSalaConc;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Button btnBuscar;

        // Controles de Datos y Log
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Panel pnlGridContainer;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.Panel pnlLogHeader;
        private System.Windows.Forms.Label lblLogTitulo;
        private System.Windows.Forms.Button btnLimpiarLog;
        private System.Windows.Forms.TextBox txtLog;

        // Controles de Edición
        private System.Windows.Forms.GroupBox grpEdicion;
        private System.Windows.Forms.TableLayoutPanel tlpEdicion;
        private System.Windows.Forms.NumericUpDown nudPartida;
        private System.Windows.Forms.NumericUpDown nudPrecio;
        private System.Windows.Forms.NumericUpDown nudVendidos;
        private System.Windows.Forms.NumericUpDown nudLinea;
        private System.Windows.Forms.NumericUpDown nudBingo;
        private System.Windows.Forms.Panel pnlBotones;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnNueva;
        private System.Windows.Forms.Button btnEliminar;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();

            this.InitFormProperties();

            this.pnlTop = new System.Windows.Forms.Panel();
            this.tlpMainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.pnlRight = new System.Windows.Forms.Panel();

            // Construcción por bloques
            this.BuildTopPanel();
            this.BuildContentArea();
            this.BuildRightPanel();

            // Configuración del Layout Principal
            this.tlpMainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMainLayout.ColumnCount = 2;
            this.tlpMainLayout.RowCount = 1;
            this.tlpMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tlpMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tlpMainLayout.Controls.Add(this.tlpContent, 0, 0);
            this.tlpMainLayout.Controls.Add(this.pnlRight, 1, 0);

            this.Controls.Add(this.tlpMainLayout);
            this.Controls.Add(this.pnlTop);

            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.tlpContent.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.grpEdicion.ResumeLayout(false);
            this.tlpEdicion.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void InitFormProperties() {
            this.Text = "Editor de Histórico - Bingo";
            this.Size = new System.Drawing.Size(1250, 850);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 35);
        }

        private void BuildTopPanel() {
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 70;
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(20, 20, 25);
            this.pnlTop.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);

            this.lblTitulo = new System.Windows.Forms.Label {
                Text = "HISTÓRICO DE VENTAS",
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 12),
                AutoSize = true
            };

            this.lblSalaConc = new System.Windows.Forms.Label {
                ForeColor = System.Drawing.Color.DarkGray,
                Location = new System.Drawing.Point(22, 42),
                AutoSize = true
            };

            this.dtpFecha = new System.Windows.Forms.DateTimePicker {
                Location = new System.Drawing.Point(380, 22),
                Width = 120,
                Format = System.Windows.Forms.DateTimePickerFormat.Short
            };

            this.btnBuscar = new System.Windows.Forms.Button {
                Text = "BUSCAR",
                Location = new System.Drawing.Point(510, 20),
                Size = new System.Drawing.Size(100, 32),
                BackColor = System.Drawing.Color.FromArgb(0, 120, 215),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat
            };
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);

            this.lblEstado = new System.Windows.Forms.Label {
                Text = "Verificando...",
                ForeColor = System.Drawing.Color.Gray,
                Dock = System.Windows.Forms.DockStyle.Right,
                TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                Width = 250
            };

            this.pnlTop.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblTitulo, this.lblSalaConc, this.dtpFecha, this.btnBuscar, this.lblEstado
            });
        }

        private void BuildContentArea() {
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.ColumnCount = 1;
            this.tlpContent.RowCount = 2;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpContent.Padding = new System.Windows.Forms.Padding(10);

            this.pnlGridContainer = new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Fill, Padding = new System.Windows.Forms.Padding(0, 0, 0, 10) };
            this.grid = new System.Windows.Forms.DataGridView {
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackgroundColor = System.Drawing.Color.FromArgb(45, 45, 50),
                BorderStyle = System.Windows.Forms.BorderStyle.None
            };
            this.grid.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
            this.pnlGridContainer.Controls.Add(this.grid);

            this.pnlLog = new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Fill, BackColor = System.Drawing.Color.Black, BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle };
            this.pnlLogHeader = new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Top, Height = 25, BackColor = System.Drawing.Color.FromArgb(50, 50, 55) };
            this.lblLogTitulo = new System.Windows.Forms.Label { Text = "  CONSOLA DE EVENTOS", ForeColor = System.Drawing.Color.White, Dock = System.Windows.Forms.DockStyle.Left, TextAlign = System.Drawing.ContentAlignment.MiddleLeft, Font = new System.Drawing.Font("Segoe UI", 8F) };
            this.btnLimpiarLog = new System.Windows.Forms.Button { Text = "Limpiar", Dock = System.Windows.Forms.DockStyle.Right, Width = 60, FlatStyle = System.Windows.Forms.FlatStyle.Flat, ForeColor = System.Drawing.Color.Gray };
            this.btnLimpiarLog.Click += (s, e) => txtLog.Clear();

            this.txtLog = new System.Windows.Forms.TextBox {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                BackColor = System.Drawing.Color.Black,
                ForeColor = System.Drawing.Color.Lime,
                Font = new System.Drawing.Font("Consolas", 9F),
                ScrollBars = System.Windows.Forms.ScrollBars.Vertical,
                BorderStyle = System.Windows.Forms.BorderStyle.None
            };

            this.pnlLogHeader.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblLogTitulo, this.btnLimpiarLog });
            this.pnlLog.Controls.AddRange(new System.Windows.Forms.Control[] { this.txtLog, this.pnlLogHeader });

            this.tlpContent.Controls.Add(this.pnlGridContainer, 0, 0);
            this.tlpContent.Controls.Add(this.pnlLog, 0, 1);
        }

        private void BuildRightPanel() {
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Padding = new System.Windows.Forms.Padding(0, 10, 15, 10);

            this.grpEdicion = new System.Windows.Forms.GroupBox {
                Text = " DATOS DE PARTIDA ",
                ForeColor = System.Drawing.Color.LightSkyBlue,
                Dock = System.Windows.Forms.DockStyle.Top,
                Height = 300,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
            };

            this.tlpEdicion = new System.Windows.Forms.TableLayoutPanel { Dock = System.Windows.Forms.DockStyle.Fill, ColumnCount = 2, RowCount = 5, Padding = new System.Windows.Forms.Padding(10) };
            this.nudPartida = new System.Windows.Forms.NumericUpDown { Dock = System.Windows.Forms.DockStyle.Fill };
            this.nudPrecio = new System.Windows.Forms.NumericUpDown { Dock = System.Windows.Forms.DockStyle.Fill, DecimalPlaces = 2 };
            this.nudVendidos = new System.Windows.Forms.NumericUpDown { Dock = System.Windows.Forms.DockStyle.Fill };
            this.nudLinea = new System.Windows.Forms.NumericUpDown { Dock = System.Windows.Forms.DockStyle.Fill, DecimalPlaces = 2 };
            this.nudBingo = new System.Windows.Forms.NumericUpDown { Dock = System.Windows.Forms.DockStyle.Fill, DecimalPlaces = 2 };

            this.AddEditRow("Nº Partida:", this.nudPartida, 0);
            this.AddEditRow("Precio Cartón:", this.nudPrecio, 1);
            this.AddEditRow("Cartones Vend.:", this.nudVendidos, 2);
            this.AddEditRow("Importe Línea:", this.nudLinea, 3);
            this.AddEditRow("Importe Bingo:", this.nudBingo, 4);

            this.grpEdicion.Controls.Add(this.tlpEdicion);

            this.pnlBotones = new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Bottom, Height = 160 };
            this.btnGuardar = new System.Windows.Forms.Button { Text = "GUARDAR CAMBIOS", Dock = System.Windows.Forms.DockStyle.Top, Height = 50, BackColor = System.Drawing.Color.FromArgb(40, 167, 69), ForeColor = System.Drawing.Color.White, FlatStyle = System.Windows.Forms.FlatStyle.Flat, Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold) };
            this.btnNueva = new System.Windows.Forms.Button { Text = "NUEVA PARTIDA", Dock = System.Windows.Forms.DockStyle.Top, Height = 40, BackColor = System.Drawing.Color.FromArgb(70, 70, 75), ForeColor = System.Drawing.Color.White, FlatStyle = System.Windows.Forms.FlatStyle.Flat };
            this.btnEliminar = new System.Windows.Forms.Button { Text = "ELIMINAR SELECCIÓN", Dock = System.Windows.Forms.DockStyle.Bottom, Height = 40, BackColor = System.Drawing.Color.FromArgb(220, 53, 69), ForeColor = System.Drawing.Color.White, FlatStyle = System.Windows.Forms.FlatStyle.Flat };

            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            this.btnNueva.Click += new System.EventHandler(this.btnNueva_Click);
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            this.pnlBotones.Controls.AddRange(new System.Windows.Forms.Control[] { this.btnNueva, this.btnGuardar, this.btnEliminar });
            this.pnlRight.Controls.AddRange(new System.Windows.Forms.Control[] { this.grpEdicion, this.pnlBotones });
        }

        private void AddEditRow(string text, System.Windows.Forms.Control ctrl, int row) {
            var lbl = new System.Windows.Forms.Label { Text = text, ForeColor = System.Drawing.Color.White, TextAlign = System.Drawing.ContentAlignment.MiddleRight, Dock = System.Windows.Forms.DockStyle.Fill };
            this.tlpEdicion.Controls.Add(lbl, 0, row);
            this.tlpEdicion.Controls.Add(ctrl, 1, row);
        }
    }
}