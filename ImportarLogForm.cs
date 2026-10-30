using BingoHistoricoEditor.Data;
using Gregal.Config;
using Gregal.Parsing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BingoHistoricoEditor {
    /// <summary>
    /// Importa todas las partidas de un log de Gregal (gregal&lt;AAAAMMDD&gt;.log)
    /// a una tabla aparte con la estructura de HISTORICO (HISTORICO_LOG por
    /// defecto, ver FirebirdService.CrearTablaComoHistoricoAsync), nunca a
    /// HISTORICO. Las partidas se reconstruyen con el mismo código que usa
    /// Gregal (librería Gregal.Protocolo: ReproductorLogGregal), así que salen
    /// igual que las habría guardado el servicio, con las horas del log.
    ///
    /// Sala y concesionario: los de la configuración del editor. Fabricante:
    /// se elige aquí (el log no lo dice) y se recuerda para la próxima vez.
    /// </summary>
    public sealed class ImportarLogForm : Form {
        private readonly FirebirdService _db;
        private readonly string _tabla;
        private readonly int _codSala;
        private readonly int _codConc;
        private readonly Action<string, object> _registrar;

        /// <summary>
        /// Día de sesión más reciente entre las partidas importadas (null si no
        /// se ha importado nada): MainForm salta a él en la pestaña del log.
        /// </summary>
        public DateTime? UltimaSesionImportada { get; private set; }

        // appsettings.json se lee una vez al arrancar: el último fabricante
        // elegido en esta ejecución se recuerda también aquí.
        private static string? _ultimoFabricante;

        private TextBox _txtRuta = null!;
        private ComboBox _cmbFabricante = null!;
        private TextBox _txtResultado = null!;
        private Button _btnExaminar = null!;
        private Button _btnImportar = null!;
        private Button _btnCerrar = null!;

        /// <param name="registrar">Anota cada importación en el log y la auditoría (detalle, datos).</param>
        public ImportarLogForm(FirebirdService db, string tabla, int codSala, int codConc,
                               string? fabricanteGuardado, Action<string, object> registrar) {
            _db = db;
            _tabla = tabla.ToUpperInvariant();
            _codSala = codSala;
            _codConc = codConc;
            _registrar = registrar;
            BuildUI(_ultimoFabricante ?? fabricanteGuardado);
        }

        private void BuildUI(string? fabricante) {
            Text = "Importar log de Gregal";
            Size = new Size(960, 640);
            MinimumSize = new Size(640, 400);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = SystemColors.Control;
            ForeColor = SystemColors.ControlText;
            Font = new Font("Segoe UI", 9f);
            ShowInTaskbar = false;

            var tlp = new TableLayoutPanel {
                Dock = DockStyle.Top, AutoSize = true, ColumnCount = 3, Padding = new Padding(10, 10, 10, 4),
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            Label Etiqueta(string t) => new() { Text = t, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 6, 8, 6) };

            _txtRuta = new TextBox { ReadOnly = true, Dock = DockStyle.Fill, Margin = new Padding(0, 3, 6, 3), BackColor = SystemColors.Window };
            _btnExaminar = CrearBoton("Examinar…", 110);
            _btnExaminar.Click += (_, _) => ElegirFichero();
            tlp.Controls.Add(Etiqueta("Log de Gregal:"), 0, 0);
            tlp.Controls.Add(_txtRuta, 1, 0);
            tlp.Controls.Add(_btnExaminar, 2, 0);

            _cmbFabricante = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 160, Margin = new Padding(0, 3, 0, 3) };
            _cmbFabricante.Items.AddRange(Enum.GetNames<Fabricante>());
            _cmbFabricante.SelectedItem = Enum.GetNames<Fabricante>()
                .FirstOrDefault(f => f.Equals(fabricante, StringComparison.OrdinalIgnoreCase));
            _cmbFabricante.SelectedIndexChanged += (_, _) => ActualizarBotones();
            tlp.Controls.Add(Etiqueta("Fabricante:"), 0, 1);
            tlp.Controls.Add(_cmbFabricante, 1, 1);

            var info = new Label {
                AutoSize = true, ForeColor = SystemColors.GrayText, Margin = new Padding(0, 8, 0, 4), MaximumSize = new Size(900, 0),
                Text = $"Se importan TODAS las partidas del log a la tabla {_tabla}, nunca a HISTORICO, como sala {_codSala} y " +
                       $"concesionario {_codConc} (los de la configuración). Si la tabla no existe se crea con la misma estructura " +
                       $"que HISTORICO. Una partida que ya esté en {_tabla} con la misma clave se sobrescribe. El fabricante decide " +
                       "cómo se interpretan algunos eventos (en MCB, A/M/p son ruido; en Metronia, premios).",
            };
            tlp.Controls.Add(info, 0, 2);
            tlp.SetColumnSpan(info, 3);

            _txtResultado = new TextBox {
                Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Both, WordWrap = false, Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9f), BackColor = SystemColors.Window,
                Text = "Elige un log de Gregal (carpeta logs junto a Gregal.exe, ficheros gregal<AAAAMMDD>.log) y el fabricante de la sala.",
            };

            _btnImportar = CrearBoton("⇩ Importar", 130);
            _btnImportar.Click += async (_, _) => await ImportarAsync();
            _btnCerrar = CrearBoton("Cerrar", 100);
            _btnCerrar.Click += (_, _) => Close();
            CancelButton = _btnCerrar;
            var flp = new FlowLayoutPanel {
                Dock = DockStyle.Bottom, Height = 48, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 7, 8, 0),
            };
            flp.Controls.AddRange(new Control[] { _btnCerrar, _btnImportar });

            var pnlResultado = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 4, 10, 0) };
            pnlResultado.Controls.Add(_txtResultado);

            Controls.Add(pnlResultado);
            Controls.Add(tlp);
            Controls.Add(flp);
            FormClosing += (_, e) => { if (!_btnCerrar.Enabled) e.Cancel = true; };   // importando: esperar
            ActualizarBotones();
        }

        private static Button CrearBoton(string texto, int ancho) => new() {
            Text = texto, Size = new Size(ancho, 32), FlatStyle = FlatStyle.Standard,
            ForeColor = SystemColors.ControlText, UseVisualStyleBackColor = true, Cursor = Cursors.Hand,
        };

        private void ActualizarBotones() =>
            _btnImportar.Enabled = _txtRuta.Text.Length > 0 && _cmbFabricante.SelectedItem != null;

        private void ElegirFichero() {
            using var dlg = new OpenFileDialog {
                Title = "Log de Gregal",
                Filter = "Logs de Gregal (gregal*.log)|gregal*.log|Logs (*.log)|*.log|Todos los ficheros (*.*)|*.*",
            };
            if (_txtRuta.Text.Length > 0) dlg.InitialDirectory = Path.GetDirectoryName(_txtRuta.Text);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;
            _txtRuta.Text = dlg.FileName;
            ActualizarBotones();
        }

        private async Task ImportarAsync() {
            string ruta = _txtRuta.Text;
            var fabricante = Enum.Parse<Fabricante>((string)_cmbFabricante.SelectedItem!);
            _ultimoFabricante = fabricante.ToString();
            try { Program.GuardarFabricanteLog(_ultimoFabricante); } catch { /* recordarlo no es imprescindible */ }

            var reglas = new ReglasProtocoloTag { Fabricante = fabricante, DefaultCodSala = _codSala, DefaultCodConc = _codConc };
            try {
                SetBusy(true);
                _txtResultado.Text = "Leyendo el log…";
                var r = await Task.Run(() => ReproductorLogGregal.Reproducir(ruta, reglas));

                if (r.Eventos == 0) {
                    _txtResultado.Text =
                        $"{ruta}\r\n\r\nEste log no tiene ninguna línea \"Fragmento tag recibido\": no se puede reconstruir ninguna partida.\r\n" +
                        "Gregal solo las escribe con TagProtocolSettings:LogRawFrames = true en su appsettings.json.";
                    return;
                }
                if (r.Partidas.Count == 0) {
                    _txtResultado.Text = Resumen(ruta, fabricante, r, null, false);
                    return;
                }

                _txtResultado.Text = $"{r.Partidas.Count} partidas reconstruidas; guardando en {_tabla}…";
                bool creada = await _db.CrearTablaComoHistoricoAsync(_tabla);
                var res = await _db.ImportarPartidasAsync(_tabla, r.Partidas.Select(p => p.Partida).ToList());
                _txtResultado.Text = Resumen(ruta, fabricante, r, res, creada) +
                    "\r\nAl cerrar esta ventana se mostrarán en la pestaña " + _tabla + " de la pantalla principal.";
                UltimaSesionImportada = r.Partidas
                    .Select(p => DateTime.ParseExact(p.Partida.FechaSesion, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                    .Max();

                _registrar(
                    $"Log {Path.GetFileName(ruta)} importado a {_tabla}: {r.Partidas.Count} partidas " +
                    $"({res.Nuevas} nuevas, {res.Sobrescritas} sobrescritas, {res.RepetidasEnElLog} con número repetido en el log).",
                    new {
                        Log = ruta, Tabla = _tabla, TablaCreada = creada, Fabricante = fabricante.ToString(),
                        Sala = _codSala, Concesionario = _codConc,
                        Partidas = r.Partidas.Count, res.Nuevas, res.Sobrescritas, res.RepetidasEnElLog, res.ConBolasRecortadas,
                        Avisos = r.Avisos.Count,
                    });
            }
            catch (Exception ex) {
                _txtResultado.Text = $"{ruta}\r\n\r\nNo se ha importado nada:\r\n{ex.Message}";
            }
            finally {
                SetBusy(false);
            }
        }

        private string Resumen(string ruta, Fabricante fabricante, ResultadoReproduccion r,
                               FirebirdService.ResultadoImportacion? res, bool tablaCreada) {
            var sb = new StringBuilder();
            sb.AppendLine($"Log:        {ruta}");
            sb.AppendLine($"Fabricante: {fabricante}    Sala {_codSala}, concesionario {_codConc}");
            sb.AppendLine($"Eventos:    {r.Eventos:N0} de {r.LineasLeidas:N0} líneas, del {r.PrimerEvento:dd/MM/yyyy HH:mm:ss} al {r.UltimoEvento:dd/MM/yyyy HH:mm:ss}; Gregal arrancó {r.Arranques} veces");
            sb.AppendLine();

            if (res == null) {
                sb.AppendLine("No se ha reconstruido ninguna partida completa: no se ha guardado nada.");
            }
            else {
                sb.AppendLine($"Tabla {_tabla}{(tablaCreada ? " (creada ahora)" : "")}: {r.Partidas.Count} partidas guardadas");
                sb.AppendLine($"  {res.Nuevas} nuevas, {res.Sobrescritas} sobrescriben una que ya estaba en la tabla");
                if (res.RepetidasEnElLog > 0)
                    sb.AppendLine($"  {res.RepetidasEnElLog} repiten un número de partida que ya salía antes en el mismo log: queda la última (como en Gregal)");
                if (res.ConBolasRecortadas > 0)
                    sb.AppendLine($"  {res.ConBolasRecortadas} con la lista de bolas recortada (no cabía en la columna BOLAS)");
                sb.AppendLine();
                sb.AppendLine("Sesión      Nº    Inicio    Fin       Bolas  Vend.  Precio   Línea    Bingo  Plus  Notas");
                foreach (var p in r.Partidas) {
                    var d = p.Partida;
                    string Imp(decimal? v) => v.HasValue ? v.Value.ToString("0.00", CultureInfo.CurrentCulture) : "-";
                    sb.AppendLine(string.Format(CultureInfo.CurrentCulture,
                        "{0,-10} {1,4}  {2,-8}  {3,-8}  {4,5}  {5,5}  {6,6}  {7,6}  {8,7}  {9,4}  {10}",
                        d.FechaSesion, d.PartidaDia, d.HoraVenta ?? "--", d.HoraFin ?? "--", d.Extracciones ?? 0, d.Vendidos,
                        Imp(d.Precio), Imp(d.Premios[0]), Imp(d.Premios[1]), d.Premios[16]?.ToString("0", CultureInfo.CurrentCulture) ?? "-",
                        d.Reclamacion?.Trim() ?? ""));
                }
            }

            if (r.Avisos.Count > 0) {
                sb.AppendLine();
                sb.AppendLine($"Avisos ({r.Avisos.Count}), agrupados; entre paréntesis, las primeras líneas del log:");
                foreach (var g in r.Avisos.GroupBy(a => Regex.Replace(a.Texto, @"\d+", "N")).OrderByDescending(g => g.Count())) {
                    string lineas = string.Join(", ", g.Take(5).Select(a => a.Linea)) + (g.Count() > 5 ? ", …" : "");
                    sb.AppendLine($"  {g.Count(),4} × {(g.Count() == 1 ? g.First().Texto : g.Key)}  ({lineas})");
                }
            }
            return sb.ToString();
        }

        private void SetBusy(bool busy) {
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            _btnImportar.Enabled = !busy && _txtRuta.Text.Length > 0 && _cmbFabricante.SelectedItem != null;
            _btnExaminar.Enabled = !busy;
            _btnCerrar.Enabled = !busy;
            _cmbFabricante.Enabled = !busy;
        }
    }
}
