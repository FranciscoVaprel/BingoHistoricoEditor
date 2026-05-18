using BingoHistoricoEditor.Data;
using BingoHistoricoEditor.Models;
using BingoHistoricoEditor.Services;
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
        private string _usuarioActual;

        public MainForm(FirebirdService db, IConfiguration config, string usuario) {
            InitializeComponent();
            _db = db;
            _codSala = GetConfigInt(config, "BingoSettings:CodSala", 1);
            _codConc = GetConfigInt(config, "BingoSettings:CodConc", 1);
            _usuarioActual = usuario;
            LogService.SetCurrentUser(usuario);
            ConfigurarGrid();
            ConfigurarLimitesNumericos();

            // ── NUEVAS LLAMADAS DE ESTILO PARA SOLUCIONAR EL MODO OSCURO ──
            ConfigurarEstilosLote();
            ConfigurarEstilosIndividual(); // <- Añade esta línea aquí

            this.dtpFecha.ValueChanged += dtpFecha_ValueChanged;
            // En el constructor de MainForm, justo debajo de ConfigurarEstilosIndividual();
            this.tlpEdicion.RowStyles.Clear(); // <- Borra cualquier residuo del diseñador
            this.tlpEdicion.RowCount = 5;      // <- Fuerza exactamente 5 filas

            for (int i = 0; i < 5; i++) {
                this.tlpEdicion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
            }

            // ── AGREGAR LOS CONTROLES DE EDICIÓN A tlpEdicion ──
            AgregarControlesAlTableLayout();

            this.FormClosed += (s, e) => BingoHistoricoEditor.Services.LogService.Close();
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
            nudPartida.Maximum = 99999;
            nudPrecio.Maximum = 9999;
            nudVendidos.Maximum = 9999;
            nudLinea.Maximum = 99999;
            nudBingo.Maximum = 99999;

            // Límites para los rangos de lote
            nudPartidaInicial.Maximum = 99999;
            nudPartidaFinal.Maximum = 99999;
        }

        private void AgregarControlesAlTableLayout() {
            // Limpiar controles previos si existen
            tlpEdicion.Controls.Clear();

            // Agregar los 5 filas de controles: Partida, Precio, Vendidos, Línea, Bingo
            AgregarControlEdicion("Partida:", nudPartida, 0);
            AgregarControlEdicion("Precio:", nudPrecio, 1);
            AgregarControlEdicion("Vendidos:", nudVendidos, 2);
            AgregarControlEdicion("Línea:", nudLinea, 3);
            AgregarControlEdicion("Bingo:", nudBingo, 4);
        }

        // Método de Log actualizado con el usuario
        private void EscribirLog(string accion, string detalles, bool registrarEnArchivo = false) {
             if (txtLog.IsDisposed) return;

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string usuario = _usuarioActual.ToUpper().PadRight(10);
            string linea = $"[{timestamp}] | {usuario} | {accion.PadRight(12)} | {detalles}{Environment.NewLine}";

            txtLog.AppendText(linea);
            txtLog.ScrollToCaret();
            
            if (registrarEnArchivo) {
                Services.LogService.Write(linea, null);
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e) => await CargarPartidasAsync();

        private async Task CargarPartidasAsync() {
            try {
                SetBusy(true);
                //EscribirLog("DB", $"Cargando datos del día {dtpFecha.Value:dd/MM/yyyy}...", registrarEnArchivo: false);

                // USANDO EL NOMBRE CORRECTO DEL MÉTODO DE TU SERVICIO
                _partidas = await _db.CargarPorFechaAsync(dtpFecha.Value, _codSala, _codConc);

                grid.DataSource = null;
                grid.DataSource = _partidas;
                //EscribirLog("INFO", $"Carga completa: {_partidas.Count} registros encontrados.", registrarEnArchivo: false);
            }
            catch (Exception ex) {
                EscribirLog("ERROR", ex.Message, registrarEnArchivo: false);
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
                    EscribirLog("GRID", $"Registro {_seleccionada.PartidaDia} seleccionado para editar.", registrarEnArchivo: false);
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

                EscribirLog("DB_INSERT/UPDATE", $"Iniciando guardado de Partida {entry.PartidaDia}", registrarEnArchivo: true);
                await _db.GuardarAsync(entry);

                EscribirLog("EXITO", $"Partida {entry.PartidaDia} procesada correctamente.", registrarEnArchivo: true);
                await CargarPartidasAsync();
            }
            catch (Exception ex) {
                EscribirLog("ERROR", ex.Message, registrarEnArchivo: true);
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
            //EscribirLog("ACCION", "Limpieza de campos para nueva partida.", registrarEnArchivo: false);
        }

        private async void btnEliminar_Click(object sender, EventArgs e) {
            if (_seleccionada == null) return;

            var res = MessageBox.Show($"¿Eliminar partida {_seleccionada.PartidaDia}?", "Confirmar", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes) {
                try {
                    SetBusy(true);
                    EscribirLog("DB_DELETE", $"Eliminando Partida {_seleccionada.PartidaDia}", registrarEnArchivo: true);
                    await _db.EliminarAsync(_seleccionada);

                    EscribirLog("EXITO", $"Partida {_seleccionada.PartidaDia} eliminada de la base de datos.", registrarEnArchivo: true);
                    await CargarPartidasAsync();
                }
                catch (Exception ex) {
                    EscribirLog("ERROR_CRITICO", $"Fallo al eliminar: {ex.Message}", registrarEnArchivo: true);
                }
                finally {
                    SetBusy(false);
                }
            }
        }

        /// <summary>
        /// Asigna el estado enabled de los botones
        /// </summary>
        /// <param name="busy"></param>
        private void SetBusy(bool busy) {
            this.Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            btnBuscar.Enabled = !busy;
            btnGuardar.Enabled = !busy;
            btnEliminar.Enabled = !busy;

            // Deshabilitar también los botones de lote en procesos asíncronos
            btnBuscarFaltante.Enabled = !busy;
            btnGuardarLote.Enabled = !busy;
            btnEliminarLote.Enabled = !busy;
        }

        private static int GetConfigInt(IConfiguration config, string key, int def) {
            var val = config.GetSection(key)?.Value;
            return int.TryParse(val, out int v) ? v : def;
        }

        private void btnNuevaPartida_Click(object sender, EventArgs e) {
            _seleccionada = null;
            grid.ClearSelection();

            // Limpiar controles
            nudPartida.Value = 0;
            nudPrecio.Value = 0;
            nudVendidos.Value = 0;
            nudLinea.Value = 0;
            nudBingo.Value = 0;

            EscribirLog("SISTEMA", "Listo para crear nueva partida.");
        }

        private async void btnBuscarFaltante_Click(object sender, EventArgs e) {
            int inicio = (int)nudPartidaInicial.Value;
            int fin = (int)nudPartidaFinal.Value;

            if (inicio > fin) {
                EscribirLog("ADVERTENCIA", "El rango de búsqueda no es válido (inicio mayor que fin).", registrarEnArchivo: false);
                return;
            }

            try {
                SetBusy(true);
                EscribirLog("INFO", $"Consultando huecos libres en BD entre {inicio} y {fin}...", registrarEnArchivo: false);

                // 1. Buscamos en la Base de Datos los números ocupados
                HashSet<int> ocupados = await _db.ObtenerPartidasExistentesEnRangoAsync(dtpFecha.Value, _codSala, _codConc, inicio, fin);

                // 2. Buscamos secuencialmente el primer número que NO esté en el conjunto
                int primerFaltante = -1;
                for (int i = inicio; i <= fin; i++) {
                    if (!ocupados.Contains(i)) {
                        primerFaltante = i;
                        break;
                    }
                }

                // 3. Procesamos el resultado
                if (primerFaltante != -1) {
                    // Encontrado: Se lo asignamos al control numérico directamente
                    nudPartidaInicial.Value = primerFaltante;

                    EscribirLog("EXITO", $"Primer número disponible encontrado: {primerFaltante}", registrarEnArchivo: false);
                    MessageBox.Show($"La primera partida faltante en el rango es la número: {primerFaltante}.\n\nSe ha colocado automáticamente como Partida Inicial.",
                                    "Hueco Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else {
                    // No hay huecos libres en todo el rango
                    EscribirLog("ADVERTENCIA", $"No hay huecos libres en el rango [{inicio} - {fin}]. Todas las partidas existen.", registrarEnArchivo: false);
                    MessageBox.Show("Todas las partidas en el rango seleccionado ya existen en la base de datos para esta fecha.",
                                    "Rango Completo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex) {
                EscribirLog("ERROR", $"Error al buscar faltante en BD: {ex.Message}", registrarEnArchivo: true);
                MessageBox.Show($"Ocurrió un error al consultar la base de datos:\n{ex.Message}", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                SetBusy(false);
            }
        }

        private async void btnGuardarLote_Click(object sender, EventArgs e) {
            int inicio = (int)nudPartidaInicial.Value;
            int fin = (int)nudPartidaFinal.Value;

            if (inicio > fin) {
                EscribirLog("ADVERTENCIA", "La partida inicial no puede ser mayor que la final.", registrarEnArchivo: false);
                return;
            }

            try {
                SetBusy(true);
                EscribirLog("INFO", $"Iniciando guardado en lote desde {inicio} hasta {fin}...", registrarEnArchivo: true);
                // Aquí irá la lógica para insertar/actualizar masivamente usando el rango
            }
            catch (Exception ex) {
                EscribirLog("ERROR_CRITICO", $"Fallo en guardado masivo: {ex.Message}", registrarEnArchivo: true);
            }
            finally {
                SetBusy(false);
            }
        }

        private async void btnEliminarLote_Click(object sender, EventArgs e) {
            int inicio = (int)nudPartidaInicial.Value;
            int fin = (int)nudPartidaFinal.Value;

            var confirm = MessageBox.Show(
                $"¿Estás seguro de que deseas eliminar TODAS las partidas entre la {inicio} y la {fin}?",
                "Confirmación de eliminación masiva",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes) return;

            try {
                SetBusy(true);
                EscribirLog("INFO", $"Eliminando lote de partidas ({inicio} - {fin})...", registrarEnArchivo: true);
                // Aquí irá el Delete masivo en base de datos
            }
            catch (Exception ex) {
                EscribirLog("ERROR_CRITICO", $"Fallo al eliminar lote: {ex.Message}", registrarEnArchivo: true);
            }
            finally {
                SetBusy(false);
            }
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e) {                        
            AsignarLimitesPartidasLote();
        }

        private void AsignarLimitesPartidasLote() {
            // Aseguramos que los labels tengan color claro para que no se vuelvan invisibles con el fondo oscuro
            if (lblIni != null) lblIni.ForeColor = System.Drawing.Color.LightGray;
            if (lblFin != null) lblFin.ForeColor = System.Drawing.Color.LightGray;

            // CORRECCIÓN CRÍTICA: Comprobar que la lista NO esté vacía (_partidas.Count > 0)
            if (_partidas != null && _partidas.Count > 0) {
                int min = int.MaxValue;
                int max = int.MinValue;

                // Buscamos los extremos reales en la lista de la fecha seleccionada
                foreach (var partida in _partidas) {
                    if (partida.PartidaDia < min) min = partida.PartidaDia;
                    if (partida.PartidaDia > max) max = partida.PartidaDia;
                }

                // Controlamos que los valores estén dentro del rango permitido por tus NumericUpDown
                nudPartidaInicial.Value = Math.Max(nudPartidaInicial.Minimum, Math.Min(nudPartidaInicial.Maximum, min));
                nudPartidaFinal.Value = Math.Max(nudPartidaFinal.Minimum, Math.Min(nudPartidaFinal.Maximum, max));

                EscribirLog("INFO", $"Lote preestablecido automáticamente: Rango [{min} - {max}] para la fecha actual.", registrarEnArchivo: false);
            }
            else {
                // Si no hay datos cargados en el Grid, asignamos el valor mínimo configurado (evita el 99999 y 0)
                nudPartidaInicial.Value = nudPartidaInicial.Minimum;
                nudPartidaFinal.Value = nudPartidaFinal.Minimum;

                EscribirLog("ADVERTENCIA", "No hay partidas cargadas en esta fecha. Selecciona una fecha con datos y pulsa Buscar.", registrarEnArchivo: false);
            }
        }
        private void ConfigurarEstilosLote() {
            // Aseguramos que tengan texto asignado por si acaso
            lblIni.Text = "Partida Inicial:";
            lblFin.Text = "Partida Final:";

            // Forzamos el color claro para el modo oscuro
            lblIni.ForeColor = Color.LightGray;
            lblFin.ForeColor = Color.LightGray;
            grpLote.ForeColor = Color.White; // Esto aclara también el título del recuadro del Lote

            // Alineación fluida dentro de la tabla del lote
            lblIni.TextAlign = ContentAlignment.MiddleLeft;
            lblFin.TextAlign = ContentAlignment.MiddleLeft;
            lblIni.Dock = DockStyle.Fill;
            lblFin.Dock = DockStyle.Fill;
        }

        private void ConfigurarEstilosIndividual() {
            // 1. CRUCIAL para Modo Oscuro en TabPages: Desactivar el estilo visual nativo
            if (tabIndividual != null) {
                tabIndividual.UseVisualStyleBackColor = false; // Permite aplicar el BackColor personalizado
                tabIndividual.BackColor = Color.FromArgb(30, 30, 35);
            }
            if (tabLote != null) {
                tabLote.UseVisualStyleBackColor = false;
                tabLote.BackColor = Color.FromArgb(30, 30, 35);
            }

            // 2. Estilo del contenedor GroupBox de edición
            if (grpEdicion != null) {
                grpEdicion.BackColor = Color.FromArgb(45, 45, 50);
                grpEdicion.ForeColor = Color.White; // Color del título del GroupBox
            }

            // 3. Estilo explícito para el TableLayoutPanel interno
            if (tlpEdicion != null) {
                tlpEdicion.BackColor = Color.FromArgb(45, 45, 50);

                // 4. Forzar par Background/Foreground en todos los inputs
                foreach (Control ctrl in tlpEdicion.Controls) {
                    if (ctrl is Label lbl) {
                        lbl.ForeColor = Color.LightGray;
                        lbl.BackColor = Color.Transparent; // Evita bloques grises alrededor del texto
                    }
                    else if (ctrl is NumericUpDown nud) {
                        nud.BackColor = Color.FromArgb(35, 35, 40);
                        nud.ForeColor = Color.White;
                    }
                    else if (ctrl is TextBox txt) {
                        txt.BackColor = Color.FromArgb(35, 35, 40);
                        txt.ForeColor = Color.White;
                    }
                }
            }
        }
    }
}