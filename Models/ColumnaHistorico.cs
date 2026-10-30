namespace BingoHistoricoEditor.Models {
    /// <summary>
    /// Cómo se edita una columna de HISTORICO en la ficha de partida. Se deduce
    /// del tipo de Firebird (RDB$FIELDS), no del nombre de la columna.
    /// </summary>
    public enum TipoColumna {
        Entero,      // SMALLINT / INTEGER / BIGINT sin decimales
        Decimal,     // NUMERIC / DECIMAL (importes: NUMERIC(15,5) en las salas)
        Real,        // FLOAT / DOUBLE PRECISION
        Texto,       // CHAR / VARCHAR / BLOB de texto
        Fecha,       // DATE
        Hora,        // TIME
        FechaHora,   // TIMESTAMP
        NoEditable,  // BLOB binario, BOOLEAN, columnas calculadas...: solo se muestran
    }

    /// <summary>
    /// Una columna de HISTORICO tal como la describe la propia base de datos de
    /// la sala. La ficha de partida se construye a partir de esta lista, así que
    /// se adapta a las columnas que tenga cada sala sin tocar el código.
    /// </summary>
    public sealed class ColumnaHistorico {
        public static readonly string[] ColumnasClave = { "COD_SALA", "COD_CONC", "FECHA_SESION", "PARTIDA_DIA" };

        public required string Nombre { get; init; }
        public TipoColumna Tipo { get; init; }

        /// <summary>Decimales (escala) de un NUMERIC/DECIMAL; 0 en el resto.</summary>
        public int Escala { get; init; }

        /// <summary>Límites del tipo numérico (p. ej. NUMERIC(15,5) → ±9999999999,99999).</summary>
        public decimal Minimo { get; init; }
        public decimal Maximo { get; init; }

        /// <summary>Longitud máxima en caracteres de un CHAR/VARCHAR; null si no tiene (BLOB).</summary>
        public int? LongitudMaxima { get; init; }

        /// <summary>CHAR rellena con espacios a la derecha: se quitan al mostrarlo.</summary>
        public bool EsChar { get; init; }
        public bool EsBlob { get; init; }
        public bool AdmiteNulo { get; init; }

        /// <summary>COMPUTED BY: no se puede escribir, ni siquiera al copiar la fila.</summary>
        public bool EsCalculada { get; init; }

        /// <summary>Tipo tal como se declaró en la BD, para mostrarlo en la ayuda (p. ej. "NUMERIC(15,5)").</summary>
        public string DescripcionTipo { get; init; } = "";

        /// <summary>
        /// Tipo en SQL para crear una columna igual (p. ej. "NUMERIC(15,5)"), o
        /// null si no se sabe recrear (tipo raro o columna calculada).
        /// </summary>
        public string? TipoSql { get; init; }

        public bool EsClave => ColumnasClave.Contains(Nombre, StringComparer.OrdinalIgnoreCase);
        public bool Editable => Tipo != TipoColumna.NoEditable;
        public bool EsNumerica => Tipo is TipoColumna.Entero or TipoColumna.Decimal or TipoColumna.Real;

        public override string ToString() => Nombre;
    }
}
