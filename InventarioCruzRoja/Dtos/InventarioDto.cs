namespace InventarioCruzRoja.Dtos
{
    public class InventarioDto
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; }
        public string Producto { get; set; }
        public string ImagenUrl { get; set; }
        public string Estado { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Presentacion { get; set; }
        public string Sede { get; set; }
    }
}
