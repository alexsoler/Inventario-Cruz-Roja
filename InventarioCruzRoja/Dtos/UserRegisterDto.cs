using System.ComponentModel.DataAnnotations;

namespace InventarioCruzRoja.Dtos
{
    public class UserRegisterDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public List<string> Roles { get; set; }
    }
}
