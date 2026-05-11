using BingoHistoricoEditor.Data;
using BingoHistoricoEditor.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingoHistoricoEditor {
    public partial class MainForm : Form {
        private readonly FirebirdService _db;
        private readonly int _codSala;
        private readonly int _codConc;
        private List<HistoricoEntry> _partidas = new();
        private HistoricoEntry? _seleccionada;

        public MainForm(FirebirdService db, IConfiguration config) {
            InitializeComponent();
            _db = db;
            _codSala = GetConfigInt(config, "BingoSettings:CodSala", 1);
            _codConc = GetConfigInt(config, "BingoSettings:CodConc", 1);

            ConfigurarGrid();
            ConfigurarLimitesNumericos();
        }

        protected override async void OnLoad(EventArgs e) {
            base.OnLoad(e);
            dtpFecha.Value = DateTime.Today;
            lblSalaConc.Text = $"SALA: {_codSala} | CONCESIONARIO: {_codConc}";

            EscribirLog("SISTEMA", "Probando conexión a base de datos...");
            bool ok = await _db.TestConnectionAsync(); //

            lblEstado.Text = ok ? "Estado: Conectado a Firebird" : "Estado: ERROR DE CONEXIÓN";
            lblEstado.ForeColor = ok ? Color.LimeGreen : Color.Red;

            if (ok) {
                EscribirLog("SISTEMA", "Conexión establecida correctamente.");
                await CargarPartidasAsync();
            }
        }

        private void ConfigurarGrid() {
            grid.AutoGenerateColumns = true;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AllowUserToAddRows = false;
        }

        private void ConfigurarLimitesNumericos() {
            decimal max = decimal.MaxValue;
            nudPartida.Maximum = max;
            nudPrecio.Maximum = max;
            nudVendidos.Maximum = max;
            nudLinea.Maximum = max;
            nudBingo.Maximum = max;
        }

        private void EscribirLog(string nivel, string mensaje) {
            if (txtLog.IsDisposed) return;
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {nivel}: {mensaje}{Environment.NewLine}");
            txtLog.ScrollToCaret();
        }

        private async void btnBuscar_Click(object sender, EventArgs e) => await CargarPartidasAsync();

        private async Task CargarPartidasAsync() {
            try {
                SetBusy(true);
                EscribirLog("DB", $"Cargando datos del día {dtpFecha.Value:dd/MM/yyyy}...");

                // USANDO EL NOMBRE CORRECTO DEL MÉTODO DE TU SERVICIO
                _partidas = await _db.CargarPorFechaAsync(dtpFecha.Value, _codSala, _codConc);

                grid.DataSource = null;
                grid.DataSource = _partidas;
                EscribirLog("INFO", $"Carga completa: {_partidas.Count} registros encontrados.");
            }
            catch (Exception ex) {
                EscribirLog("ERROR", ex.Message);
            }
            finally {
                SetBusy(false);
            }
        }

        private void grid_SelectionChanged(object sender, EventArgs e) {
            if (grid.SelectedRows.Count > 0) {
                _seleccionada = grid.SelectedRows[0].DataBoundItem as HistoricoEntry;
                if (_seleccionada != null) {
                    nudPartida.Value = _seleccionada.PartidaDia;
                    nudPrecio.Value = _seleccionada.Precio;
                    nudVendidos.Value = _seleccionada.Vendidos;
                    nudLinea.Value = _seleccionada.Premio1;
                    nudBingo.Value = _seleccionada.Premio2;
                    EscribirLog("GRID", $"Registro {_seleccionada.PartidaDia} seleccionado para editar.");
                }
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e) {
            try {
                SetBusy(true);
                var entry = _seleccionada ?? new HistoricoEntry {
                    CodSala = _codSala,
                    CodConc = _codConc,
                    FechaSesion = dtpFecha.Value.Date,
                    Fecha = DateTime.Now
                };

                entry.PartidaDia = (int)nudPartida.Value;
                entry.Precio = nudPrecio.Value;
                entry.Vendidos = (int)nudVendidos.Value;
                entry.Premio1 = nudLinea.Value;
                entry.Premio2 = nudBingo.Value;

                EscribirLog("DB", "Enviando datos al servidor...");
                await _db.GuardarAsync(entry); //
                EscribirLog("EXITO", $"La partida {entry.PartidaDia} se guardó correctamente.");

                await CargarPartidasAsync();
            }
            catch (Exception ex) {
                EscribirLog("ERROR", ex.Message);
            }
            finally {
                SetBusy(false);
            }
        }

        private void btnNueva_Click(object sender, EventArgs e) {
            _seleccionada = null;
            grid.ClearSelection();
            nudPartida.Value = _partidas.Count + 1;
            nudPrecio.Value = 1.50m;
            nudVendidos.Value = 0;
            nudLinea.Value = 0;
            nudBingo.Value = 0;
            EscribirLog("ACCION", "Limpieza de campos para nueva partida.");
        }

        private async void btnEliminar_Click(object sender, EventArgs e) {
            if (_seleccionada == null) {
                EscribirLog("AVISO", "Selecciona una partida en la tabla primero.");
                return;
            }

            var res = MessageBox.Show($"¿Deseas eliminar permanentemente la partida {_seleccionada.PartidaDia}?",
                                     "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (res == DialogResult.Yes) {
                try {
                    SetBusy(true);
                    await _db.EliminarAsync(_seleccionada); //
                    EscribirLog("DB", "Registro eliminado de la base de datos.");
                    await CargarPartidasAsync();
                }
                catch (Exception ex) {
                    EscribirLog("ERROR", ex.Message);
                }
                finally {
                    SetBusy(false);
                }
            }
        }

        private void SetBusy(bool busy) {
            this.Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            btnBuscar.Enabled = !busy;
            btnGuardar.Enabled = !busy;
            btnEliminar.Enabled = !busy;
        }

        private static int GetConfigInt(IConfiguration config, string key, int def) {
            var val = config.GetSection(key)?.Value;
            return int.TryParse(val, out int v) ? v : def;
        }
    }
}