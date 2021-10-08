using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioCruzRoja.Data.EntityConfigurations
{
    public class TrasladosConfiguration : IEntityTypeConfiguration<Traslado>
    {
        public void Configure(EntityTypeBuilder<Traslado> builder)
        {
            builder.ToTable("Traslados");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Observaciones).HasMaxLength(300);
            builder.Property(x => x.MotivoAnula).HasMaxLength(300);
            builder.Property(x => x.Fecha).IsRequired();
        }
    }
}
