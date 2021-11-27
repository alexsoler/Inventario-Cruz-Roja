using System.ComponentModel.DataAnnotations;

namespace InventarioCruzRoja.Dtos
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [StringLength(300)]
        public string Descripcion { get; set; }
        [Required]
        public int EstadoId { get; set; }
    }
}
