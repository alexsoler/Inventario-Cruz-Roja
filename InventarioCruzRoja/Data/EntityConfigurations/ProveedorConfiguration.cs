using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Data.EntityConfigurations
{
    public class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
    {
        public void Configure(EntityTypeBuilder<Proveedor> builder)
        {
            builder.ToTable("Proveedores");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Direccion).HasMaxLength(300).IsRequired();
            builder.Property(x => x.TelefonoFijo1).HasMaxLength(15);
            builder.Property(x => x.TelefonoFijo2).HasMaxLength(15);
            builder.Property(x => x.SitioWeb).HasMaxLength(300);
            builder.Property(x => x.Email).HasMaxLength(100);
        }
    }
}
