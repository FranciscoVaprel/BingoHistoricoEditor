namespace BingoHistoricoEditor {
    partial class MainForm {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 35);
            this.Text = "Bingo Histórico - Editor de Partidas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            // 1. PANEL SUPERIOR (Cabecera)
            this.pnlTop = new System.Windows.Forms.Panel {
                Dock = System.Windows.Forms.DockStyle.Top,
                Height = 75,
                BackColor = System.Drawing.Color.FromArgb(45, 45, 50),
                Padding = new System.Windows.Forms.Padding(20, 10, 20, 10)
            };

            this.lblTitulo = new System.Windows.Forms.Label {
                Text = "HISTÓRICO DE PARTIDAS",
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(20, 12)
            };

            this.lblSalaConc = new System.Windows.Forms.Label {
                Text = "SALA: -- | CONCESIONARIO: --",
                ForeColor = System.Drawing.Color.LightSkyBlue,
                Font = new System.Drawing.Font("Segoe UI", 10F),
                AutoSize = true,
                Location = new System.Drawing.Point(22, 42)
            };

            this.dtpFecha = new System.Windows.Forms.DateTimePicker {
                Format = System.Windows.Forms.DateTimePickerFormat.Short,
                Width = 120,
                Location = new System.Drawing.Point(400, 25),
                Font = new System.Drawing.Font("Segoe UI", 11F)
            };

            this.btnBuscar = new System.Windows.Forms.Button {
                Text = "BUSCAR",
                Width = 100,
                Height = 32,
                Location = new System.Drawing.Point(530, 24),
                BackColor = System.Drawing.Color.FromArgb(100, 100, 110),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat
            };
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);

            this.lblEstado = new System.Windows.Forms.Label {
                Text = "Conectando...",
                ForeColor = System.Drawing.Color.Gray,
                Dock = System.Windows.Forms.DockStyle.Right,
                TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                Width = 250,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic)
            };

            this.pnlTop.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblTitulo, this.lblSalaConc, this.dtpFecha, this.btnBuscar, this.lblEstado
            });

            // 2. CONTENEDOR PRINCIPAL
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel {
                Dock = System.Windows.Forms.DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = System.Drawing.Color.Transparent
            };
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));

            // 3. LADO IZQUIERDO (Grid y Log)
            System.Windows.Forms.Panel pnlLeft = new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Fill };

            this.pnlGridContainer = new System.Windows.Forms.Panel {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Padding = new System.Windows.Forms.Padding(20, 20, 10, 10)
            };
            this.grid = new System.Windows.Forms.DataGridView {
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackgroundColor = System.Drawing.Color.FromArgb(40, 40, 45),
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                EnableHeadersVisualStyles = false,
                SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true
            };
            this.pnlGridContainer.Controls.Add(this.grid);

            this.pnlLog = new System.Windows.Forms.Panel {
                Dock = System.Windows.Forms.DockStyle.Bottom,
                Height = 180,
                Padding = new System.Windows.Forms.Padding(20, 0, 10, 20)
            };
            this.txtLog = new System.Windows.Forms.TextBox {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                BackColor = System.Drawing.Color.Black,
                ForeColor = System.Drawing.Color.Lime,
                Font = new System.Drawing.Font("Consolas", 9.5F),
                ScrollBars = System.Windows.Forms.ScrollBars.Vertical,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            };
            this.pnlLog.Controls.Add(this.txtLog);

            pnlLeft.Controls.Add(this.pnlGridContainer);
            pnlLeft.Controls.Add(this.pnlLog);

            // 4. LADO DERECHO (Edición)
            this.pnlRight = new System.Windows.Forms.Panel {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Padding = new System.Windows.Forms.Padding(10, 20, 20, 20)
            };
            BuildRightPanel();

            // 5. ENSAMBLAJE FINAL
            this.tlpContent.Controls.Add(pnlLeft, 0, 0);
            this.tlpContent.Controls.Add(this.pnlRight, 1, 0);

            this.Controls.Add(this.tlpContent);
            this.Controls.Add(this.pnlTop);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void BuildRightPanel() {
            this.grpEdicion = new System.Windows.Forms.GroupBox {
                Text = " EDICIÓN DE PARTIDA ",
                ForeColor = System.Drawing.Color.White,
                Dock = System.Windows.Forms.DockStyle.Top,
                Height = 350,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
            };

            this.tlpEdicion = new System.Windows.Forms.TableLayoutPanel {
                Dock = System.Windows.Forms.DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new System.Windows.Forms.Padding(10, 10, 10, 10)
            };

            this.tlpEdicion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpEdicion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));

            for (int i = 0; i < 5; i++) {
                this.tlpEdicion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            }

            // CORRECCIÓN: Usamos Anchor en lugar de Dock para centrar verticalmente
            var anchorStyle = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            this.nudPartida = new System.Windows.Forms.NumericUpDown { Anchor = anchorStyle };
            this.nudPrecio = new System.Windows.Forms.NumericUpDown { Anchor = anchorStyle, DecimalPlaces = 2 };
            this.nudVendidos = new System.Windows.Forms.NumericUpDown { Anchor = anchorStyle };
            this.nudLinea = new System.Windows.Forms.NumericUpDown { Anchor = anchorStyle, DecimalPlaces = 2 };
            this.nudBingo = new System.Windows.Forms.NumericUpDown { Anchor = anchorStyle, DecimalPlaces = 2 };

            this.AddEditRow("Nº Partida:", this.nudPartida, 0);
            this.AddEditRow("Precio Cartón:", this.nudPrecio, 1);
            this.AddEditRow("Cartones Vend.:", this.nudVendidos, 2);
            this.AddEditRow("Importe Línea:", this.nudLinea, 3);
            this.AddEditRow("Importe Bingo:", this.nudBingo, 4);

            this.pnlBotones = new System.Windows.Forms.Panel {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Padding = new System.Windows.Forms.Padding(0, 15, 0, 0)
            };

            this.btnGuardar = new System.Windows.Forms.Button {
                Text = "GUARDAR CAMBIOS",
                Dock = System.Windows.Forms.DockStyle.Top,
                Height = 55,
                BackColor = System.Drawing.Color.FromArgb(40, 167, 69),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold)
            };

            this.btnNueva = new System.Windows.Forms.Button {
                Text = "NUEVA PARTIDA",
                Dock = System.Windows.Forms.DockStyle.Top,
                Height = 45,
                BackColor = System.Drawing.Color.FromArgb(50, 50, 55),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Margin = new System.Windows.Forms.Padding(0, 0, 0, 10)
            };

            this.btnEliminar = new System.Windows.Forms.Button {
                Text = "ELIMINAR SELECCIONADA",
                Dock = System.Windows.Forms.DockStyle.Bottom,
                Height = 40,
                BackColor = System.Drawing.Color.FromArgb(220, 53, 69),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat
            };

            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            this.btnNueva.Click += new System.EventHandler(this.btnNueva_Click);
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            this.pnlBotones.Controls.Add(this.btnGuardar);
            this.pnlBotones.Controls.Add(this.btnNueva);
            this.pnlBotones.Controls.Add(this.btnEliminar);

            this.grpEdicion.Controls.Add(this.tlpEdicion);
            this.pnlRight.Controls.Add(this.pnlBotones);
            this.pnlRight.Controls.Add(this.grpEdicion);
        }

        private void AddEditRow(string text, System.Windows.Forms.Control ctrl, int row) {
            // El Label se mantiene en Dock = Fill pero con TextAlign centrado y a la derecha
            var lbl = new System.Windows.Forms.Label {
                Text = text,
                ForeColor = System.Drawing.Color.White,
                Dock = System.Windows.Forms.DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleRight, // Alineado derecha
                Padding = new System.Windows.Forms.Padding(0, 0, 10, 0), // Separación del input
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            this.tlpEdicion.Controls.Add(lbl, 0, row);
            this.tlpEdicion.Controls.Add(ctrl, 1, row);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSalaConc;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Panel pnlGridContainer;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.TextBox txtLog;
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
    }
}