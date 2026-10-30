namespace BingoHistoricoEditor.Models {
    /// <summary>
    /// Clave primaria de HISTORICO (PK_HISTORICO): identifica una partida.
    /// </summary>
    public sealed record ClaveHistorico(int CodSala, int CodConc, DateTime FechaSesion, int PartidaDia) {
        public DateTime FechaSesion { get; init; } = FechaSesion.Date;

        public override string ToString() =>
            $"partida {PartidaDia} del {FechaSesion:dd/MM/yyyy} (sala {CodSala}, concesionario {CodConc})";
    }

    /// <summary>
    /// Un campo modificado en la ficha de partida. <see cref="Antes"/> es el
    /// valor tal como se leyó de la BD (se usa también para comprobar al
    /// guardar que nadie lo ha cambiado mientras tanto); <see cref="Despues"/>,
    /// el nuevo (null = NULL).
    /// </summary>
    public sealed record CambioCampo(ColumnaHistorico Columna, object? Antes, object? Despues);
}
