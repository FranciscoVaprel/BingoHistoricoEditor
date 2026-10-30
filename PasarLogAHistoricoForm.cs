using BingoHistoricoEditor.Data;
using BingoHistoricoEditor.Models;
using System.Globalization;
using static BingoHistoricoEditor.Data.FirebirdService;

namespace BingoHistoricoEditor {
    /// <summary>
    /// Compara las partidas de un día en la tabla de importación (HISTORICO_LOG,
    /// ver ImportarLogForm) con las de HISTORICO y pasa a HISTORICO las que se
    /// marquen. Por defecto se marcan las que faltan en HISTORICO, salvo las
    /// dudosas (sin cabecera o sin cartones vendidos). Si alguna marcada ya
    /// existe con datos distintos, se pregunta al pasar si sustituir la fila
    /// entera o solo rellenar los huecos (decidido con el usuario 2026-10-30).
    /// HISTORICO_LOG no se modifica.
    /// </summary>
    public sealed class PasarLogAHistoricoForm : Form {
        private readonly FirebirdService _db;
        private readonly string _tabla;
        private readonly int _codSala;
        private readonly int _codConc;
        private readonly DateTime _fechaPreferida;
        private readonly Action<string, object> _registrar;

        private ComboBox _cmbSesion = null!;
        private Button _btnComparar = null!;
        private DataGridView _grid = null!;
        private DataGridView _gridDetalle = null!;
        private Label _lblDetalle = null!;
        private Label _lblResumen = null!;
        private Button _btnMarcarFaltan = null!;
        private Button _btnDesmarcar = null!;
        private Button _btnPasar = null!;
        private Button _btnCerrar = null!;

        private const int ColPasar = 0;
        private IReadOnlyList<ColumnaHistorico> _columnasHistorico = Array.Empty<ColumnaHistorico>();

        /// <summary>Se ha escrito algo en HISTORICO (MainForm recarga su tabla).</summary>
        public bool HuboCambios { get; private set; }

        public PasarLogAHistoricoForm(FirebirdService db, string tabla, int codSala, int codConc,
                                      DateTime fechaPreferida, Action<string, object> registrar) {
            _db = db;
            _tabla = tabla.ToUpperInvariant();
            _codSala = codSala;
            _codConc = codConc;
            _fechaPreferida = fechaPreferida.Date;
            _registrar = registrar;
            BuildUI();
            Load += async (_, _) => await CargarSesionesAsync();
        }

        // ── Ventana ──────────────────────────────────────────────────────────

        private void BuildUI() {
            Text = $"Pasar de {_tabla} a HISTORICO";
            Size = new Size(1000, 700);
            MinimumSize = new Size(720, 480);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = SystemColors.Control;
            ForeColor = SystemColors.ControlText;
            Font = new Font("Segoe UI", 9f);
            ShowInTaskbar = false;

            var flpArriba = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(8, 8, 8, 2) };
            flpArriba.Controls.Add(new Label { Text = "Sesión:", AutoSize = true, Margin = new Padding(3, 7, 3, 0) });
            _cmbSesion = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 230 };
            _cmbSesion.SelectedIndexChanged += async (_, _) => await CompararAsync();
            _btnComparar = CrearBoton("↻ Volver a comparar", 160);
            _btnComparar.Click += async (_, _) => await CompararAsync();
            flpArriba.Controls.Add(_cmbSesion);
            flpArriba.Controls.Add(_btnComparar);
            flpArriba.Controls.Add(new Label {
                AutoSize = true, ForeColor = SystemColors.GrayText, Margin = new Padding(16, 7, 3, 0),
                Text = $"Sala {_codSala}, concesionario {_codConc}. {_tabla} no se modifica.",
            });

            _grid = new DataGridView {
                Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AllowUserToResizeRows = false,
                RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = SystemColors.Window,
                BorderStyle = BorderStyle.FixedSingle, EditMode = DataGridViewEditMode.EditOnEnter,
            };
            _grid.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Pasar", FillWeight = 6 });
            foreach (var (titulo, peso) in new[] {
                         ("Nº", 5), ("Estado", 20), ("Inicio", 9), ("Fin", 9), ("Bolas", 6), ("Vend.", 6),
                         ("Línea", 8), ("Bingo", 8), ("Notas", 30) }) {
                _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = titulo, FillWeight = peso, ReadOnly = true, SortMode = DataGridViewColumnSortMode.NotSortable });
            }
            _grid.Columns[ColPasar].SortMode = DataGridViewColumnSortMode.NotSortable;
            // CurrentCellChanged y no SelectionChanged: este llega antes de que cambie CurrentRow.
            _grid.CurrentCellChanged += (_, _) => MostrarDetalle();
            // Que el resumen cambie al marcar/desmarcar sin tener que salir de la celda.
            _grid.CurrentCellDirtyStateChanged += (_, _) => { if (_grid.IsCurrentCellDirty) _grid.CommitEdit(DataGridViewDataErrorContexts.Commit); };
            _grid.CellValueChanged += (_, e) => { if (e.ColumnIndex == ColPasar) ActualizarResumen(); };

            _lblDetalle = new Label { Dock = DockStyle.Top, AutoSize = false, Height = 26, Padding = new Padding(2, 6, 2, 0) };
            _gridDetalle = new DataGridView {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = SystemColors.Window,
                BorderStyle = BorderStyle.FixedSingle,
            };
            _gridDetalle.Columns.Add("campo", "Campo");
            _gridDetalle.Columns.Add("hist", "En HISTORICO");
            _gridDetalle.Columns.Add("log", $"En {_tabla}");
            _gridDetalle.Columns.Add("tipo", "");
            _gridDetalle.Columns[0].FillWeight = 22;
            _gridDetalle.Columns[1].FillWeight = 34;
            _gridDetalle.Columns[2].FillWeight = 34;
            _gridDetalle.Columns[3].FillWeight = 10;

            var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, Padding = new Padding(8, 4, 8, 0) };
            split.Panel1.Controls.Add(_grid);
            split.Panel2.Controls.Add(_gridDetalle);
            split.Panel2.Controls.Add(_lblDetalle);
            Shown += (_, _) => split.SplitterDistance = (int)(split.Height * 0.55);

            var pnlAbajo = new Panel { Dock = DockStyle.Bottom, Height = 50 };
            _lblResumen = new Label { AutoSize = true, Location = new Point(10, 17) };
            _btnMarcarFaltan = CrearBoton("Marcar las que faltan", 160);
            _btnMarcarFaltan.Click += (_, _) => Marcar(c => c.Estado == EstadoComparacion.FaltaEnHistorico && Dudosa(c) == null);
            _btnDesmarcar = CrearBoton("Desmarcar todo", 120);
            _btnDesmarcar.Click += (_, _) => Marcar(_ => false);
            _btnPasar = CrearBoton("⇧ Pasar a HISTORICO", 170);
            _btnPasar.Click += async (_, _) => await PasarAsync();
            _btnCerrar = CrearBoton("Cerrar", 100);
            _btnCerrar.Click += (_, _) => Close();
            CancelButton = _btnCerrar;
            var flpBotones = new FlowLayoutPanel {
                Dock = DockStyle.Right, AutoSize = true, WrapContents = false, Padding = new Padding(0, 8, 8, 0),
            };
            flpBotones.Controls.AddRange(new Control[] { _btnMarcarFaltan, _btnDesmarcar, _btnPasar, _btnCerrar });
            pnlAbajo.Controls.Add(_lblResumen);
            pnlAbajo.Controls.Add(flpBotones);

            Controls.Add(split);
            Controls.Add(flpArriba);
            Controls.Add(pnlAbajo);
            FormClosing += (_, e) => { if (!_btnCerrar.Enabled) e.Cancel = true; };   // trabajando: esperar
        }

        private static Button CrearBoton(string texto, int ancho) => new() {
            Text = texto, Size = new Size(ancho, 32), FlatStyle = FlatStyle.Standard,
            ForeColor = SystemColors.ControlText, UseVisualStyleBackColor = true, Cursor = Cursors.Hand,
        };

        private sealed record Sesion(DateTime Fecha, int Partidas) {
            public override string ToString() => $"{Fecha:dd/MM/yyyy}  ({Partidas} partidas)";
        }

        // ── Cargar y comparar ────────────────────────────────────────────────

        private async Task CargarSesionesAsync() {
            try {
                SetBusy(true);
                var sesiones = await _db.ObtenerSesionesAsync(_tabla, _codSala, _codConc);
                if (sesiones.Count == 0) {
                    _lblResumen.Text = $"No hay partidas de la sala {_codSala} / concesionario {_codConc} en {_tabla}. Importa antes un log de Gregal.";
                    return;
                }
                foreach (var (fecha, n) in sesiones) _cmbSesion.Items.Add(new Sesion(fecha, n));
                // El día que se estaba viendo en la pantalla principal, si está; si no, el más reciente.
                _cmbSesion.SelectedItem = _cmbSesion.Items.Cast<Sesion>().FirstOrDefault(s => s.Fecha == _fechaPreferida)
                                          ?? _cmbSesion.Items[0];
            }
            catch (Exception ex) {
                _lblResumen.Text = "Error: " + ex.Message;
            }
            finally {
                SetBusy(false);
            }
        }

        private async Task CompararAsync() {
            if (_cmbSesion.SelectedItem is not Sesion sesion) return;
            try {
                SetBusy(true);
                _columnasHistorico = await _db.ObtenerColumnasHistoricoAsync();
                var comparacion = await _db.CompararConHistoricoAsync(_tabla, _codSala, _codConc, sesion.Fecha);
                _grid.Rows.Clear();
                foreach (var c in comparacion) AñadirFila(c);
                Marcar(c => c.Estado == EstadoComparacion.FaltaEnHistorico && Dudosa(c) == null);
                MostrarDetalle();
            }
            catch (Exception ex) {
                _grid.Rows.Clear();
                _lblResumen.Text = "Error al comparar: " + ex.Message;
            }
            finally {
                SetBusy(false);
            }
        }

        private static object? Campo(ComparacionPartida c, string nombre) =>
            c.FilaLog.TryGetValue(nombre, out var v) ? v : null;

        /// <summary>Por qué no conviene pasar la partida sin mirarla, o null.</summary>
        private static string? Dudosa(ComparacionPartida c) {
            string reclamacion = (Campo(c, "RECLAMACION") as string)?.Trim() ?? "";
            if (reclamacion.StartsWith("PARTIDA A MEDIAS", StringComparison.OrdinalIgnoreCase)) return "⚠ sin cabecera (partida a medias en el log)";
            if (Campo(c, "VENDIDOS") is { } v && Convert.ToDecimal(v, CultureInfo.InvariantCulture) == 0) return "⚠ 0 cartones vendidos (¿partida de prueba?)";
            return null;
        }

        private void AñadirFila(ComparacionPartida c) {
            string estado = c.Estado switch {
                EstadoComparacion.FaltaEnHistorico => "Falta en HISTORICO",
                EstadoComparacion.Igual => "Igual",
                _ => $"Distinta ({c.Diferencias.Count} {(c.Diferencias.Count == 1 ? "campo" : "campos")}" +
                     $"{(c.Huecos == 1 ? ", 1 hueco" : c.Huecos > 1 ? $", {c.Huecos} huecos" : "")})",
            };
            string Hora(string col) => Campo(c, col) is { } v ? FirebirdService.AHora(v).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture) : "";
            string Importe(string col) => Campo(c, col) is { } v ? Convert.ToDecimal(v, CultureInfo.InvariantCulture).ToString("0.00", CultureInfo.CurrentCulture) : "";
            string Entero(string col) => Campo(c, col) is { } v ? Convert.ToDecimal(v, CultureInfo.InvariantCulture).ToString("0", CultureInfo.CurrentCulture) : "";
            string notas = string.Join("  ", new[] { Dudosa(c), (Campo(c, "RECLAMACION") as string)?.Trim() }
                .Where(s => !string.IsNullOrEmpty(s)).Distinct());

            int i = _grid.Rows.Add(false, c.Clave.PartidaDia, estado, Hora("HORA_VENTA"), Hora("HORA_FIN"),
                                   Entero("EXTRACCIONES"), Entero("VENDIDOS"), Importe("PREMIO1"), Importe("PREMIO2"), notas);
            var fila = _grid.Rows[i];
            fila.Tag = c;
            switch (c.Estado) {
                case EstadoComparacion.Igual:
                    fila.DefaultCellStyle.ForeColor = SystemColors.GrayText;
                    fila.Cells[ColPasar].ReadOnly = true;   // no hay nada que pasar
                    break;
                case EstadoComparacion.Distinta:
                    fila.Cells[2].Style.BackColor = SystemColors.Info;
                    break;
                default:
                    fila.Cells[2].Style.BackColor = Color.FromArgb(220, 240, 220);
                    break;
            }
            if (Dudosa(c) != null) fila.Cells[9].Style.ForeColor = Color.DarkRed;
        }

        private IEnumerable<(DataGridViewRow Fila, ComparacionPartida C)> Filas =>
            _grid.Rows.Cast<DataGridViewRow>().Select(f => (f, (ComparacionPartida)f.Tag!));

        private List<ComparacionPartida> Marcadas =>
            Filas.Where(x => x.Fila.Cells[ColPasar].Value is true && x.C.Estado != EstadoComparacion.Igual)
                 .Select(x => x.C).ToList();

        private void Marcar(Func<ComparacionPartida, bool> criterio) {
            _grid.EndEdit();
            foreach (var (fila, c) in Filas) fila.Cells[ColPasar].Value = c.Estado != EstadoComparacion.Igual && criterio(c);
            ActualizarResumen();
        }

        private void ActualizarResumen() {
            var todas = Filas.Select(x => x.C).ToList();
            var marcadas = Marcadas;
            int faltan = todas.Count(c => c.Estado == EstadoComparacion.FaltaEnHistorico);
            int distintas = todas.Count(c => c.Estado == EstadoComparacion.Distinta);
            _lblResumen.Text = $"{todas.Count} partidas: {faltan} faltan en HISTORICO, {distintas} distintas, " +
                               $"{todas.Count - faltan - distintas} iguales.   Marcadas: {marcadas.Count}";
            _btnPasar.Enabled = marcadas.Count > 0 && _btnCerrar.Enabled;
        }

        private void MostrarDetalle() {
            _gridDetalle.Rows.Clear();
            if (_grid.CurrentRow?.Tag is not ComparacionPartida c) { _lblDetalle.Text = ""; return; }

            switch (c.Estado) {
                case EstadoComparacion.Igual:
                    _lblDetalle.Text = $"Partida {c.Clave.PartidaDia}: igual en las dos tablas, no hay nada que pasar.";
                    break;
                case EstadoComparacion.Distinta:
                    _lblDetalle.Text = $"Partida {c.Clave.PartidaDia}: campos distintos (hueco = vacío o 0 en HISTORICO y con valor en {_tabla}).";
                    foreach (var d in c.Diferencias) {
                        int i = _gridDetalle.Rows.Add(d.Columna.Nombre,
                            FichaPartidaCampos.Formatear(d.Columna, d.EnHistorico),
                            FichaPartidaCampos.Formatear(d.Columna, d.EnLog),
                            d.EsHueco ? "hueco" : "");
                        if (d.EsHueco) _gridDetalle.Rows[i].DefaultCellStyle.BackColor = SystemColors.Info;
                    }
                    break;
                default:
                    _lblDetalle.Text = $"Partida {c.Clave.PartidaDia}: no está en HISTORICO; se añadiría con estos valores (campos vacíos o a 0 no se muestran).";
                    foreach (var col in _columnasHistorico.Where(x => c.FilaLog.ContainsKey(x.Nombre))) {
                        var v = FichaPartidaCampos.Normalizar(col, c.FilaLog[col.Nombre]);
                        if (v is null || v is decimal dv && dv == 0 || v is string s && s.Length == 0) continue;
                        _gridDetalle.Rows.Add(col.Nombre, "(no existe)", FichaPartidaCampos.Formatear(col, c.FilaLog[col.Nombre]), "");
                    }
                    break;
            }
        }

        // ── Pasar ────────────────────────────────────────────────────────────

        private async Task PasarAsync() {
            _grid.EndEdit();
            var marcadas = Marcadas;
            if (marcadas.Count == 0) return;
            int faltan = marcadas.Count(c => c.Estado == EstadoComparacion.FaltaEnHistorico);
            var distintas = marcadas.Where(c => c.Estado == EstadoComparacion.Distinta).ToList();
            int dudosas = marcadas.Count(c => Dudosa(c) != null);

            const string aviso = "Lo que se escriba en HISTORICO entra en la replicación de la sala y en la auditoría, " +
                                 "como cualquier otro cambio.";
            string extra = dudosas > 0 ? $"\n\n⚠ {dudosas} de las marcadas son dudosas (ver Notas)." : "";
            bool soloHuecos = false;

            if (distintas.Count > 0) {
                var sustituir = new TaskDialogButton("Sustituir la fila entera");
                var rellenar  = new TaskDialogButton("Rellenar solo los huecos");
                var pagina = new TaskDialogPage {
                    Caption = Text,
                    Icon = TaskDialogIcon.Warning,
                    Heading = $"{distintas.Count} de las partidas marcadas ya existen en HISTORICO con datos distintos",
                    Text = "¿Qué se hace con ellas?\n\n" +
                           $"• Sustituir la fila entera: quedan igual que en {_tabla} " +
                           $"({distintas.Sum(c => c.Diferencias.Count)} campos en total).\n" +
                           "• Rellenar solo los huecos: solo se escriben los campos que en HISTORICO están vacíos o a 0 " +
                           $"({distintas.Sum(c => c.Huecos)} campos en total); lo que ya tiene valor no se toca." +
                           (faltan > 0 ? $"\n\nAdemás se añadirán {faltan} partidas que faltan en HISTORICO." : "") +
                           extra + "\n\n" + aviso,
                    Buttons = { sustituir, rellenar, TaskDialogButton.Cancel },
                    DefaultButton = TaskDialogButton.Cancel,
                };
                var r = TaskDialog.ShowDialog(this, pagina);
                if (r == rellenar) soloHuecos = true;
                else if (r != sustituir) return;
            }
            else {
                var r = MessageBox.Show(this, $"Se añadirán {faltan} partidas a HISTORICO.{extra}\n\n{aviso}\n\n¿Continuar?",
                    Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (r != DialogResult.Yes) return;
            }

            try {
                SetBusy(true);
                var res = await _db.PasarAHistoricoAsync(marcadas.Select(c => new PasoAHistorico(c, soloHuecos)).ToList());
                HuboCambios = true;

                string resumen = $"{res.Añadidas} añadidas" +
                                 (res.Sustituidas > 0 ? $", {res.Sustituidas} sustituidas" : "") +
                                 (res.ConHuecosRellenados > 0 ? $", {res.ConHuecosRellenados} con huecos rellenados" : "") +
                                 (res.SinHuecos > 0 ? $", {res.SinHuecos} sin huecos que rellenar (no se han tocado)" : "");
                var fecha = marcadas[0].Clave.FechaSesion;
                _registrar($"De {_tabla} a HISTORICO, sesión {fecha:dd/MM/yyyy}: {resumen}.", new {
                    Tabla = _tabla, Sesion = fecha.ToString("yyyy-MM-dd"), Sala = _codSala, Concesionario = _codConc,
                    Modo = distintas.Count == 0 ? null : soloHuecos ? "rellenar huecos" : "sustituir fila",
                    Partidas = marcadas.Select(c => new {
                        Partida = c.Clave.PartidaDia,
                        Accion = c.Estado == EstadoComparacion.FaltaEnHistorico ? "añadida" : soloHuecos ? "huecos" : "sustituida",
                        Campos = c.Estado == EstadoComparacion.FaltaEnHistorico ? null
                            : c.Diferencias.Where(d => !soloHuecos || d.EsHueco).Select(d => new {
                                Campo = d.Columna.Nombre,
                                Antes = FichaPartidaCampos.Formatear(d.Columna, d.EnHistorico),
                                Despues = FichaPartidaCampos.Formatear(d.Columna, d.EnLog),
                            }).ToList(),
                    }).ToList(),
                });
                MessageBox.Show(this, $"Hecho: {resumen}.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (InvalidOperationException ex) {
                MessageBox.Show(this, ex.Message, "No se ha pasado nada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                SetBusy(false);
            }
            await CompararAsync();
        }

        private void SetBusy(bool busy) {
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            _cmbSesion.Enabled = !busy;
            _btnComparar.Enabled = !busy;
            _btnMarcarFaltan.Enabled = !busy;
            _btnDesmarcar.Enabled = !busy;
            _btnCerrar.Enabled = !busy;
            _btnPasar.Enabled = !busy && Marcadas.Count > 0;
        }
    }
}
