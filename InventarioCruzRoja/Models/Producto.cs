namespace InventarioCruzRoja.Models
{
    public class Producto : EntidadBase<int>
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Modelo { get; set; }
        public string Presentacion { get; set; }
        public string Descripcion { get; set; }
        public string Observaciones { get; set; }
        public int FabricanteId { get; set; }
        public int CategoriaId { get; set; }
        public int EstadoId { get; set; }
        public decimal Costo { get; set; }
        public string ImagenUrl { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModifica { get; set; }
        public Estado Estado { get; set; }
        public Fabricante Fabricante { get; set; }
        public ICollection<Sede> Sedes { get; set; }
        public Categoria Categoria { get; set; }
        public ICollection<Ingreso> Ingresos { get; set; }
        public ICollection<Egreso> Egresos { get; set; }
        public ICollection<EventoProducto> Eventos { get; set; }

    }
}