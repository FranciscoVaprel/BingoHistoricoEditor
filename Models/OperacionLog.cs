namespace BingoHistoricoEditor.Models {
    /// <summary>
    /// Modelo para las operaciones registradas en la tabla de auditoría
    /// </summary>
    public class OperacionLog {
        public int ID { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; } = "";
        public string Operacion { get; set; } = "";
        public string Detalles { get; set; } = "";

        public override string ToString() {
            return $"[{Fecha:yyyy-MM-dd HH:mm:ss}] {Usuario} - {Operacion}";
        }
    }
}
