﻿using InventarioCruzRoja.Data.EntityConfigurations;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Contacto> Contactos { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<Egreso> Egresos { get; set; }
        public DbSet<Traslado> Traslados { get; set; }
        public DbSet<EventoProducto> EventosProductos { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoConfiguration());
            modelBuilder.ApplyConfiguration(new FabricanteConfiguration());
            modelBuilder.ApplyConfiguration(new EstadoConfiguration());
            modelBuilder.ApplyConfiguration(new SedeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new ProveedorConfiguration());
            modelBuilder.ApplyConfiguration(new ContactoConfiguration());
            modelBuilder.ApplyConfiguration(new IngresoConfiguration());
            modelBuilder.ApplyConfiguration(new EgresoConfiguration());
            modelBuilder.ApplyConfiguration(new TrasladosConfiguration());
            modelBuilder.ApplyConfiguration(new EventoProductoConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
