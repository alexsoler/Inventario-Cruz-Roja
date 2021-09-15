using System.Collections.Generic;

namespace InventarioCruzRoja.Dtos
{
    public class SedeDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int EstadoId { get; set; }
        public IEnumerable<ProductoDto> Productos { get; set; }
    }
}