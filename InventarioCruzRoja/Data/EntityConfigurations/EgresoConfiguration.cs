using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioCruzRoja.Data.EntityConfigurations
{
    public class EgresoConfiguration : IEntityTypeConfiguration<Egreso>
    {
        public void Configure(EntityTypeBuilder<Egreso> builder)
        {
            builder.ToTable("Egresos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Observaciones).HasMaxLength(300);
            builder.Property(x => x.MotivoAnula).HasMaxLength(300);
            builder.Property(x => x.Fecha).IsRequired();
        }
    }
}
