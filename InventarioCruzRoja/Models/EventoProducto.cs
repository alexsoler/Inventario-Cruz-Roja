namespace InventarioCruzRoja.Models
{
    public class EventoProducto : EntidadBase<int>
    {
        public int ProductoId { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public Producto Producto { get; set; }
    }
}
