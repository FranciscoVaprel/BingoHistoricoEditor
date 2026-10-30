using BingoHistoricoEditor.Data;
using BingoHistoricoEditor.Models;
using System.Globalization;
using System.Text;

namespace BingoHistoricoEditor {
    /// <summary>
    /// Ficha con TODAS las columnas de una partida de HISTORICO, agrupadas en
    /// pestañas (ver FichaPartidaCampos). Se construye a partir de las columnas
    /// que tiene la BD de la sala, con el control adecuado a cada tipo. Los 99
    /// PREMIOn van en una rejilla aparte. Al guardar solo se escriben los campos
    /// modificados, tras confirmar un resumen de los cambios.
    ///
    /// La clave (COD_SALA, COD_CONC, FECHA_SESION, PARTIDA_DIA) también se puede
    /// cambiar —decidido con el usuario—: eso mueve la partida, así que se avisa
    /// y no se guarda si ya hay otra partida con la clave nueva.
    ///
    /// No escribe en el log: quien la abre (MainForm) registra
    /// <see cref="CambiosGuardados"/> si se cierra con DialogResult.OK.
    /// </summary>
    public sealed class FichaPartidaForm : Form {
        private readonly FirebirdService _db;
        private readonly ClaveHistorico _clave;
        private readonly ToolTip _ayuda = new() { AutoPopDelay = 15000 };

        private readonly List<EditorCampo> _editoresClave = new();
        private readonly List<(TabPage Pagina, string Nombre, TableLayoutPanel Tabla, List<EditorCampo> Editores)> _paginas = new();
        private readonly List<FilaPremio> _premios = new();

        private TextBox _txtBuscar = null!;
        private CheckBox _chkSoloModificados = null!;
        private CheckBox? _chkSoloConValor;
        private TabControl _tabs = null!;
        private TabPage? _paginaPremios;
        private DataGridView? _gridPremios;
        private Label _lblAvisoClave = null!;
        private Label _lblCambios = null!;
        private Button _btnDeshacer = null!;
        private Button _btnGuardar = null!;
        private Button _btnCerrar = null!;
        private Font _fuenteNegrita = null!;
        private bool _guardado;

        /// <summary>Campos guardados (solo si la ficha se cerró con OK).</summary>
        public IReadOnlyList<CambioCampo> CambiosGuardados { get; private set; } = Array.Empty<CambioCampo>();

        /// <summary>Clave de la partida tras guardar (distinta de la original si se cambió).</summary>
        public ClaveHistorico ClaveFinal { get; private set; }

        /// <param name="soloConsultaDe">Si se indica (nombre de la tabla, p. ej. HISTORICO_LOG), la
        /// ficha solo muestra los datos: nada se puede editar ni guardar.</param>
        public FichaPartidaForm(FirebirdService db, IReadOnlyList<ColumnaHistorico> columnas,
                                ClaveHistorico clave, IReadOnlyDictionary<string, object?> fila,
                                string? soloConsultaDe = null) {
            _db = db;
            _clave = clave;
            ClaveFinal = clave;
            BuildUI(columnas.Where(c => fila.ContainsKey(c.Nombre)).ToList(), fila);
            ActualizarEstado();
            if (soloConsultaDe != null) PonerSoloConsulta(soloConsultaDe);
        }

        private void PonerSoloConsulta(string tabla) {
            Text += $"  ·  {tabla} (solo consulta)";
            foreach (var ed in _editoresClave.Concat(_paginas.SelectMany(p => p.Editores))) {
                ed.Entrada.Enabled = false;
                if (ed.Nulo != null) ed.Nulo.Enabled = false;
            }
            if (_gridPremios != null) _gridPremios.ReadOnly = true;
            _btnGuardar.Visible = false;
            _btnDeshacer.Visible = false;
            _lblCambios.Text = $"Solo consulta: datos de {tabla}.";
        }

        private IEnumerable<ICampoFicha> TodosLosCampos =>
            _editoresClave.Cast<ICampoFicha>()
                .Concat(_paginas.SelectMany(p => p.Editores))
                .Concat(_premios);

        // ── Construcción de la ventana ───────────────────────────────────────

        private void BuildUI(IReadOnlyList<ColumnaHistorico> columnas, IReadOnlyDictionary<string, object?> fila) {
            Text = $"Ficha de la partida {_clave.PartidaDia} — {_clave.FechaSesion:dd/MM/yyyy}";
            Size = new Size(880, 700);
            MinimumSize = new Size(680, 480);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = SystemColors.Control;
            ForeColor = SystemColors.ControlText;
            Font = new Font("Segoe UI", 9f);
            _fuenteNegrita = new Font(Font, FontStyle.Bold);
            ShowInTaskbar = false;
            KeyPreview = true;
            KeyDown += (_, e) => {
                if (e.Control && e.KeyCode == Keys.F) { _txtBuscar.Focus(); _txtBuscar.SelectAll(); e.SuppressKeyPress = true; }
            };

            SuspendLayout();

            object? Valor(ColumnaHistorico c) => fila.TryGetValue(c.Nombre, out var v) ? v : null;

            // Clave, siempre visible encima de las pestañas.
            var flpClave = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true, Padding = new Padding(4) };
            foreach (var col in columnas.Where(c => c.EsClave)) {
                var ed = new EditorCampo(col, Valor(col), _ayuda);
                ed.Etiqueta.Dock = DockStyle.None;
                ed.Etiqueta.AutoSize = true;
                ed.Etiqueta.Margin = new Padding(6, 7, 2, 0);
                ed.Entrada.Width = col.Tipo == TipoColumna.Fecha ? 110 : 80;
                ed.Entrada.Margin = new Padding(0, 3, 12, 3);
                ed.Cambiado += (_, _) => ActualizarEstado();
                flpClave.Controls.Add(ed.Etiqueta);
                flpClave.Controls.Add(ed.Entrada);
                _editoresClave.Add(ed);
            }
            _lblAvisoClave = new Label {
                AutoSize = true, Visible = false, ForeColor = Color.DarkRed,
                Margin = new Padding(6, 4, 6, 4),
            };
            flpClave.Controls.Add(_lblAvisoClave);
            flpClave.SetFlowBreak(flpClave.Controls[^2], true);

            var grpClave = new GroupBox {
                Text = " Clave de la partida ", Dock = DockStyle.Top, AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(6, 3, 6, 6),
            };
            grpClave.Controls.Add(flpClave);
            _ayuda.SetToolTip(grpClave,
                "Cambiar la clave mueve la partida a otra sala, concesionario, día o número.\n" +
                "Al guardar se comprueba que no exista ya otra partida con la clave nueva.");

            // Buscador.
            var flpBuscar = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4, 6, 4, 2) };
            flpBuscar.Controls.Add(new Label { Text = "Buscar campo:", AutoSize = true, Margin = new Padding(3, 6, 3, 0) });
            _txtBuscar = new TextBox { Width = 220, PlaceholderText = "nombre o descripción (Ctrl+F)" };
            _txtBuscar.TextChanged += (_, _) => AplicarFiltro();
            _chkSoloModificados = new CheckBox { Text = "Solo modificados", AutoSize = true, Margin = new Padding(16, 5, 3, 0) };
            _chkSoloModificados.CheckedChanged += (_, _) => AplicarFiltro();
            flpBuscar.Controls.AddRange(new Control[] { _txtBuscar, _chkSoloModificados });

            // Pestañas.
            _tabs = new TabControl { Dock = DockStyle.Fill };
            var noClave = columnas.Where(c => !c.EsClave).ToList();
            foreach (string nombre in FichaPartidaCampos.Pestañas) {
                var cols = noClave.Where(c => FichaPartidaCampos.Pestaña(c) == nombre).ToList();
                if (cols.Count == 0) continue;
                if (nombre == FichaPartidaCampos.PestañaPremios) CrearPaginaPremios(cols, Valor);
                else CrearPagina(nombre, cols, Valor);
            }

            // Botones.
            var pnlAbajo = new Panel { Dock = DockStyle.Bottom, Height = 48 };
            _lblCambios = new Label { AutoSize = true, Location = new Point(10, 16) };
            _btnCerrar = CrearBoton("Cerrar", 100);
            _btnCerrar.Click += (_, _) => Close();
            _btnGuardar = CrearBoton("💾 Guardar", 120);
            _btnGuardar.Click += async (_, _) => await GuardarAsync();
            _btnDeshacer = CrearBoton("↶ Deshacer todo", 130);
            _btnDeshacer.Click += (_, _) => DeshacerTodo();
            _ayuda.SetToolTip(_btnDeshacer, "Vuelve a poner en todos los campos el valor que tienen en la base de datos.");
            _ayuda.SetToolTip(_btnGuardar, "Guarda solo los campos modificados, después de mostrar un resumen para confirmar.");
            var flpBotones = new FlowLayoutPanel {
                Dock = DockStyle.Right, AutoSize = true, FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 7, 6, 0), WrapContents = false,
            };
            flpBotones.Controls.AddRange(new Control[] { _btnDeshacer, _btnGuardar, _btnCerrar });
            pnlAbajo.Controls.Add(_lblCambios);
            pnlAbajo.Controls.Add(flpBotones);
            CancelButton = _btnCerrar;

            // El que ocupa el resto (Fill) se añade primero para que se acople después.
            Controls.Add(_tabs);
            Controls.Add(flpBuscar);
            Controls.Add(grpClave);
            Controls.Add(pnlAbajo);

            FormClosing += AlCerrar;
            ResumeLayout(true);
        }

        private static Button CrearBoton(string texto, int ancho) => new() {
            Text = texto, Size = new Size(ancho, 32), FlatStyle = FlatStyle.Standard,
            ForeColor = SystemColors.ControlText, UseVisualStyleBackColor = true, Cursor = Cursors.Hand,
        };

        private void CrearPagina(string nombre, List<ColumnaHistorico> columnas, Func<ColumnaHistorico, object?> valor) {
            // Ancho fijo para que la casilla "vacío" quede junto a su campo y no
            // al otro lado de la ventana.
            var tabla = new TableLayoutPanel {
                Location = new Point(0, 0), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 3, Padding = new Padding(8, 6, 16, 6),
            };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 440));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tabla.SuspendLayout();

            var editores = new List<EditorCampo>();
            foreach (var col in columnas) {
                var ed = new EditorCampo(col, valor(col), _ayuda);
                ed.Cambiado += (_, _) => ActualizarEstado();
                int fila = tabla.RowCount++;
                tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                ed.Etiqueta.Margin = new Padding(0, 2, 6, 2);
                tabla.Controls.Add(ed.Etiqueta, 0, fila);
                tabla.Controls.Add(ed.Entrada, 1, fila);
                if (ed.Nulo != null) tabla.Controls.Add(ed.Nulo, 2, fila);
                editores.Add(ed);
            }
            tabla.ResumeLayout(false);

            var desplazable = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            desplazable.Controls.Add(tabla);
            var pagina = new TabPage(nombre) { UseVisualStyleBackColor = true };
            pagina.Controls.Add(desplazable);
            _tabs.TabPages.Add(pagina);
            _paginas.Add((pagina, nombre, tabla, editores));
        }

        // ── Premios: rejilla Nº / Campo / Descripción / Valor ────────────────

        private const int ColValor = 3;

        private void CrearPaginaPremios(List<ColumnaHistorico> columnas, Func<ColumnaHistorico, object?> valor) {
            _gridPremios = new DataGridView {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AllowUserToOrderColumns = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                MultiSelect = false,
                EditMode = DataGridViewEditMode.EditOnEnter,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Window,
                BorderStyle = BorderStyle.FixedSingle,
            };
            _gridPremios.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nº", ReadOnly = true, FillWeight = 8, SortMode = DataGridViewColumnSortMode.NotSortable });
            _gridPremios.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Campo", ReadOnly = true, FillWeight = 16, SortMode = DataGridViewColumnSortMode.NotSortable });
            _gridPremios.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Descripción", ReadOnly = true, FillWeight = 56, SortMode = DataGridViewColumnSortMode.NotSortable });
            _gridPremios.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Valor", FillWeight = 20, SortMode = DataGridViewColumnSortMode.NotSortable,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight },
            });
            foreach (DataGridViewColumn c in _gridPremios.Columns) {
                if (c.ReadOnly) c.DefaultCellStyle.ForeColor = SystemColors.GrayText;
            }

            foreach (var col in columnas.OrderBy(c => FichaPartidaCampos.EsPremio(c, out int n) ? n : int.MaxValue)) {
                FichaPartidaCampos.EsPremio(col, out int numero);
                var premio = new FilaPremio(col, numero, valor(col));
                int i = _gridPremios.Rows.Add(numero, col.Nombre, FichaPartidaCampos.Descripcion(col.Nombre) ?? "", premio.Texto);
                var fila = _gridPremios.Rows[i];
                fila.Tag = premio;
                fila.Cells[ColValor].ToolTipText = FichaPartidaCampos.Ayuda(col) + "\nDeja la celda vacía para guardarlo sin valor (NULL).";
                _premios.Add(premio);
            }

            _gridPremios.CellValidating += GridPremios_CellValidating;
            _gridPremios.CellEndEdit += (_, e) => {
                var fila = _gridPremios.Rows[e.RowIndex];
                fila.ErrorText = "";
                if (fila.Tag is FilaPremio p) fila.Cells[ColValor].Value = p.Texto;
                EstiloPremio(fila);
                ActualizarEstado();
            };

            _chkSoloConValor = new CheckBox { Text = "Solo con valor (distinto de 0 y de vacío)", AutoSize = true, Margin = new Padding(3, 5, 3, 3) };
            _chkSoloConValor.CheckedChanged += (_, _) => AplicarFiltro();
            var flpPremios = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4, 4, 4, 2) };
            flpPremios.Controls.Add(_chkSoloConValor);
            flpPremios.Controls.Add(new Label {
                Text = "Escribe en la columna Valor para cambiar un premio; vacía = sin valor (NULL).",
                AutoSize = true, ForeColor = SystemColors.GrayText, Margin = new Padding(16, 6, 3, 3),
            });

            _paginaPremios = new TabPage(FichaPartidaCampos.PestañaPremios) { UseVisualStyleBackColor = true };
            _paginaPremios.Controls.Add(_gridPremios);
            _paginaPremios.Controls.Add(flpPremios);
            _tabs.TabPages.Add(_paginaPremios);
        }

        private void GridPremios_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e) {
            if (_gridPremios == null || e.ColumnIndex != ColValor) return;
            var fila = _gridPremios.Rows[e.RowIndex];
            if (fila.Tag is not FilaPremio premio) return;

            string? error = premio.Asignar(Convert.ToString(e.FormattedValue, CultureInfo.CurrentCulture) ?? "");
            fila.ErrorText = error ?? "";
            if (error != null) e.Cancel = true;
        }

        private void EstiloPremio(DataGridViewRow fila) {
            if (fila.Tag is not FilaPremio p) return;
            var celda = fila.Cells[ColValor];
            celda.Style.BackColor = p.Modificado ? SystemColors.Info : Color.Empty;
            celda.Style.Font = p.Modificado ? _fuenteNegrita : null;
        }

        // ── Filtro ───────────────────────────────────────────────────────────

        private bool Coincide(ICampoFicha c) {
            string t = _txtBuscar.Text.Trim();
            bool porTexto = t.Length == 0
                || c.Columna.Nombre.Contains(t, StringComparison.OrdinalIgnoreCase)
                || (FichaPartidaCampos.Descripcion(c.Columna.Nombre)?.Contains(t, StringComparison.OrdinalIgnoreCase) ?? false);
            return porTexto && (!_chkSoloModificados.Checked || c.Modificado);
        }

        private void AplicarFiltro() {
            bool filtrando = _txtBuscar.Text.Trim().Length > 0 || _chkSoloModificados.Checked;
            var visiblesPorPagina = new Dictionary<TabPage, int>();

            foreach (var (pagina, nombre, tabla, editores) in _paginas) {
                tabla.SuspendLayout();
                int visibles = 0;
                foreach (var ed in editores) {
                    bool v = Coincide(ed);
                    ed.MostrarFila(v);
                    if (v) visibles++;
                }
                tabla.ResumeLayout(true);
                pagina.Text = filtrando ? $"{nombre} ({visibles})" : nombre;
                visiblesPorPagina[pagina] = visibles;
            }

            if (_gridPremios != null && _paginaPremios != null) {
                // No se puede ocultar la fila de la celda actual; si lo escrito en
                // ella no es válido, se descarta para poder salir de la celda.
                if (!_gridPremios.EndEdit()) _gridPremios.CancelEdit();
                _gridPremios.CurrentCell = null;
                bool soloConValor = _chkSoloConValor?.Checked == true;
                int visibles = 0;
                foreach (DataGridViewRow fila in _gridPremios.Rows) {
                    var p = (FilaPremio)fila.Tag!;
                    bool v = Coincide(p) && (!soloConValor || p.Valor is decimal d && d != 0);
                    fila.Visible = v;
                    if (v) visibles++;
                }
                bool filtrandoPremios = filtrando || soloConValor;
                _paginaPremios.Text = filtrandoPremios ? $"{FichaPartidaCampos.PestañaPremios} ({visibles})" : FichaPartidaCampos.PestañaPremios;
                visiblesPorPagina[_paginaPremios] = visibles;
            }

            // Si la pestaña abierta se ha quedado vacía, saltar a la primera que tenga algo.
            if (filtrando && _tabs.SelectedTab is TabPage actual
                && visiblesPorPagina.TryGetValue(actual, out int enActual) && enActual == 0) {
                var conAlgo = _tabs.TabPages.Cast<TabPage>()
                    .FirstOrDefault(p => visiblesPorPagina.TryGetValue(p, out int n) && n > 0);
                if (conAlgo != null) _tabs.SelectedTab = conAlgo;
            }
        }

        // ── Estado, deshacer y guardar ────────────────────────────────────────

        private ClaveHistorico ClaveSegunEditores() {
            var cambios = _editoresClave.Where(e => e.Modificado)
                .Select(e => new CambioCampo(e.Columna, e.ValorBd, e.Valor));
            return FirebirdService.AplicarCambiosDeClave(_clave, cambios);
        }

        private void ActualizarEstado() {
            int n = TodosLosCampos.Count(c => c.Modificado);
            _lblCambios.Text = n switch { 0 => "Sin cambios", 1 => "1 campo modificado", _ => $"{n} campos modificados" };
            _lblCambios.Font = n > 0 ? _fuenteNegrita : Font;
            _btnDeshacer.Enabled = n > 0;

            var claveNueva = ClaveSegunEditores();
            _lblAvisoClave.Visible = claveNueva != _clave;
            if (_lblAvisoClave.Visible) {
                _lblAvisoClave.Text = $"⚠ Al guardar, esta partida pasará a ser la {claveNueva}. " +
                                      "Se comprobará antes que no exista ya otra con esa clave.";
            }
        }

        private void DeshacerTodo() {
            _gridPremios?.CancelEdit();
            _gridPremios?.EndEdit();
            foreach (var c in TodosLosCampos.Where(c => c.Modificado).ToList()) c.Deshacer();
            if (_gridPremios != null) {
                foreach (DataGridViewRow fila in _gridPremios.Rows) {
                    fila.ErrorText = "";
                    fila.Cells[ColValor].Value = ((FilaPremio)fila.Tag!).Texto;
                    EstiloPremio(fila);
                }
            }
            ActualizarEstado();
            AplicarFiltro();
        }

        private async Task GuardarAsync() {
            // Cerrar la edición en curso: una celda de premio no válida bloquea el guardado.
            if (_gridPremios != null && !_gridPremios.EndEdit()) return;
            foreach (var ed in _editoresClave.Concat(_paginas.SelectMany(p => p.Editores))) ed.ConfirmarEdicion();

            var cambios = TodosLosCampos.Where(c => c.Modificado)
                .Select(c => new CambioCampo(c.Columna, c.ValorBd, c.Valor))
                .ToList();
            if (cambios.Count == 0) {
                MessageBox.Show("No hay cambios que guardar.", "Ficha de partida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var claveNueva = FirebirdService.AplicarCambiosDeClave(_clave, cambios);
            try {
                SetBusy(true);
                if (claveNueva != _clave && await _db.ExistePartidaAsync(claveNueva)) {
                    MessageBox.Show($"Ya existe la {claveNueva}.\n\nNo se puede mover esta partida ahí sin pisarla. " +
                                    "Cambia la clave o elimina antes esa otra partida.",
                                    "Clave ocupada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"No se pudo comprobar la clave nueva:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally {
                SetBusy(false);
            }

            if (!ConfirmarCambios(cambios, claveNueva)) return;

            try {
                SetBusy(true);
                await _db.ActualizarCamposAsync(_clave, cambios);
            }
            catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message, "No se ha guardado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception ex) {
                MessageBox.Show($"Error al guardar:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally {
                SetBusy(false);
            }

            CambiosGuardados = cambios;
            ClaveFinal = claveNueva;
            _guardado = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ConfirmarCambios(IReadOnlyList<CambioCampo> cambios, ClaveHistorico claveNueva) {
            var sb = new StringBuilder();
            if (claveNueva != _clave) {
                sb.AppendLine("⚠ CAMBIA LA CLAVE: la partida se mueve.");
                sb.AppendLine($"   Antes:   {_clave}");
                sb.AppendLine($"   Después: {claveNueva}");
                sb.AppendLine();
            }
            sb.AppendLine(cambios.Count == 1 ? "Se va a guardar 1 campo:" : $"Se van a guardar {cambios.Count} campos:");
            sb.AppendLine();
            int ancho = cambios.Max(c => c.Columna.Nombre.Length);
            foreach (var c in cambios) {
                sb.AppendLine($"  {c.Columna.Nombre.PadRight(ancho)}  {FichaPartidaCampos.Formatear(c.Columna, c.Antes)}  →  " +
                              FichaPartidaCampos.Formatear(c.Columna, c.Despues));
            }

            using var dlg = new Form {
                Text = "Confirmar cambios", Size = new Size(640, 420), MinimumSize = new Size(420, 260),
                StartPosition = FormStartPosition.CenterParent, MinimizeBox = false, MaximizeBox = false,
                ShowInTaskbar = false, Font = Font,
            };
            var txt = new TextBox {
                Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Both, WordWrap = false,
                Dock = DockStyle.Fill, Font = new Font("Consolas", 9.5f), Text = sb.ToString(),
                BackColor = SystemColors.Window,
            };
            var btnSi = CrearBoton("💾 Guardar", 120);
            btnSi.DialogResult = DialogResult.OK;
            var btnNo = CrearBoton("Cancelar", 100);
            btnNo.DialogResult = DialogResult.Cancel;
            var flp = new FlowLayoutPanel {
                Dock = DockStyle.Bottom, Height = 46, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 6, 6, 0),
            };
            flp.Controls.AddRange(new Control[] { btnNo, btnSi });
            dlg.Controls.Add(txt);
            dlg.Controls.Add(flp);
            dlg.CancelButton = btnNo;
            dlg.Shown += (_, _) => btnNo.Focus();   // por defecto, no guardar con un Intro despistado
            return dlg.ShowDialog(this) == DialogResult.OK;
        }

        private void SetBusy(bool busy) {
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            _btnGuardar.Enabled = !busy;
            _btnCerrar.Enabled = !busy;
            _btnDeshacer.Enabled = !busy && TodosLosCampos.Any(c => c.Modificado);
        }

        private void AlCerrar(object? sender, FormClosingEventArgs e) {
            if (_guardado) return;
            if (!_btnCerrar.Enabled) { e.Cancel = true; return; }   // guardando: esperar
            _gridPremios?.CancelEdit();
            int n = TodosLosCampos.Count(c => c.Modificado);
            if (n == 0) return;
            var r = MessageBox.Show($"Hay {(n == 1 ? "1 campo modificado" : $"{n} campos modificados")} sin guardar.\n\n¿Cerrar la ficha y descartarlos?",
                                    "Cambios sin guardar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (r != DialogResult.Yes) e.Cancel = true;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                _ayuda.Dispose();
                _fuenteNegrita?.Dispose();
            }
            base.Dispose(disposing);
        }

        // ── Un PREMIOn en la rejilla ─────────────────────────────────────────

        private sealed class FilaPremio : ICampoFicha {
            private readonly object? _inicial;

            public FilaPremio(ColumnaHistorico columna, int numero, object? valorBd) {
                Columna = columna;
                Numero = numero;
                ValorBd = valorBd;
                _inicial = FichaPartidaCampos.Normalizar(columna, valorBd);
                Valor = _inicial;
            }

            public ColumnaHistorico Columna { get; }
            public int Numero { get; }
            public object? ValorBd { get; }
            public object? Valor { get; private set; }
            public bool Modificado => !FichaPartidaCampos.Iguales(Valor, _inicial);

            public string Texto => Valor is decimal d
                ? (Columna.Tipo == TipoColumna.Entero ? d.ToString("0", CultureInfo.CurrentCulture) : FichaPartidaCampos.FormatearImporte(d, Columna.Escala))
                : "";

            public void Deshacer() => Valor = _inicial;

            /// <summary>Interpreta lo escrito en la celda. Devuelve el error, o null si es válido (y lo guarda).</summary>
            public string? Asignar(string texto) {
                texto = texto.Trim();
                if (texto.Length == 0) {
                    if (!Columna.AdmiteNulo) return $"{Columna.Nombre} no puede quedar vacío.";
                    Valor = null;
                    return null;
                }

                // Con coma decimal (es-ES), "2.5" se leería como 25 (punto = miles):
                // un punto sin ninguna coma se toma como separador decimal.
                var nf = CultureInfo.CurrentCulture.NumberFormat;
                if (nf.NumberDecimalSeparator == "," && texto.Contains('.') && !texto.Contains(','))
                    texto = texto.Replace('.', ',');

                if (!decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal d))
                    return $"\"{texto}\" no es un número.";
                if (d < Columna.Minimo || d > Columna.Maximo)
                    return $"Fuera de rango ({Columna.Minimo} a {Columna.Maximo}).";
                if (decimal.Round(d, Columna.Escala) != d)
                    return Columna.Escala == 0 ? "Tiene que ser un número entero." : $"Como mucho {Columna.Escala} decimales.";

                Valor = d;
                return null;
            }
        }
    }
}
