namespace InventarioCruzRoja.Models
{
    public class Categoria : EntidadBase<int>
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int EstadoId { get; set; }
        public Estado Estado { get; set; }
    }
}
