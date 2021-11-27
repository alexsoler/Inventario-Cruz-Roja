using System.ComponentModel.DataAnnotations;

namespace InventarioCruzRoja.Dtos
{
    public class ContactoDto
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Nombre { get; set; }
        [Phone, StringLength(15)]
        public string Telefono { get; set; }
        [EmailAddress, StringLength(100)]
        public string Email { get; set; }
        [Required]
        public int ProveedorId { get; set; }
    }
}
