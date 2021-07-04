using System;
using System.ComponentModel.DataAnnotations;

namespace InventarioCruzRoja.Dtos
{
    public class ProductoDto
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Codigo { get; set; }
        [Required, StringLength(200)]
        public string Nombre { get; set; }
        [StringLength(100)]
        public string Modelo { get; set; }
        [StringLength(100)]
        public string Presentacion { get; set; }
        [StringLength(500)]
        public string Descripcion { get; set; }
        [StringLength(1000)]
        public string Observaciones { get; set; }
        public string Fabricante { get; set; }
        public int FabricanteId { get; set; }
        public string Sede { get; set; }
        public int SedeId { get; set; }
        public string Estado { get; set; }
        public int EstadoId { get; set; }
        [Required]
        public decimal Costo { get; set; }
        [Required]
        public int Stock { get; set; }
        [StringLength(200)]
        public string ImagenUrl { get; set; }
        public string UsuarioModifica { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}