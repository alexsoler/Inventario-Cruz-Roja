using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioCruzRoja.Data.EntityConfigurations
{
    public class FabricanteConfiguration : IEntityTypeConfiguration<Fabricante>
    {
        public void Configure(EntityTypeBuilder<Fabricante> builder)
        {
            builder.ToTable("Fabricantes");
            builder.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
            builder.Property(x => x.EstadoId).IsRequired();
        }
    }
}