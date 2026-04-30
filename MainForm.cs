using BingoHistoricoEditor.Data;
using BingoHistoricoEditor.Models;
using Microsoft.Extensions.Configuration;

namespace BingoHistoricoEditor {
    public partial class MainForm : Form {
        private readonly FirebirdService _db;
        private readonly int _codSala;
        private readonly int _codConc;
        private List<HistoricoEntry> _partidas = new();
        private HistoricoEntry? _seleccionada;

        public MainForm(FirebirdService db, IConfiguration config) {
            _db      = db;
            _codSala = GetConfigInt(config, "BingoSettings:CodSala", 1);
            _codConc = GetConfigInt(config, "BingoSettings:CodConc", 1);
            InitializeComponent();
            ConfigurarGrid();
        }

        // ── Inicialización ────────────────────────────────────────────────────

        protected override async void OnLoad(EventArgs e) {
            base.OnLoad(e);
            dtpFecha.Value = DateTime.Today;
            lblSalaConc.Text = $"Sala: {_codSala}  |  Conc: {_codConc}";

            bool ok = await _db.TestConnectionAsync();
            lblEstado.Text = ok ? "✓ Conectado a Firebird" : "✗ Sin conexión";
            lblEstado.ForeColor = ok ? Color.FromArgb(80, 200, 120) : Color.FromArgb(220, 80, 80);

            if (ok) await CargarPartidasAsync();
        }

        private void ConfigurarGrid() {
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartidaDia", HeaderText = "Partida",   Width = 65  });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Precio",     HeaderText = "Precio €",  Width = 80, DefaultCellStyle = { Format = "F2" } });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Vendidos",   HeaderText = "Vendidos",  Width = 80  });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Venta",      HeaderText = "Venta €",   Width = 90, DefaultCellStyle = { Format = "F2" } });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Premio1",    HeaderText = "Línea €",   Width = 80, DefaultCellStyle = { Format = "F2" } });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Premio2",    HeaderText = "Bingo €",   Width = 80, DefaultCellStyle = { Format = "F2" } });
        }

        // ── Carga de datos ────────────────────────────────────────────────────

        private async Task CargarPartidasAsync() {
            try {
                SetBusy(true);
                _partidas = await _db.CargarPorFechaAsync(dtpFecha.Value, _codSala, _codConc);

                //// ── DEBUG TEMPORAL — quitar cuando funcione ───────────────
                //MessageBox.Show(
                //    $"Parámetros enviados:\n" +
                //    $"  FECHA_SESION = {dtpFecha.Value.Date:yyyy-MM-dd}\n" +
                //    $"  COD_SALA     = {_codSala}\n" +
                //    $"  COD_CONC     = {_codConc}\n\n" +
                //    $"Filas devueltas: {_partidas.Count}",
                //    "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //// ─────────────────────────────────────────────────────────

                grid.DataSource = null;
                grid.DataSource = _partidas;
                lblTotal.Text = $"{_partidas.Count} partidas  |  Venta total: {_partidas.Sum(p => p.Venta):F2} €";
                LimpiarFormulario();
            }
            catch (Exception ex) {
                MostrarError(ex.Message);
            }
            finally {
                SetBusy(false);
            }
        }

        // ── Eventos de UI ─────────────────────────────────────────────────────

        private async void btnBuscar_Click(object sender, EventArgs e)
            => await CargarPartidasAsync();

        private void grid_SelectionChanged(object sender, EventArgs e) {
            if (grid.CurrentRow?.DataBoundItem is HistoricoEntry entrada) {
                _seleccionada = entrada;
                CargarEnFormulario(entrada);
            }
        }

        private void btnNueva_Click(object sender, EventArgs e) {
            _seleccionada = null;
            LimpiarFormulario();
            // Sugerir el siguiente número de partida
            int siguiente = _partidas.Count > 0 ? _partidas.Max(p => p.PartidaDia) + 1 : 1;
            nudPartida.Value = siguiente;
            nudPartida.Focus();
        }

        private async void btnGuardar_Click(object sender, EventArgs e) {
            if (!ValidarFormulario()) return;

            var entrada = new HistoricoEntry {
                CodSala     = _codSala,
                CodConc     = _codConc,
                FechaSesion = dtpFecha.Value.Date,
                PartidaDia  = (int)nudPartida.Value,
                Precio      = nudPrecio.Value,
                Vendidos    = (int)nudVendidos.Value,
                Premio1     = nudLinea.Value,
                Premio2     = nudBingo.Value,
            };

            try {
                SetBusy(true);
                await _db.GuardarAsync(entrada);
                await CargarPartidasAsync();
                MostrarInfo($"Partida {entrada.PartidaDia} guardada correctamente.");
            }
            catch (Exception ex) {
                MostrarError(ex.Message);
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
                $"¿Eliminar la partida {_seleccionada.PartidaDia} del {_seleccionada.FechaSesion:dd/MM/yyyy}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try {
                SetBusy(true);
                await _db.EliminarAsync(_seleccionada);
                await CargarPartidasAsync();
                MostrarInfo("Partida eliminada.");
            }
            catch (Exception ex) {
                MostrarError(ex.Message);
            }
            finally {
                SetBusy(false);
            }
        }

        // Recalcular venta total en tiempo real mientras el usuario escribe
        private void nudPrecio_ValueChanged(object sender, EventArgs e)    => ActualizarVentaPreview();
        private void nudVendidos_ValueChanged(object sender, EventArgs e)  => ActualizarVentaPreview();

        private void ActualizarVentaPreview() {
            decimal venta = nudPrecio.Value * nudVendidos.Value;
            lblVentaPreview.Text = $"Venta: {venta:F2} €";
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private void CargarEnFormulario(HistoricoEntry e) {
            nudPartida.Value  = e.PartidaDia;
            nudPrecio.Value   = e.Precio;
            nudVendidos.Value = e.Vendidos;
            nudLinea.Value    = e.Premio1;
            nudBingo.Value    = e.Premio2;
            ActualizarVentaPreview();
        }

        private void LimpiarFormulario() {
            nudPartida.Value  = 1;
            nudPrecio.Value   = 0;
            nudVendidos.Value = 0;
            nudLinea.Value    = 0;
            nudBingo.Value    = 0;
            ActualizarVentaPreview();
            _seleccionada     = null;
        }

        private bool ValidarFormulario() {
            if (nudPartida.Value < 1) {
                MostrarAviso("El número de partida debe ser mayor que 0.");
                nudPartida.Focus(); return false;
            }
            if (nudPrecio.Value <= 0) {
                MostrarAviso("El precio del cartón debe ser mayor que 0.");
                nudPrecio.Focus(); return false;
            }
            if (nudVendidos.Value < 0) {
                MostrarAviso("El número de cartones vendidos no puede ser negativo.");
                nudVendidos.Focus(); return false;
            }
            return true;
        }

        private void SetBusy(bool ocupado) {
            Cursor = ocupado ? Cursors.WaitCursor : Cursors.Default;
            btnGuardar.Enabled  = !ocupado;
            btnEliminar.Enabled = !ocupado;
            btnBuscar.Enabled   = !ocupado;
        }

        private void MostrarError (string msg) => MessageBox.Show(msg, "Error",   MessageBoxButtons.OK, MessageBoxIcon.Error);
        private void MostrarAviso (string msg) => MessageBox.Show(msg, "Aviso",   MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private void MostrarInfo  (string msg) => MessageBox.Show(msg, "Listo",   MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Helper para leer enteros desde IConfiguration sin depender de GetValue<T>
        private static int GetConfigInt(IConfiguration config, string key, int defaultValue) {
            try {
                var section = config.GetSection(key);
                if (section is not null) {
                    var val = section.Value;
                    if (!string.IsNullOrEmpty(val) && int.TryParse(val, out var parsed)) return parsed;
                }
            }
            catch {
                // ignorar y devolver valor por defecto
            }
            return defaultValue;
        }
    }
}
