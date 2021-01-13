using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioCruzRoja.Data.EntityConfigurations
{
    public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder.ToTable("Estados");
            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
        }
    }
}