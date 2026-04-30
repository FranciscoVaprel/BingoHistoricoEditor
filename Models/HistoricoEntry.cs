namespace BingoHistoricoEditor.Models {
    public class HistoricoEntry {
        public int     CodSala     { get; set; }
        public int     CodConc     { get; set; }
        public DateTime FechaSesion { get; set; }
        public int     PartidaDia  { get; set; }
        public DateTime Fecha      { get; set; }
        public decimal Precio      { get; set; }
        public int     Vendidos    { get; set; }
        public decimal Venta       { get; set; }   // calculado: Precio × Vendidos
        public decimal Premio1     { get; set; }   // línea
        public decimal Premio2     { get; set; }   // bingo
    }
}
