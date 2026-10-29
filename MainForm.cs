using BingoHistoricoEditor.Data;
using BingoHistoricoEditor.Models;
using BingoHistoricoEditor.Services;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingoHistoricoEditor {
    public partial class MainForm : Form {
        private readonly FirebirdService _db;
        private readonly IConfiguration _config;
        private readonly int _codSala;
        private readonly int _codConc;
        private List<HistoricoEntry> _partidas = new();
        private HistoricoEntry? _seleccionada;
        private string _usuarioActual;
        private readonly bool _esAdmin;

        public MainForm(FirebirdService db, IConfiguration config, string usuario) {
            InitializeComponent();
            _db = db;
            _config = config;
            _codSala = GetConfigInt(config, "BingoSettings:CodSala", 1);
            _codConc = GetConfigInt(config, "BingoSettings:CodConc", 1);
            _usuarioActual = usuario;
            _esAdmin = UserStore.EsAdmin(usuario);
            LogService.SetCurrentUser(usuario);
            LogService.SetDbService(db);  // Pasar el servicio de BD para logueo

            ConfigurarGrid();
            ConfigurarLimitesNumericos();
            ConfigurarEstilosIndividual();
            ConfigurarEstilosLote();

            // La auditoría solo la puede abrir un administrador.
            if (btnAuditoria != null) btnAuditoria.Visible = _esAdmin;

            // Suscribir a eventos después de que InitializeComponent() ha creado todos los controles
            if (dtpFecha != null) dtpFecha.ValueChanged += dtpFecha_ValueChanged;
            this.FormClosed += (s, e) => BingoHistoricoEditor.Services.LogService.Close();
        }

        protected override async void OnLoad(EventArgs e) {
            base.OnLoad(e);

            dtpFecha.Value = DateTime.Today;
            lblSalaConc.Text = $"SALA: {_codSala} | CONCESIONARIO: {_codConc}";

            // Verificar si la BD existe
            string rutaBD = _db.ObtenerRutaBD();
            bool existeBD = _db.ExisteBD();

            EscribirLog("SISTEMA", $"BD configurada en: {rutaBD}");
            if (!existeBD) {
                EscribirLog("ADVERTENCIA", "El archivo .fdb NO existe. Se creará al conectar.");
            } else {
                EscribirLog("SISTEMA", "✓ Archivo .fdb encontrado.");
            }

            EscribirLog("SISTEMA", "Probando conexión a base de datos...");
            bool ok = await _db.TestConnectionAsync(); //

            lblEstado.Text = ok ? "Estado: Conectado a Firebird" : "Estado: ERROR DE CONEXIÓN";
            lblEstado.ForeColor = ok ? Color.Green : Color.Red;

            if (ok) {
                EscribirLog("SISTEMA", "Conexión establecida correctamente.");

                // Crear tabla de operaciones y trigger de auditoría si no existen
                try {
                    await _db.CrearTablaOperacionesAsync();
                    await _db.CrearTriggerAuditoriaHistoricoAsync();
                }
                catch (Exception ex) {
                    EscribirLog("SISTEMA", $"Error al crear tabla/trigger de auditoría: {ex.Message}");
                }

                // Cargar por defecto el último día con partidas registradas
                try {
                    DateTime? ultimaFecha = await _db.ObtenerUltimaFechaConDatosAsync(_codSala, _codConc);
                    if (ultimaFecha.HasValue) {
                        dtpFecha.Value = ultimaFecha.Value.Date;
                        EscribirLog("SISTEMA", $"Último día con datos: {ultimaFecha.Value:yyyy-MM-dd}.");
                    }
                    else {
                        EscribirLog("SISTEMA", "No hay partidas registradas todavía; se muestra la fecha de hoy.");
                    }
                }
                catch (Exception ex) {
                    EscribirLog("ERROR", $"No se pudo determinar el último día con datos: {ex.Message}");
                }

                await CargarPartidasAsync();
            }
        }

        private void ConfigurarGrid() {
            grid.AutoGenerateColumns = true;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = true;                    // ← Importante para rango con Shift
            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AllowUserToAddRows = false;

            // Colores alternos (paleta estándar de Windows)
            grid.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;
            grid.DefaultCellStyle.BackColor = SystemColors.Window;
            grid.DefaultCellStyle.ForeColor = SystemColors.WindowText;
            grid.BackgroundColor = SystemColors.Window;
        }

        private void ConfigurarLimitesNumericos() {
            nudPartida.Minimum = 0;
            nudPartida.Maximum = 99999;
            nudPrecio.Minimum = 0;
            nudPrecio.Maximum = 9999;
            nudVendidos.Maximum = 9999;
            nudLinea.Maximum = 99999;
            nudBingo.Maximum = 99999;

            // Cargar el precio de partida por defecto desde configuración
            decimal precioPartida = Program.ObtenerPrecioPartida();
            nudPrecio.Value = precioPartida;
            nudLotePrecio.Value = precioPartida;

            // Límites para los rangos de lote
            nudPartidaInicial.Minimum = 0;
            nudPartidaInicial.Maximum = 99999;
            nudPartidaFinal.Maximum = 99999;
        }

        /// <summary>
        /// Agrega un par Label + NumericUpDown a una fila específica del TableLayoutPanel.
        /// Se usa solo si se necesita agregar dinámicamente controles.
        /// </summary>
        private void AgregarControlEdicion(string etiqueta, NumericUpDown control, int fila) {
            if (tlpEdicion == null) return;

            var lbl = new Label {
                Text = etiqueta,
                ForeColor = SystemColors.ControlText,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };

            control.BackColor = SystemColors.Window;
            control.ForeColor = SystemColors.WindowText;
            control.Dock = DockStyle.Fill;

            tlpEdicion.Controls.Add(lbl, 0, fila);
            tlpEdicion.Controls.Add(control, 1, fila);
        }

        // Método de Log actualizado con el usuario
        // 'datos' es opcional: cuando se indica, se guarda serializado en el campo
        // DETALLES del log de fichero y de la tabla de auditoría (operacionesBGEditor).
        private void EscribirLog(string accion, string detalles, bool registrarEnArchivo = false, object? datos = null) {
            if (txtLog.IsDisposed) return;

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string usuario = _usuarioActual.ToUpper().PadRight(10);
            string linea = $"[{timestamp}] | {usuario} | {accion.PadRight(12)} | {detalles}{Environment.NewLine}";

            txtLog.AppendText(linea);
            txtLog.ScrollToCaret();

            if (registrarEnArchivo) {
                Services.LogService.Write(accion, datos ?? (object)detalles);
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e) => await CargarPartidasAsync();

        private async Task CargarPartidasAsync() {
            try {
                SetBusy(true);

                _partidas = await _db.CargarPorFechaAsync(dtpFecha.Value, _codSala, _codConc);

                grid.DataSource = null;
                grid.DataSource = _partidas;

                // === NUEVA LÓGICA: Asignar automáticamente Partida Final ===
                if (_partidas != null && _partidas.Count > 0) {
                    int maxPartida = _partidas.Max(p => p.PartidaDia);
                    int minPartida = _partidas.Min(p => p.PartidaDia);

                    nudPartidaInicial.Value = Math.Max(nudPartidaInicial.Minimum, minPartida);
                    nudPartidaFinal.Value = Math.Max(nudPartidaFinal.Minimum, Math.Min(nudPartidaFinal.Maximum, maxPartida));

                    EscribirLog("INFO", $"Rango de lote actualizado automáticamente: {minPartida} - {maxPartida}", false);
                }
                else {
                    nudPartidaInicial.Value = nudPartidaInicial.Minimum;
                    nudPartidaFinal.Value = nudPartidaFinal.Minimum;
                }

            }
            catch (Exception ex) {
                EscribirLog("ERROR", ex.Message, false);
            }
            finally {
                SetBusy(false);
            }
        }

        private void grid_SelectionChanged(object sender, EventArgs e) {
            if (grid.SelectedRows.Count == 0) return;

            // Ordenar filas seleccionadas por índice
            var filasSeleccionadas = grid.SelectedRows.Cast<DataGridViewRow>()
                .OrderBy(r => r.Index)
                .ToList();

            if (filasSeleccionadas.Count > 0) {
                // Primera seleccionada → Partida Inicial
                var primera = filasSeleccionadas.First().DataBoundItem as HistoricoEntry;
                if (primera != null) {
                    AsignarAcotado(nudPartidaInicial, primera.PartidaDia);
                }

                // Última seleccionada → Partida Final (funciona con Shift + clic)
                if (filasSeleccionadas.Count > 1) {
                    var ultima = filasSeleccionadas.Last().DataBoundItem as HistoricoEntry;
                    if (ultima != null) {
                        AsignarAcotado(nudPartidaFinal, ultima.PartidaDia);
                    }
                }
            }

            // Actualizar selección individual si no hay rango
            if (filasSeleccionadas.Count == 1) {
                _seleccionada = filasSeleccionadas[0].DataBoundItem as HistoricoEntry;
                if (_seleccionada != null) {
                    AsignarAcotado(nudPartida, _seleccionada.PartidaDia);
                    AsignarAcotado(nudPrecio, _seleccionada.Precio);
                    AsignarAcotado(nudVendidos, _seleccionada.Vendidos);
                    AsignarAcotado(nudLinea, _seleccionada.Premio1);
                    AsignarAcotado(nudBingo, _seleccionada.Premio2);
                }
            }
        }

        /// <summary>
        /// Asigna un valor a un NumericUpDown acotándolo a su [Minimum, Maximum].
        /// Evita que un registro de la BD fuera de los límites habituales de la UI
        /// (p. ej. una PARTIDA_DIA = 0 insertada por otra vía) haga lanzar
        /// ArgumentOutOfRangeException al seleccionarlo en la rejilla.
        /// </summary>
        private static void AsignarAcotado(NumericUpDown control, decimal valor) {
            control.Value = Math.Max(control.Minimum, Math.Min(control.Maximum, valor));
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

                EscribirLog("EXITO", $"Partida {entry.PartidaDia} procesada correctamente.", registrarEnArchivo: true,
                    datos: new {
                        entry.PartidaDia, entry.FechaSesion,
                        entry.Precio, entry.Vendidos, entry.Venta,
                        Linea = entry.Premio1, Bingo = entry.Premio2
                    });
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
                    EscribirLog("DB_DELETE", $"Eliminando Partida {_seleccionada.PartidaDia}", registrarEnArchivo: true,
                        datos: new {
                            _seleccionada.PartidaDia, _seleccionada.FechaSesion,
                            _seleccionada.Precio, _seleccionada.Vendidos, _seleccionada.Venta,
                            Linea = _seleccionada.Premio1, Bingo = _seleccionada.Premio2
                        });
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

            // Limpiar todos los controles excepto Partida
            nudPrecio.Value = 0;
            nudVendidos.Value = 0;
            nudLinea.Value = 0;
            nudBingo.Value = 0;

            // Buscar la primera partida faltante en los datos actuales
            if (_partidas != null && _partidas.Count > 0) {
                // Obtener el rango min y max de las partidas cargadas
                int min = _partidas.Min(p => p.PartidaDia);
                int max = _partidas.Max(p => p.PartidaDia);

                // Buscar la primera partida faltante en ese rango
                int primerFaltante = -1;
                for (int i = min; i <= max; i++) {
                    if (!_partidas.Any(p => p.PartidaDia == i)) {
                        primerFaltante = i;
                        break;
                    }
                }

                // Si no hay faltante en el rango, usar max + 1
                if (primerFaltante == -1) {
                    primerFaltante = max + 1;
                }

                nudPartida.Value = primerFaltante;
                EscribirLog("SISTEMA", $"Listo para crear nueva partida. Número sugerido: {primerFaltante}");
            }
            else {
                // Si no hay partidas, empezar desde 1
                nudPartida.Value = 1;
                EscribirLog("SISTEMA", "Listo para crear nueva partida. (Sin datos cargados)");
            }
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

                // 2. Buscamos el PRIMER grupo de huecos consecutivos
                int primerFaltante = -1;
                int ultimoFaltante = -1;

                for (int i = inicio; i <= fin; i++) {
                    // Si encontramos el primer hueco libre
                    if (!ocupados.Contains(i) && primerFaltante == -1) {
                        primerFaltante = i;
                    }
                    // Si ya tenemos el primer hueco, buscamos dónde termina la secuencia
                    else if (primerFaltante != -1 && !ocupados.Contains(i)) {
                        ultimoFaltante = i;
                    }
                    // Si encontramos un ocupado después de huecos, terminamos
                    else if (primerFaltante != -1 && ocupados.Contains(i)) {
                        break;
                    }
                }

                // 3. Procesamos el resultado
                if (primerFaltante != -1) {
                    // Encontrado: Se asignan el primer y último faltante del primer grupo
                    nudPartidaInicial.Value = primerFaltante;
                    nudPartidaFinal.Value = ultimoFaltante;

                    EscribirLog("EXITO", $"Primer grupo de huecos encontrado: [{primerFaltante} - {ultimoFaltante}]", registrarEnArchivo: false);
                    MessageBox.Show($"Primer grupo de partidas faltantes encontrado en el rango [{primerFaltante} - {ultimoFaltante}].\n\nSe han colocado automáticamente como rango de búsqueda.",
                                    "Huecos Encontrados", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (inicio > fin) { EscribirLog("ADVERTENCIA", "Rango inválido"); return; }

            try {
                SetBusy(true);
                EscribirLog("INFO", $"Guardando lote {inicio}-{fin}", true, datos: new {
                    Fecha = dtpFecha.Value.Date, PartidaInicial = inicio, PartidaFinal = fin,
                    Precio = nudLotePrecio.Value, Vendidos = nudLoteVendidos.Value
                });

                var entradas = new List<HistoricoEntry>();
                for (int p = inicio; p <= fin; p++) {
                    entradas.Add(new HistoricoEntry {
                        CodSala = _codSala,
                        CodConc = _codConc,
                        FechaSesion = dtpFecha.Value.Date,
                        PartidaDia = p,
                        Fecha = DateTime.Now,
                        Precio = nudLotePrecio.Value,
                        Vendidos = (int)nudLoteVendidos.Value,
                        Premio1 = 0,
                        Premio2 = 0
                    });
                }
                await _db.GuardarLoteAsync(entradas);

                EscribirLog("EXITO", "Lote guardado correctamente", true);
                await CargarPartidasAsync();
            }
            catch (Exception ex) {
                EscribirLog("ERROR", ex.Message, true);
            }
            finally { SetBusy(false); }
        }

        private async void btnEliminarLote_Click(object sender, EventArgs e) {
            int inicio = (int)nudPartidaInicial.Value;
            int fin = (int)nudPartidaFinal.Value;

            if (inicio > fin) {
                EscribirLog("ADVERTENCIA", "Rango inválido (inicio > fin)");
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Eliminar TODAS las partidas del {inicio} al {fin}?\n\nEsta acción es IRREVERSIBLE.",
                "⚠️ Confirmar eliminación masiva",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try {
                SetBusy(true);

                var partidasExistentes = await _db.ObtenerPartidasExistentesEnRangoAsync(
                    dtpFecha.Value, _codSala, _codConc, inicio, fin);

                EscribirLog("DB_DELETE_LOTE", $"Eliminando rango {inicio}-{fin}", true, datos: new {
                    Fecha = dtpFecha.Value.Date, PartidaInicial = inicio, PartidaFinal = fin,
                    PartidasEliminadas = partidasExistentes.OrderBy(p => p).ToList()
                });

                await _db.EliminarRangoAsync(dtpFecha.Value.Date, _codSala, _codConc, inicio, fin);

                EscribirLog("EXITO", $"Lote {inicio}-{fin} eliminado correctamente", true);
                await CargarPartidasAsync();
            }
            catch (Exception ex) {
                EscribirLog("ERROR_CRITICO", ex.Message, true);
            }
            finally {
                SetBusy(false);
            }
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e) {
            //AsignarLimitesPartidasLote();
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
            // Aplicar estilos a los controles existentes en grpLote
            // SIN recrear ningún control - solo aplicar propiedades visuales

            // Estilo del GroupBox (paleta estándar de Windows)
            grpLote.BackColor = SystemColors.Control;
            grpLote.ForeColor = SystemColors.ControlText;

            // Estilo del TableLayoutPanel
            tlpLoteCampos.BackColor = SystemColors.Control;

            // Aplicar estilos a todos los controles dentro de tlpLoteCampos
            foreach (Control ctrl in tlpLoteCampos.Controls) {
                if (ctrl is Label lbl) {
                    lbl.ForeColor = SystemColors.ControlText;
                    lbl.BackColor = Color.Transparent;
                    lbl.TextAlign = ContentAlignment.MiddleLeft;
                }
                else if (ctrl is NumericUpDown nud) {
                    nud.BackColor = SystemColors.Window;
                    nud.ForeColor = SystemColors.WindowText;
                }
            }

            // Configurar textos de las etiquetas del rango
            if (lblIni != null) {
                lblIni.Text = "Partida Inicial:";
            }
            if (lblFin != null) {
                lblFin.Text = "Partida Final:";
            }

            // Estilo de los botones en flpBotonesLote
            foreach (Control ctrl in flpBotonesLote.Controls) {
                if (ctrl is Button btn) {
                    btn.FlatStyle = FlatStyle.Standard;
                    btn.UseVisualStyleBackColor = true;
                    btn.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private void ConfigurarEstilosIndividual() {
            // Aplicar estilos a los tabs (paleta estándar de Windows)
            if (tabIndividual != null) {
                tabIndividual.UseVisualStyleBackColor = true;
                tabIndividual.BackColor = SystemColors.Control;
            }
            if (tabLote != null) {
                tabLote.UseVisualStyleBackColor = true;
                tabLote.BackColor = SystemColors.Control;
            }

            // Aplicar estilos al GroupBox de edición
            if (grpEdicion != null) {
                grpEdicion.BackColor = SystemColors.Control;
                grpEdicion.ForeColor = SystemColors.ControlText;
            }

            // Aplicar estilos al TableLayoutPanel y sus controles
            if (tlpEdicion != null) {
                tlpEdicion.BackColor = SystemColors.Control;

                foreach (Control ctrl in tlpEdicion.Controls) {
                    if (ctrl is Label lbl) {
                        lbl.ForeColor = SystemColors.ControlText;
                        lbl.BackColor = Color.Transparent;
                        lbl.TextAlign = ContentAlignment.MiddleLeft;
                    }
                    else if (ctrl is NumericUpDown nud) {
                        nud.BackColor = SystemColors.Window;
                        nud.ForeColor = SystemColors.WindowText;
                    }
                }
            }
        }

        private void btnAuditoria_Click(object sender, EventArgs e) {
            if (!_esAdmin) return; // defensa en profundidad, no debería ocurrir (botón oculto)
            using var auditoria = new AuditoriaForm(_db);
            auditoria.ShowDialog(this);
        }

        private void btnConfiguracion_Click(object sender, EventArgs e) {
            using (var settingsForm = new SettingsForm(_config)) {
                if (settingsForm.ShowDialog(this) == DialogResult.OK) {
                    // Recargar la configuración actualizada
                    MessageBox.Show("Configuración guardada correctamente.\n\nPor favor, reinicia la aplicación para aplicar los cambios de base de datos.",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Método de DEBUG: Verifica las operaciones guardadas en la BD
        /// Llama esto desde la consola de debug con: await MainFormInstance.VerificarOperacionesAsync();
        /// </summary>
        public async Task VerificarOperacionesAsync() {
            try {
                int totalOps = await _db.ObtenerCountOperacionesAsync();
                EscribirLog("DEBUG", $"Total de operaciones en BD: {totalOps}");

                var ultimas = await _db.ObtenerOperacionesAsync(ultimasN: 10);

                if (ultimas.Count == 0) {
                    EscribirLog("DEBUG", "No hay operaciones registradas aún.");
                } else {
                    EscribirLog("DEBUG", $"Últimas {ultimas.Count} operaciones:");
                    foreach (var op in ultimas) {
                        string detalles = op.Detalles.Length > 50 ? op.Detalles.Substring(0, 50) + "..." : op.Detalles;
                        EscribirLog("DEBUG", $"  [{op.ID}] {op.Fecha:yyyy-MM-dd HH:mm:ss} | {op.Usuario} | {op.Operacion} | {detalles}");
                    }
                }
            }
            catch (Exception ex) {
                EscribirLog("ERROR", $"Error verificando operaciones: {ex.Message}");
            }
        }

    }
}
