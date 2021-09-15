using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioCruzRoja.Data.EntityConfigurations
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.ToTable("Productos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Codigo).HasMaxLength(20);
            builder.Property(x => x.Costo).HasColumnType("money").IsRequired();
            builder.Property(x => x.Descripcion).HasMaxLength(500);
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Presentacion).HasMaxLength(100);
            builder.Property(x => x.Observaciones).HasMaxLength(1000);
            builder.Property(x => x.Modelo).HasMaxLength(100);
            builder.Property(x => x.ImagenUrl).HasMaxLength(200);
            builder.Property(x => x.EstadoId).IsRequired();
        }
    }
}