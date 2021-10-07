using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Models
{
    public class Ingreso : EntidadBase<int>
    {
        public int ProveedorId { get; set; }
        public int SedeId { get; set; }
        public int ProductoId { get; set; }
        public int UserId { get; set; }
        public int? UserAnulaId { get; set; }
        public string Observaciones { get; set; }
        public string MotivoAnula { get; set; }
        public bool Anulado { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaAnula { get; set; }
        public int Cantidad { get; set; }
        public Proveedor Proveedor { get; set; }
        public Sede Sede { get; set; }
        public Producto Producto { get; set; }
        public User User { get; set; }
        public User UserAnula { get; set; }
    }
}
