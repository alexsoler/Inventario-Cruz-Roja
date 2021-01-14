using System.Collections.Generic;

namespace InventarioCruzRoja.Models
{
    public class Fabricante : EntidadBase<int>
    {
        public string Nombre { get; set; }
        public int EstadoId { get; set; }
        public Estado Estado { get; set; }
        public ICollection<Producto> Productos { get; set; }
    }
}