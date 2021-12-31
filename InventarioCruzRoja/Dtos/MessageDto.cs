using System.ComponentModel.DataAnnotations;

namespace InventarioCruzRoja.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string UserName { get; set; }
        [Required, StringLength(300)]
        public string Text { get; set; }
        public DateTime Fecha { get; set; }
        public int UserId { get; set; }
        public MessageDto()
        {
            Fecha = DateTime.Now;
        }
    }
}
