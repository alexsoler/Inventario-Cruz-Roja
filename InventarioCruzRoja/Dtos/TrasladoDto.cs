using System.ComponentModel.DataAnnotations;

namespace InventarioCruzRoja.Dtos
{
    public class TrasladoDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int SedeOrigenId { get; set; }
        public string SedeOrigen { get; set; }
        [Required]
        public int SedeDestinoId { get; set; }
        public string SedeDestino { get; set; }
        [Required]
        public int ProductoId { get; set; }
        public string Producto { get; set; }
        [Required]
        public int UserId { get; set; }
        public string Usuario { get; set; }
        public int? UserAnulaId { get; set; }
        public string UsuarioAnula { get; set; }
        [StringLength(300)]
        public string Observaciones { get; set; }
        public bool Anulado { get; set; }
        [StringLength(300)]
        public string MotivoAnula { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaAnula { get; set; }
        [Required]
        public int Cantidad { get; set; }
    }
}
