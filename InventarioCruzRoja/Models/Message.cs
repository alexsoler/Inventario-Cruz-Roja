namespace InventarioCruzRoja.Models
{
    public class Message : EntidadBase<int>
    {
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime Fecha { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public Message()
        {
            Fecha = DateTime.Now;
        }
    }
}
