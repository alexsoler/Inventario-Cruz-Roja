using System.ComponentModel.DataAnnotations;

namespace InventarioCruzRoja.Dtos
{
    public class FabricanteDto
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Nombre { get; set; }
        [Required]
        public int EstadoId { get; set; }
    }
}