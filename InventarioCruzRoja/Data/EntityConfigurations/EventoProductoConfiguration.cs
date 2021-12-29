using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioCruzRoja.Data.EntityConfigurations
{
    public class EventoProductoConfiguration : IEntityTypeConfiguration<EventoProducto>
    {
        public void Configure(EntityTypeBuilder<EventoProducto> builder)
        {
            builder.ToTable("EventosProductos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Descripcion).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Fecha).IsRequired();

        }
    }
}
