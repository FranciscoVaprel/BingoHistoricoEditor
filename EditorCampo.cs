using BingoHistoricoEditor.Models;

namespace BingoHistoricoEditor {
    /// <summary>
    /// Un campo de la ficha de partida, se edite en una fila (<see cref="EditorCampo"/>)
    /// o en la rejilla de premios (<see cref="FichaPartidaForm"/>).
    /// </summary>
    internal interface ICampoFicha {
        ColumnaHistorico Columna { get; }

        /// <summary>Valor leído de la BD, sin tocar (para el resumen y el control de concurrencia).</summary>
        object? ValorBd { get; }

        /// <summary>Valor actual, normalizado (ver FichaPartidaCampos.Normalizar); null = NULL.</summary>
        object? Valor { get; }

        bool Modificado { get; }
        void Deshacer();
    }

    /// <summary>
    /// Fila de la ficha para una columna de HISTORICO: etiqueta con el nombre,
    /// el control que corresponde a su tipo (número, texto, fecha, hora...) y,
    /// si la columna admite NULL, una casilla "vacío" para dejarla sin valor.
    ///
    /// "Modificado" compara con lo que mostraba el control al abrir la ficha, no
    /// con el valor crudo de la BD: así una hora con milésimas o un CHAR con
    /// espacios de relleno no cuentan como cambio si el usuario no los toca.
    /// </summary>
    internal sealed class EditorCampo : ICampoFicha {
        public ColumnaHistorico Columna { get; }
        public object? ValorBd { get; }
        public Label Etiqueta { get; }
        public Control Entrada { get; }
        public CheckBox? Nulo { get; }

        public event EventHandler? Cambiado;

        private readonly object? _inicial;
        private readonly Font _fuenteNormal;
        private readonly Font _fuenteModificado;
        private bool _cargando;

        public EditorCampo(ColumnaHistorico columna, object? valorBd, ToolTip ayuda) {
            Columna = columna;
            ValorBd = valorBd;

            Etiqueta = new Label {
                Text = columna.Nombre,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = columna.Editable ? SystemColors.ControlText : SystemColors.GrayText,
            };
            _fuenteNormal = Etiqueta.Font;
            _fuenteModificado = new Font(Etiqueta.Font, FontStyle.Bold);
            Etiqueta.Disposed += (_, _) => _fuenteModificado.Dispose();

            Entrada = CrearEntrada(columna);

            if (columna.Editable && columna.AdmiteNulo) {
                Nulo = new CheckBox { Text = "vacío", AutoSize = true, Anchor = AnchorStyles.Left };
                ayuda.SetToolTip(Nulo, "Marcado: el campo se guarda sin valor (NULL).");
            }

            string textoAyuda = FichaPartidaCampos.Ayuda(columna);
            ayuda.SetToolTip(Etiqueta, textoAyuda);
            ayuda.SetToolTip(Entrada, textoAyuda);

            Escribir(valorBd);
            _inicial = Valor;
            ActualizarEstilo();

            switch (Entrada) {
                case NumericUpDown n: n.ValueChanged += (_, _) => AlCambiar(); break;
                case DateTimePicker d: d.ValueChanged += (_, _) => AlCambiar(); break;
                case TextBox t when columna.Editable: t.TextChanged += (_, _) => AlCambiar(); break;
            }
            if (Nulo != null) {
                Nulo.CheckedChanged += (_, _) => {
                    Entrada.Enabled = !Nulo.Checked;
                    AlCambiar();
                };
            }
        }

        private static Control CrearEntrada(ColumnaHistorico col) {
            switch (col.Tipo) {
                case TipoColumna.Entero:
                case TipoColumna.Decimal:
                case TipoColumna.Real:
                    return new NumericUpDown {
                        Minimum = col.Minimo,
                        Maximum = col.Maximo,
                        Increment = col.Tipo == TipoColumna.Entero || col.Escala < 2 ? 1 : 0.01m,
                        TextAlign = HorizontalAlignment.Right,
                        Width = 180,
                        Anchor = AnchorStyles.Left,
                    };
                case TipoColumna.Fecha:
                    return new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Width = 180, Anchor = AnchorStyles.Left };
                case TipoColumna.Hora:
                    return new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm:ss", ShowUpDown = true, Width = 180, Anchor = AnchorStyles.Left };
                case TipoColumna.FechaHora:
                    return new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm:ss", Width = 180, Anchor = AnchorStyles.Left };
                case TipoColumna.Texto: {
                    // Textos largos (BOLAS, CART_LINEA...) en varias líneas; los
                    // muy cortos (LETRA1: CHAR(2)) en una caja estrecha.
                    bool largo = col.LongitudMaxima is null or > 100;
                    var t = new TextBox {
                        MaxLength = col.LongitudMaxima ?? 0,
                        Multiline = largo,
                        WordWrap = true,
                        ScrollBars = largo ? ScrollBars.Vertical : ScrollBars.None,
                        Height = largo ? 54 : 23,
                    };
                    if (col.LongitudMaxima is <= 10) { t.Width = 100; t.Anchor = AnchorStyles.Left; }
                    else { t.Anchor = AnchorStyles.Left | AnchorStyles.Right; }
                    return t;
                }
                default:
                    return new TextBox { ReadOnly = true, Anchor = AnchorStyles.Left | AnchorStyles.Right };
            }
        }

        /// <summary>Pone en el control un valor (crudo de la BD o ya normalizado).</summary>
        private void Escribir(object? valor) {
            _cargando = true;
            try {
                var v = FichaPartidaCampos.Normalizar(Columna, valor);
                if (Nulo != null) Nulo.Checked = v is null;
                Entrada.Enabled = v is not null || Nulo is null;

                switch (Entrada) {
                    case NumericUpDown n: {
                        decimal d = v is decimal dv ? dv : 0;
                        if (Columna.Tipo != TipoColumna.Entero) {
                            n.DecimalPlaces = Math.Max(n.DecimalPlaces, FichaPartidaCampos.DecimalesAMostrar(d, Columna.Escala));
                        }
                        n.Value = Math.Clamp(d, n.Minimum, n.Maximum);
                        break;
                    }
                    case DateTimePicker p: {
                        DateTime f = v switch {
                            DateTime dt => dt,
                            TimeSpan ts => DateTime.Today + ts,
                            _ => Columna.Tipo == TipoColumna.Hora ? DateTime.Today : DateTime.Now,
                        };
                        p.Value = f < DateTimePicker.MinimumDateTime ? DateTimePicker.MinimumDateTime
                                : f > DateTimePicker.MaximumDateTime ? DateTimePicker.MaximumDateTime : f;
                        break;
                    }
                    case TextBox t:
                        t.Text = Columna.Editable
                            ? v as string ?? ""
                            : (v is null ? "" : FichaPartidaCampos.Formatear(Columna, v));
                        break;
                }
            }
            finally {
                _cargando = false;
            }
            ActualizarEstilo();
        }

        public object? Valor {
            get {
                if (!Columna.Editable) return _inicial;
                if (Nulo?.Checked == true) return null;
                return Entrada switch {
                    NumericUpDown n => FichaPartidaCampos.Normalizar(Columna, n.Value),
                    DateTimePicker p => FichaPartidaCampos.Normalizar(Columna, Columna.Tipo == TipoColumna.Hora ? p.Value.TimeOfDay : p.Value),
                    TextBox t => FichaPartidaCampos.Normalizar(Columna, t.Text),
                    _ => _inicial,
                };
            }
        }

        public bool Modificado => Columna.Editable && !FichaPartidaCampos.Iguales(Valor, _inicial);

        public void Deshacer() {
            Escribir(ValorBd);
            Cambiado?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Un NumericUpDown no actualiza su valor hasta que pierde el foco; leer
        /// Value lo obliga a validar lo que haya escrito. Se llama antes de guardar.
        /// </summary>
        public void ConfirmarEdicion() {
            if (Entrada is NumericUpDown n) _ = n.Value;
        }

        public void MostrarFila(bool visible) {
            Etiqueta.Visible = visible;
            Entrada.Visible = visible;
            if (Nulo != null) Nulo.Visible = visible;
        }

        private void AlCambiar() {
            if (_cargando) return;
            ActualizarEstilo();
            Cambiado?.Invoke(this, EventArgs.Empty);
        }

        private void ActualizarEstilo() {
            bool mod = Modificado;
            Etiqueta.Font = mod ? _fuenteModificado : _fuenteNormal;
            Etiqueta.BackColor = mod ? SystemColors.Info : Color.Transparent;
            Etiqueta.ForeColor = !Columna.Editable ? SystemColors.GrayText : mod ? SystemColors.InfoText : SystemColors.ControlText;
        }
    }
}
