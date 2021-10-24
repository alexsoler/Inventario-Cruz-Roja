﻿// <auto-generated />
using System;
using InventarioCruzRoja.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InventarioCruzRoja.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("InventarioCruzRoja.Models.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<int>("EstadoId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("EstadoId");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Contacto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("ProveedorId")
                        .HasColumnType("int");

                    b.Property<string>("Telefono")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.HasKey("Id");

                    b.HasIndex("ProveedorId");

                    b.ToTable("Contactos");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Egreso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Anulado")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FechaAnula")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MotivoAnula")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<int>("ProductoId")
                        .HasColumnType("int");

                    b.Property<int>("SedeId")
                        .HasColumnType("int");

                    b.Property<int?>("UserAnulaId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductoId");

                    b.HasIndex("SedeId");

                    b.HasIndex("UserAnulaId");

                    b.HasIndex("UserId");

                    b.ToTable("Egresos");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Estado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Estados");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Fabricante", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EstadoId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("EstadoId");

                    b.ToTable("Fabricantes");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Ingreso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Anulado")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FechaAnula")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MotivoAnula")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<int>("ProductoId")
                        .HasColumnType("int");

                    b.Property<int?>("ProveedorId")
                        .HasColumnType("int");

                    b.Property<int>("SedeId")
                        .HasColumnType("int");

                    b.Property<int?>("UserAnulaId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductoId");

                    b.HasIndex("ProveedorId");

                    b.HasIndex("SedeId");

                    b.HasIndex("UserAnulaId");

                    b.HasIndex("UserId");

                    b.ToTable("Ingresos");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Producto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("Codigo")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<decimal>("Costo")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<int>("EstadoId")
                        .HasColumnType("int");

                    b.Property<int>("FabricanteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ImagenUrl")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Modelo")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Presentacion")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UsuarioModifica")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("EstadoId");

                    b.HasIndex("FabricanteId");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Proveedor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("EstadoId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("SitioWeb")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("TelefonoFijo1")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("TelefonoFijo2")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.HasKey("Id");

                    b.HasIndex("EstadoId");

                    b.ToTable("Proveedores");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Sede", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Direccion")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<int>("EstadoId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("EstadoId");

                    b.ToTable("Sedes");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Traslado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Anulado")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("EgresoOrigenId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FechaAnula")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IngresoDestinoId")
                        .HasColumnType("int");

                    b.Property<string>("MotivoAnula")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<int>("ProductoId")
                        .HasColumnType("int");

                    b.Property<int?>("UserAnulaId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EgresoOrigenId");

                    b.HasIndex("IngresoDestinoId");

                    b.HasIndex("ProductoId");

                    b.HasIndex("UserAnulaId");

                    b.HasIndex("UserId");

                    b.ToTable("Traslados");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("longblob");

                    b.Property<string>("Username")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("ProductoSede", b =>
                {
                    b.Property<int>("ProductosId")
                        .HasColumnType("int");

                    b.Property<int>("SedesId")
                        .HasColumnType("int");

                    b.HasKey("ProductosId", "SedesId");

                    b.HasIndex("SedesId");

                    b.ToTable("ProductoSede");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Categoria", b =>
                {
                    b.HasOne("InventarioCruzRoja.Models.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estado");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Contacto", b =>
                {
                    b.HasOne("InventarioCruzRoja.Models.Proveedor", "Proveedor")
                        .WithMany("Contactos")
                        .HasForeignKey("ProveedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Proveedor");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Egreso", b =>
                {
                    b.HasOne("InventarioCruzRoja.Models.Producto", "Producto")
                        .WithMany("Egresos")
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventarioCruzRoja.Models.Sede", "Sede")
                        .WithMany()
                        .HasForeignKey("SedeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventarioCruzRoja.Models.User", "UserAnula")
                        .WithMany()
                        .HasForeignKey("UserAnulaId");

                    b.HasOne("InventarioCruzRoja.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");

                    b.Navigation("Sede");

                    b.Navigation("User");

                    b.Navigation("UserAnula");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Fabricante", b =>
                {
                    b.HasOne("InventarioCruzRoja.Models.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estado");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Ingreso", b =>
                {
                    b.HasOne("InventarioCruzRoja.Models.Producto", "Producto")
                        .WithMany("Ingresos")
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventarioCruzRoja.Models.Proveedor", "Proveedor")
                        .WithMany()
                        .HasForeignKey("ProveedorId");

                    b.HasOne("InventarioCruzRoja.Models.Sede", "Sede")
                        .WithMany()
                        .HasForeignKey("SedeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventarioCruzRoja.Models.User", "UserAnula")
                        .WithMany()
                        .HasForeignKey("UserAnulaId");

                    b.HasOne("InventarioCruzRoja.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");

                    b.Navigation("Proveedor");

                    b.Navigation("Sede");

                    b.Navigation("User");

                    b.Navigation("UserAnula");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Producto", b =>
                {
                    b.HasOne("InventarioCruzRoja.Models.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventarioCruzRoja.Models.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventarioCruzRoja.Models.Fabricante", "Fabricante")
                        .WithMany("Productos")
                        .HasForeignKey("FabricanteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Estado");

                    b.Navigation("Fabricante");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Proveedor", b =>
                {
                    b.HasOne("InventarioCruzRoja.Models.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estado");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Sede", b =>
                {
                    b.HasOne("InventarioCruzRoja.Models.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estado");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Traslado", b =>
                {
                    b.HasOne("InventarioCruzRoja.Models.Egreso", "EgresoOrigen")
                        .WithMany()
                        .HasForeignKey("EgresoOrigenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventarioCruzRoja.Models.Ingreso", "IngresoDestino")
                        .WithMany()
                        .HasForeignKey("IngresoDestinoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventarioCruzRoja.Models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventarioCruzRoja.Models.User", "UserAnula")
                        .WithMany()
                        .HasForeignKey("UserAnulaId");

                    b.HasOne("InventarioCruzRoja.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EgresoOrigen");

                    b.Navigation("IngresoDestino");

                    b.Navigation("Producto");

                    b.Navigation("User");

                    b.Navigation("UserAnula");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.UserRole", b =>
                {
                    b.HasOne("InventarioCruzRoja.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventarioCruzRoja.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProductoSede", b =>
                {
                    b.HasOne("InventarioCruzRoja.Models.Producto", null)
                        .WithMany()
                        .HasForeignKey("ProductosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventarioCruzRoja.Models.Sede", null)
                        .WithMany()
                        .HasForeignKey("SedesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Fabricante", b =>
                {
                    b.Navigation("Productos");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Producto", b =>
                {
                    b.Navigation("Egresos");

                    b.Navigation("Ingresos");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Proveedor", b =>
                {
                    b.Navigation("Contactos");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("InventarioCruzRoja.Models.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
