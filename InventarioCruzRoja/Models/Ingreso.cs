using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Models
{
    public class Ingreso : EntidadBase<int>
    {
        public int ProveedorId { get; set; }
        public int ProductoId { get; set; }
        public int UserId { get; set; }
        public DateTime Fecha { get; set; }
        public int Cantidad { get; set; }
        public Proveedor Proveedor { get; set; }
        public Producto Producto { get; set; }
        public User User { get; set; }
    }
}
