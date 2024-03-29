﻿namespace InventarioCruzRoja.Models
{
    public class Contacto : EntidadBase<int>
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
    }
}
