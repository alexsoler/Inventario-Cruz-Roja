namespace InventarioCruzRoja.Models
{
    public class Sede
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int EstadoId { get; set; }
        public Estado Estado { get; set; }
    }
}