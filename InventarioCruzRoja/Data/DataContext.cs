﻿using InventarioCruzRoja.Data.EntityConfigurations;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Fabricante> Fabricantes { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Sede> Sedes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoConfiguration());
            modelBuilder.ApplyConfiguration(new FabricanteConfiguration());
            modelBuilder.ApplyConfiguration(new EstadoConfiguration());
            modelBuilder.ApplyConfiguration(new SedeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
