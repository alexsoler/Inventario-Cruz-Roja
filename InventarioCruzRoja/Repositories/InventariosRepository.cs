using InventarioCruzRoja.Data;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioCruzRoja.Repositories
{
    public class InventariosRepository : IInventariosRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<InventariosRepository> _logger;

        public InventariosRepository(DataContext context,
            ILogger<InventariosRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ServiceResponse<IEnumerable<InventarioDto>>> GetInventario(int? sedeId, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                var inventario = new List<InventarioDto>();
                if (sedeId is null || !sedeId.HasValue)
                {
                    inventario = await _context.Productos.Include(x => x.Sedes)
                        .Include(x => x.Egresos)
                        .Include(x => x.Ingresos.Where(i => !i.Anulado))
                        .Select(p => new InventarioDto
                        {
                            Codigo = p.Codigo,
                            Producto = p.Nombre,
                            Presentacion = p.Presentacion,
                            Estado = p.Estado.Nombre,
                            Sede = "Todas",
                            ImagenUrl = p.ImagenUrl,
                            Precio = p.Costo,
                            Stock = p.Ingresos.Where(i => !i.Anulado && 
                                        (!fechaDesde.HasValue || i.Fecha >= fechaDesde.Value) && 
                                        (!fechaHasta.HasValue || i.Fecha <= fechaHasta.Value)).Sum(x => x.Cantidad) - 
                                    p.Egresos.Where(e => !e.Anulado && 
                                        (!fechaDesde.HasValue || e.Fecha >= fechaDesde.Value) && 
                                        (!fechaHasta.HasValue || e.Fecha <= fechaHasta.Value)).Sum(x => x.Cantidad)
                        }).ToListAsync();
                }
                else
                {
                    inventario = await _context.Sedes
                        .Include(x => x.Productos).ThenInclude(x => x.Estado)
                        .Include(x => x.Productos).ThenInclude(x => x.Egresos)
                        .Include(x => x.Productos).ThenInclude(x => x.Ingresos).Where(x => x.Id == sedeId.Value)
                        .SelectMany(x => x.Productos, (x, p) => new InventarioDto
                        {
                            Codigo = p.Codigo,
                            Producto = p.Nombre,
                            Presentacion = p.Presentacion,
                            Estado = p.Estado.Nombre,
                            Sede = x.Nombre,
                            ImagenUrl = p.ImagenUrl,
                            Precio = p.Costo,
                            Stock = p.Ingresos.Where(i => i.SedeId == sedeId.Value && !i.Anulado && 
                                        (!fechaDesde.HasValue || i.Fecha >= fechaDesde.Value) && 
                                        (!fechaHasta.HasValue || i.Fecha <= fechaHasta.Value)).Sum(x => x.Cantidad) - 
                                    p.Egresos.Where(e => e.SedeId == sedeId.Value && !e.Anulado && 
                                        (!fechaDesde.HasValue || e.Fecha >= fechaDesde.Value) && 
                                        (!fechaHasta.HasValue || e.Fecha <= fechaHasta.Value)).Sum(x => x.Cantidad)
                        }).ToListAsync();
                }

                return new ServiceResponse<IEnumerable<InventarioDto>>
                {
                    Success = true,
                    Data = inventario
                };
            }

            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de buscar el proveedor.", ex);
                return new ServiceResponse<IEnumerable<InventarioDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
