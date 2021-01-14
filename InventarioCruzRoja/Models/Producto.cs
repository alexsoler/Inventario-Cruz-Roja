using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Models
{
    public class Producto : EntidadBase<string>
    {
        public string Nombre { get; set; }
        public string Modelo { get; set; }
        public string Presentacion { get; set; }
        public string Descripcion { get; set; }
        public string Observaciones { get; set; }
        public int FabricanteId { get; set; }
        public int SedeId { get; set; }
        public int EstadoId { get; set; }
        public decimal Costo { get; set; }
        public int Stock { get; set; }
        public string ImagenUrl { get; set; }
        public Estado Estado { get; set; }
        public Fabricante Fabricante { get; set; }
        public Sede Sede { get; set; }
    }
}