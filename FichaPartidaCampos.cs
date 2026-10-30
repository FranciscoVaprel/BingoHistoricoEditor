using BingoHistoricoEditor.Data;
using BingoHistoricoEditor.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BingoHistoricoEditor {
    /// <summary>
    /// Lo que la ficha de partida sabe de cada columna de HISTORICO por su
    /// nombre: en qué pestaña va y qué significa. Una columna que no aparece
    /// aquí (p. ej. una que solo tenga alguna sala) se edita igual, en "Otros",
    /// y sin descripción.
    /// </summary>
    internal static class FichaPartidaCampos {
        public const string PestañaGeneral = "General";
        public const string PestañaVenta   = "Venta y caja";
        public const string PestañaSeries  = "Series";
        public const string PestañaCantos  = "Bolas y cantos";
        public const string PestañaPremios = "Premios";
        public const string PestañaOtros   = "Otros";

        public static readonly string[] Pestañas = {
            PestañaGeneral, PestañaVenta, PestañaSeries, PestañaCantos, PestañaPremios, PestañaOtros
        };

        private static readonly Regex Premio = new(@"^PREMIO(\d+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly HashSet<string> General = new(StringComparer.OrdinalIgnoreCase) {
            "FECHA", "PARTIDA_ANUAL", "TIPO", "NUM_DECIMALES", "FECHA_SESION_MES", "RECLAMACION",
            "HORA_VENTA", "HORA_CIERRE", "HORA_CIERRE_H", "HORA_SORTEO", "HORA_FIN",
        };
        private static readonly HashSet<string> Venta = new(StringComparer.OrdinalIgnoreCase) {
            "PRECIO", "VENDIDOS", "VENTA", "CAJA", "TASA", "BENEFICIO", "CAJA_I", "CAJA_A_I", "ACUMULADOS",
        };
        private static readonly HashSet<string> Cantos = new(StringComparer.OrdinalIgnoreCase) {
            "EXTRACCIONES", "GUIABOLAS", "BOLAS",
            "BOLA_LINEA", "NUM_LINEAS", "HORA_LINEA", "CART_LINEA",
            "BOLA_BINGO", "NUM_BINGOS", "HORA_BINGO", "CART_BINGO",
        };

        // Solo las que se conocen con seguridad (por el propio editor y por lo
        // que guarda Gregal); al resto no se le inventa un significado.
        private static readonly Dictionary<string, string> Descripciones = new(StringComparer.OrdinalIgnoreCase) {
            ["COD_SALA"]      = "Código de la sala",
            ["COD_CONC"]      = "Código del concesionario",
            ["FECHA_SESION"]  = "Día de la sesión a la que pertenece la partida",
            ["PARTIDA_DIA"]   = "Número de la partida dentro del día",
            ["FECHA"]         = "Fecha y hora de inicio de la partida",
            ["NUM_DECIMALES"] = "Decimales de los importes (normalmente 2)",
            ["HORA_VENTA"]    = "Hora de inicio de la venta",
            ["HORA_CIERRE"]   = "Hora de cierre de la venta",
            ["HORA_FIN"]      = "Hora de fin de la partida",
            ["RECLAMACION"]   = "Observaciones (Gregal anota aquí, p. ej., las partidas recuperadas a medias)",
            ["PRECIO"]        = "Precio del cartón",
            ["VENDIDOS"]      = "Cartones vendidos",
            ["VENTA"]         = "Venta total. En la edición individual se calcula como PRECIO × VENDIDOS; aquí no se recalcula sola",
            ["LETRA1"]        = "Letra de la serie 1",
            ["SERIE_1"]       = "Número de la serie 1",
            ["DEL_1"]         = "Primer cartón vendido de la serie 1",
            ["AL_1"]          = "Último cartón vendido de la serie 1",
            ["LETRA2"]        = "Letra de la serie 2 (si la venta pasa a una segunda serie)",
            ["SERIE_2"]       = "Número de la serie 2",
            ["DEL_2"]         = "Primer cartón vendido de la serie 2",
            ["AL_2"]          = "Último cartón vendido de la serie 2",
            ["EXTRACCIONES"]  = "Número de bolas extraídas",
            ["BOLAS"]         = "Bolas en orden de salida, separadas por comas",
            ["BOLA_LINEA"]    = "Bolas extraídas cuando se cantó la línea",
            ["NUM_LINEAS"]    = "Número de cartones premiados con línea",
            ["HORA_LINEA"]    = "Hora del primer canto de línea",
            ["CART_LINEA"]    = "Cartones premiados con línea, separados por comas",
            ["BOLA_BINGO"]    = "Bolas extraídas cuando se cantó el bingo",
            ["NUM_BINGOS"]    = "Número de cartones premiados con bingo",
            ["HORA_BINGO"]    = "Hora del primer canto de bingo",
            ["CART_BINGO"]    = "Cartones premiados con bingo, separados por comas",
            ["PREMIO1"]       = "Línea",
            ["PREMIO2"]       = "Bingo",
            ["PREMIO5"]       = "Línea plus (salas Metronia)",
            ["PREMIO6"]       = "Prima (salas Metronia)",
            ["PREMIO7"]       = "Bola tope de la prima: es un nº de bola, no un importe (salas Metronia)",
            ["PREMIO17"]      = "Bola plus: nº de bolas que habían salido cuando llegó, no un importe",
        };

        public static string Pestaña(ColumnaHistorico col) {
            string n = col.Nombre;
            if (EsPremio(col, out _)) return PestañaPremios;
            if (n.EndsWith("_VIR", StringComparison.OrdinalIgnoreCase)) return PestañaSeries;
            if (General.Contains(n)) return PestañaGeneral;
            if (Venta.Contains(n)) return PestañaVenta;
            if (Cantos.Contains(n)) return PestañaCantos;
            if (Regex.IsMatch(n, @"^(LETRA|SERIE_|DEL_|AL_|LARGO_SERIE)", RegexOptions.IgnoreCase)) return PestañaSeries;
            return PestañaOtros;
        }

        /// <summary>PREMIOn numérico: va a la rejilla de premios en vez de a una fila de la ficha.</summary>
        public static bool EsPremio(ColumnaHistorico col, out int numero) {
            numero = 0;
            var m = Premio.Match(col.Nombre);
            return m.Success && col.EsNumerica && col.Editable && int.TryParse(m.Groups[1].Value, out numero);
        }

        public static string? Descripcion(string nombre) =>
            Descripciones.TryGetValue(nombre, out var d) ? d : null;

        /// <summary>Texto de ayuda de un campo: significado (si se conoce) y tipo en la BD.</summary>
        public static string Ayuda(ColumnaHistorico col) {
            var partes = new List<string>();
            if (Descripcion(col.Nombre) is string d) partes.Add(d);
            string tipo = col.DescripcionTipo + (col.AdmiteNulo ? ", admite vacío (NULL)" : ", obligatorio");
            if (!col.Editable) tipo += ". No se puede modificar desde aquí";
            partes.Add($"{col.Nombre}: {tipo}");
            return string.Join(Environment.NewLine, partes);
        }

        /// <summary>
        /// Valor de la BD (o de un control) en la forma en que la ficha lo
        /// compara: números como decimal, textos sin el relleno de CHAR, horas
        /// como TimeSpan y fechas/horas al segundo (lo que muestran los controles).
        /// </summary>
        public static object? Normalizar(ColumnaHistorico col, object? valor) {
            if (valor is null or DBNull) return null;
            switch (col.Tipo) {
                case TipoColumna.Entero:
                case TipoColumna.Decimal:
                case TipoColumna.Real:
                    return Convert.ToDecimal(valor, CultureInfo.InvariantCulture);
                case TipoColumna.Texto: {
                    string s = Convert.ToString(valor, CultureInfo.InvariantCulture) ?? "";
                    return col.EsChar ? s.TrimEnd() : s;
                }
                case TipoColumna.Fecha:
                    return Convert.ToDateTime(valor, CultureInfo.InvariantCulture).Date;
                case TipoColumna.Hora: {
                    var t = FirebirdService.AHora(valor);
                    return new TimeSpan(t.Hours, t.Minutes, t.Seconds);
                }
                case TipoColumna.FechaHora: {
                    var d = Convert.ToDateTime(valor, CultureInfo.InvariantCulture);
                    return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
                }
                default:
                    return valor;
            }
        }

        public static bool Iguales(object? a, object? b) => Equals(a, b);

        /// <summary>Valor para mostrarlo en textos (resumen de cambios, log, rejilla de premios).</summary>
        public static string Formatear(ColumnaHistorico col, object? valor) {
            var v = Normalizar(col, valor);
            return v switch {
                null => "(vacío)",
                decimal d when col.Tipo == TipoColumna.Entero => d.ToString("0", CultureInfo.CurrentCulture),
                decimal d => FormatearImporte(d, col.Escala),
                DateTime f when col.Tipo == TipoColumna.Fecha => f.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture),
                DateTime f => f.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture),
                TimeSpan t => t.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture),
                string s => $"\"{s}\"",
                _ => Convert.ToString(v, CultureInfo.CurrentCulture) ?? "",
            };
        }

        /// <summary>
        /// Los importes son NUMERIC(15,5) pero casi siempre llevan 2 decimales:
        /// se muestran con 2, o con los que tenga de verdad si son más.
        /// </summary>
        public static string FormatearImporte(decimal valor, int escala) =>
            valor.ToString("F" + DecimalesAMostrar(valor, escala), CultureInfo.CurrentCulture);

        public static int DecimalesAMostrar(decimal valor, int escala) {
            // Decimales reales sin los ceros finales (2,28000 → 2): dividir entre
            // 1,000…0 normaliza la escala interna del decimal.
            decimal sinCeros = valor / 1.000000000000000000000000000000000m;
            int reales = (decimal.GetBits(sinCeros)[3] >> 16) & 0xFF;
            return Math.Min(escala, Math.Max(Math.Min(2, escala), reales));
        }
    }
}
