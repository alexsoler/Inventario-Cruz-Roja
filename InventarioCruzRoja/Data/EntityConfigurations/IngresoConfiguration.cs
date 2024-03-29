﻿using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioCruzRoja.Data.EntityConfigurations
{
    public class IngresoConfiguration : IEntityTypeConfiguration<Ingreso>
    {
        public void Configure(EntityTypeBuilder<Ingreso> builder)
        {
            builder.ToTable("Ingresos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Observaciones).HasMaxLength(300);
            builder.Property(x => x.MotivoAnula).HasMaxLength(300);
            builder.Property(x => x.Fecha).IsRequired();
        }
    }
}
