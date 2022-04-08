using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioCruzRoja.Data
{
    public static class SeedDatabase
    {
        public static async Task Seed(IServiceProvider services)
        {
            var _db = services.GetRequiredService<DataContext>();
            var _authRepository = services.GetRequiredService<IAuthRepository>();

            if (!await _db.Roles.AnyAsync())
                _db.Roles.AddRange(
                    new Role { Name = "Administrador" },
                    new Role { Name = "employee" }
                );

            if (!await _db.Estados.AnyAsync())
                _db.Estados.AddRange(
                    new Estado { Nombre = "Activo" },
                    new Estado { Nombre = "Inactivo" }
                );

            await _db.SaveChangesAsync();

            if (!await _db.Fabricantes.AnyAsync())
                _db.Fabricantes.AddRange(
                    new Fabricante { Nombre = "EDUP", EstadoId = 1 }
                );

            if (!await _db.Sedes.AnyAsync())
                _db.Sedes.AddRange(
                    new Sede { Nombre = "Comayagua", EstadoId = 1, Direccion = "Meh", Latitud = 14.450453905319334, Longitud = -87.64308882541238 },
                    new Sede { Nombre = "Tegucigalpa", EstadoId = 1, Direccion = "Meh", Latitud = 14.090445738332303, Longitud = -87.22336054676272 },
                    new Sede { Nombre = "SPS", EstadoId = 1, Direccion = "Meh", Latitud = 15.499901502837202, Longitud = -88.0297447643924 }
                );

            if (!await _db.Categorias.AnyAsync())
                _db.Categorias.AddRange(
                    new Categoria { Nombre = "Cómputo", Descripcion = "", EstadoId = 1 },
                    new Categoria { Nombre = "Medicamentos", Descripcion = "", EstadoId = 1 }
                );

            await _db.SaveChangesAsync();

            if (!await _db.Productos.AnyAsync())
                _db.Productos.AddRange(new Producto
                {
                    Codigo = "X0012T4ZKF",
                    Nombre = "WIFI Adapter",
                    Modelo = "EP-DB1607,",
                    Presentacion = "UNIDAD",
                    Descripcion = "Ejemplo de descripción",
                    Observaciones = "Ejemplo de Observaciones",
                    Costo = 650.99M,
                    Fabricante = await _db.Fabricantes.FirstOrDefaultAsync(),
                    Categoria = await _db.Categorias.FirstOrDefaultAsync(),
                    Estado = await _db.Estados.FirstOrDefaultAsync(),
                    Sedes = await _db.Sedes.ToListAsync(),
                    FechaCreacion = DateTime.Now
                });

            if (!await _db.Proveedores.AnyAsync())
                _db.Proveedores.AddRange(new Proveedor
                {
                    Nombre = "Proveedor de Ejemplo",
                    Direccion = "Dirección de ejemplo",
                    Estado = await _db.Estados.FirstOrDefaultAsync()
                });

            await _db.SaveChangesAsync();

            var resultCreateUser1 = await _authRepository.Register(new User { Username = "demouser" }, "Password");

            if (resultCreateUser1.Success)
            {
                await _authRepository.AddRoleToUser(resultCreateUser1.Data, "Administrador");
            }


        }
    }
}
