using BingoHistoricoEditor.Data;

namespace BingoHistoricoEditor {
    /// <summary>
    /// Muestra el contenido de la tabla de auditoría (operacionesBGEditor).
    /// Solo debe abrirse para usuarios administradores (ver MainForm.btnAuditoria_Click).
    /// </summary>
    public class AuditoriaForm : Form {
        private const int MaxOperaciones = 500;

        private readonly FirebirdService _db;
        private DataGridView _grid = null!;
        private Button _btnRefrescar = null!;
        private Button _btnCerrar = null!;
        private Label _lblInfo = null!;

        public AuditoriaForm(FirebirdService db) {
            _db = db;
            BuildUI();
            Load += async (_, _) => await CargarAsync();
        }

        private void BuildUI() {
            Text = "Auditoría — operacionesBGEditor";
            Size = new Size(900, 520);
            MinimumSize = new Size(600, 320);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = SystemColors.Control;
            ForeColor = SystemColors.ControlText;
            Font = new Font("Segoe UI", 9f);

            _grid = new DataGridView {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Window,
                BorderStyle = BorderStyle.FixedSingle,
            };

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 46 };

            _btnRefrescar = new Button {
                Text = "🔄 Refrescar", Size = new Size(120, 32), Location = new Point(8, 7),
                FlatStyle = FlatStyle.Standard, ForeColor = SystemColors.ControlText,
                Cursor = Cursors.Hand, UseVisualStyleBackColor = true,
            };
            _btnRefrescar.Click += async (_, _) => await CargarAsync();

            _btnCerrar = new Button {
                Text = "Cerrar", Size = new Size(100, 32), Location = new Point(136, 7),
                FlatStyle = FlatStyle.Standard, ForeColor = SystemColors.ControlText,
                Cursor = Cursors.Hand, UseVisualStyleBackColor = true,
            };
            _btnCerrar.Click += (_, _) => Close();

            _lblInfo = new Label {
                AutoSize = true, ForeColor = SystemColors.GrayText,
                Location = new Point(254, 16),
            };

            pnlBottom.Controls.AddRange(new Control[] { _btnRefrescar, _btnCerrar, _lblInfo });

            Controls.Add(_grid);
            Controls.Add(pnlBottom);
        }

        private async Task CargarAsync() {
            try {
                Cursor = Cursors.WaitCursor;
                var operaciones = await _db.ObtenerOperacionesAsync(ultimasN: MaxOperaciones);

                _grid.DataSource = null;
                _grid.DataSource = operaciones;

                _lblInfo.Text = $"{operaciones.Count} operaciones (máx. {MaxOperaciones} más recientes) — actualizado {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al cargar el registro de auditoría:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                Cursor = Cursors.Default;
            }
        }
    }
}
