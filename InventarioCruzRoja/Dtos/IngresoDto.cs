using System;
using System.ComponentModel.DataAnnotations;

namespace InventarioCruzRoja.Dtos
{
    public class IngresoDto
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public string Proveedor { get; set; }
        public int SedeId { get; set; }
        public string Sede { get; set; }
        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public int UserId { get; set; }
        public string Usuario { get; set; }
        public string Observaciones { get; set; }
        public bool Anulado { get; set; }
        public DateTime Fecha { get; set; }
        [Required]
        public int Cantidad { get; set; }

    }
}
