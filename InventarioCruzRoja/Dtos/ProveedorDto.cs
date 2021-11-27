using System.ComponentModel.DataAnnotations;

namespace InventarioCruzRoja.Dtos
{
    public class ProveedorDto
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Nombre { get; set; }
        [Required, StringLength(300)]
        public string Direccion { get; set; }
        [StringLength(15)]
        public string TelefonoFijo1 { get; set; }
        [StringLength(15)]
        public string TelefonoFijo2 { get; set; }
        [StringLength(300)]
        public string SitioWeb { get; set; }
        [EmailAddress, StringLength(100)]
        public string Email { get; set; }
        [Required]
        public int EstadoId { get; set; }
        public ICollection<ContactoDto> Contactos { get; set; }
    }
}
