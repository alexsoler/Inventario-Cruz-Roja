﻿namespace InventarioCruzRoja.Models
{
    public class Proveedor : EntidadBase<int>
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string TelefonoFijo1 { get; set; }
        public string TelefonoFijo2 { get; set; }
        public string SitioWeb { get; set; }
        public string Email { get; set; }
        public int EstadoId { get; set; }
        public Estado Estado { get; set; }
        public ICollection<Contacto> Contactos { get; set; }
    }
}
