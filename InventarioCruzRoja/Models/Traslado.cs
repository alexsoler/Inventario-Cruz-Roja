namespace InventarioCruzRoja.Models
{
    public class Traslado : EntidadBase<int>
    {
        public int EgresoOrigenId { get; set; }
        public int IngresoDestinoId { get; set; }
        public int ProductoId { get; set; }
        public int UserId { get; set; }
        public int? UserAnulaId { get; set; }
        public int Cantidad { get; set; }
        public string Observaciones { get; set; }
        public string MotivoAnula { get; set; }
        public DateTime Fecha { get; set; }
        public bool Anulado { get; set; }
        public DateTime? FechaAnula { get; set; }
        public Ingreso IngresoDestino { get; set; }
        public Egreso EgresoOrigen { get; set; }
        public Producto Producto { get; set; }
        public User User { get; set; }
        public User UserAnula { get; set; }
    }
}
