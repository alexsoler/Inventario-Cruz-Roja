using System.Collections.Generic;

namespace InventarioCruzRoja.Models
{
    public class Sede : EntidadBase<int>
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int EstadoId { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public Estado Estado { get; set; }
        public ICollection<Producto> Productos { get; set; }
    }
}