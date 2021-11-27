using System.ComponentModel.DataAnnotations;

namespace InventarioCruzRoja.Dtos
{
    public class UserEditDto
    {
        public int Id { get; set; }
        public string Username { get; set; }

        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public List<string> Roles { get; set; }
    }
}
