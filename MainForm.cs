using BingoHistoricoEditor.Data;
using BingoHistoricoEditor.Models;
using BingoHistoricoEditor.Services;
using Microsoft.Extensions.Configuration;

namespace BingoHistoricoEditor {
    public partial class MainForm : Form {
        private readonly FirebirdService _db;
        private readonly int _codSala;
        private readonly int _codConc;
        private List<HistoricoEntry> _partidas = new();
        private HistoricoEntry? _seleccionada;

        public MainForm(FirebirdService db, IConfiguration config) {
            InitializeComponent();    // siempre primero
            _db = db;
            _codSala = GetConfigInt(config, "BingoSettings:CodSala", 1);
            _codConc = GetConfigInt(config, "BingoSettings:CodConc", 1);
            ConfigurarGrid();         // despues de InitializeComponent: grid ya existe
        }

        protected override async void OnLoad(EventArgs e) {
            base.OnLoad(e);
            dtpFecha.Value = DateTime.Today;
            lblSalaConc.Text = $"Sala: {_codSala}  |  Conc: {_codConc}";

            bool ok = await _db.TestConnectionAsync();
            lblEstado.Text = ok ? "Conectado a Firebird" : "Sin conexion";
            lblEstado.ForeColor = ok ? Color.FromArgb(80, 200, 120) : Color.FromArgb(220, 80, 80);

            if (ok) await CargarPartidasAsync();

        }

        private void ConfigurarGrid() {
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartidaDia", HeaderText = "Partida", Width = 65 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Precio", HeaderText = "Precio", Width = 80, DefaultCellStyle = { Format = "F2" } });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Vendidos", HeaderText = "Vendidos", Width = 80 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Venta", HeaderText = "Venta", Width = 90, DefaultCellStyle = { Format = "F2" } });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Premio1", HeaderText = "Linea", Width = 80, DefaultCellStyle = { Format = "F2" } });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Premio2", HeaderText = "Bingo", Width = 80, DefaultCellStyle = { Format = "F2" } });
        }

        private async Task CargarPartidasAsync() {
            try {
                SetBusy(true);
                _partidas = await _db.CargarPorFechaAsync(dtpFecha.Value, _codSala, _codConc);
                grid.DataSource = null;
                grid.DataSource = _partidas;
                btnVerificarHuecos.Enabled = _partidas.Count > 1;
                lblTotal.Text = $"{_partidas.Count} partidas | Venta: {_partidas.Sum(p => p.Venta):F2}";
                LimpiarFormulario();
            }
            catch (Exception ex) {
                LogService.Error("Error al cargar partidas", ex);
                MostrarError($"No se pudieron cargar los datos:\n{ex.Message}");
            }
            finally {
                SetBusy(false);
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e) {
            pnlResultados.Visible = false; // Ocultar resultados anteriores al iniciar nueva búsqueda
            await CargarPartidasAsync();
        }

        private void grid_SelectionChanged(object sender, EventArgs e) {
            if (grid.CurrentRow?.DataBoundItem is HistoricoEntry entrada) {
                _seleccionada = entrada;
                CargarEnFormulario(entrada);
            }
        }

        private void btnNueva_Click(object sender, EventArgs e) {
            _seleccionada = null;
            LimpiarFormulario();
            nudPartida.Value = _partidas.Count > 0 ? _partidas.Max(p => p.PartidaDia) + 1 : 1;
            nudPartida.Focus();
        }

        private async void btnGuardar_Click(object sender, EventArgs e) {
            if (!ValidarFormulario()) return;

            var entrada = new HistoricoEntry {
                CodSala = _codSala,
                CodConc = _codConc,
                FechaSesion = dtpFecha.Value.Date,
                PartidaDia = (int)nudPartida.Value,
                Precio = nudPrecio.Value,
                Vendidos = (int)nudVendidos.Value,
                Premio1 = nudLinea.Value,
                Premio2 = nudBingo.Value,
            };

            try {
                SetBusy(true);
                await _db.GuardarAsync(entrada);
                LogService.Write("Partida guardada", new {
                    entrada.PartidaDia,
                    entrada.FechaSesion,
                    VentaCalculada = entrada.Precio * entrada.Vendidos
                });
                await CargarPartidasAsync();
                MostrarInfo($"Partida {entrada.PartidaDia} guardada correctamente.");
            }
            catch (Exception ex) {
                LogService.Error($"Error al guardar partida {entrada.PartidaDia}", ex);
                MostrarError($"No se pudo guardar la partida:\n{ex.Message}");
            }
            finally {
                SetBusy(false);
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e) {
            if (_seleccionada is null) {
                MostrarAviso("Selecciona una partida en la tabla para eliminarla.");
                return;
            }
            var confirm = MessageBox.Show(
                $"Eliminar la partida {_seleccionada.PartidaDia} del {_seleccionada.FechaSesion:dd/MM/yyyy}?",
                "Confirmar eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try {
                SetBusy(true);
                await _db.EliminarAsync(_seleccionada);
                LogService.Write("Partida eliminada", new { _seleccionada.PartidaDia, _seleccionada.FechaSesion });
                await CargarPartidasAsync();
                MostrarInfo("Partida eliminada.");
            }
            catch (Exception ex) {
                LogService.Error($"Error al eliminar partida {_seleccionada.PartidaDia}", ex);
                MostrarError(ex.Message);
            }
            finally {
                SetBusy(false);
            }
        }

        private void nudPrecio_ValueChanged(object sender, EventArgs e) => ActualizarVentaPreview();
        private void nudVendidos_ValueChanged(object sender, EventArgs e) => ActualizarVentaPreview();
        private void ActualizarVentaPreview() =>
            lblVentaPreview.Text = $"Venta: {nudPrecio.Value * nudVendidos.Value:F2}";

        private void btnVerificarHuecos_Click(object sender, EventArgs e) {
            if (_partidas.Count < 2) {
                txtResultados.Text = "No hay suficientes partidas para comparar.";
                pnlResultados.Visible = true;
                return;
            }

            var ordenadas = _partidas.OrderBy(p => p.PartidaDia).ToList();
            var faltantes = new List<int>();

            for (int i = 0; i < ordenadas.Count - 1; i++) {
                for (int j = ordenadas[i].PartidaDia + 1; j < ordenadas[i + 1].PartidaDia; j++) {
                    faltantes.Add(j);
                }
            }

            // En lugar de usar MostrarAviso/MostrarInfo, escribimos en el TextBox
            if (faltantes.Count > 0) {
                txtResultados.Text = "PARTIDAS FALTANTES:" + Environment.NewLine +
                                     string.Join(", ", faltantes);
            }
            else {
                txtResultados.Text = "No hay huecos. Todas las partidas son consecutivas.";
            }

            // Hacemos visible el panel de resultados
            pnlResultados.Visible = true;
        }

        private void CargarEnFormulario(HistoricoEntry e) {
            nudPartida.Value = e.PartidaDia;
            nudPrecio.Value = e.Precio;
            nudVendidos.Value = e.Vendidos;
            nudLinea.Value = e.Premio1;
            nudBingo.Value = e.Premio2;
            ActualizarVentaPreview();
        }

        private void LimpiarFormulario() {
            nudPartida.Value = 1;
            nudPrecio.Value = 0;
            nudVendidos.Value = 0;
            nudLinea.Value = 0;
            nudBingo.Value = 0;
            ActualizarVentaPreview();
            _seleccionada = null;

            // Ocultar resultados previos
            txtResultados.Clear();
            pnlResultados.Visible = false;
        }

        private bool ValidarFormulario() {
            if (nudPartida.Value < 1) { MostrarAviso("El numero de partida debe ser mayor que 0."); nudPartida.Focus(); return false; }
            if (nudPrecio.Value <= 0) { MostrarAviso("El precio del carton debe ser mayor que 0."); nudPrecio.Focus(); return false; }
            if (nudVendidos.Value < 0) { MostrarAviso("Los cartones vendidos no pueden ser negativos."); nudVendidos.Focus(); return false; }
            return true;
        }

        private void SetBusy(bool ocupado) {
            Cursor = ocupado ? Cursors.WaitCursor : Cursors.Default;
            btnGuardar.Enabled = !ocupado;
            btnEliminar.Enabled = !ocupado;
            btnBuscar.Enabled = !ocupado;
            btnNueva.Enabled = !ocupado;
            btnVerificarHuecos.Enabled = !ocupado && _partidas.Count > 1;
        }

        private void MostrarError(string msg) => MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        private void MostrarAviso(string msg) => MessageBox.Show(msg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private void MostrarInfo(string msg) => MessageBox.Show(msg, "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private static int GetConfigInt(IConfiguration config, string key, int def) {
            var val = config.GetSection(key)?.Value;
            return int.TryParse(val, out int v) ? v : def;
        }

        private void btnCerrarResultados_Click(object sender, EventArgs e) {
            pnlResultados.Visible = false;
            txtResultados.Clear();
        }
    }
}
