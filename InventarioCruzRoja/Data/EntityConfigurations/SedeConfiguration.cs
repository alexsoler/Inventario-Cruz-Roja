using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioCruzRoja.Data.EntityConfigurations
{
    public class SedeConfiguration : IEntityTypeConfiguration<Sede>
    {
        public void Configure(EntityTypeBuilder<Sede> builder)
        {
            builder.ToTable("Sedes");
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Direccion).HasMaxLength(500);
            builder.Property(x => x.EstadoId).IsRequired();
        }
    }
}