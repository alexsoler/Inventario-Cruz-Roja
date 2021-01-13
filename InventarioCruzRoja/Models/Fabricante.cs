using System.Collections.Generic;

namespace InventarioCruzRoja.Models
{
    public class Fabricante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int EstadoId { get; set; }
        public Estado Estado { get; set; }
        public ICollection<Producto> Productos { get; set; }
    }
}