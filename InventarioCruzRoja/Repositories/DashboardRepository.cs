using InventarioCruzRoja.Data;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioCruzRoja.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<DashboardRepository> _logger;

        public DashboardRepository(DataContext context,
            ILogger<DashboardRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ServiceResponse<IEnumerable<ResumenProductosDto>>> ObtenerResumenDeProductos()
        {
            var response = new ServiceResponse<IEnumerable<ResumenProductosDto>>();
            try
            {
                var fechaDesde = DateTime.Now.AddMonths(-7);
                response.Data = await _context.Productos.Where(x => x.FechaCreacion >= fechaDesde.Date && x.EstadoId == 1)
                    .GroupBy(x => new { x.FechaCreacion.Month, x.FechaCreacion.Year })
                    .Select(x => new ResumenProductosDto
                    {
                        Cantidad = x.Count(),
                        Anio = x.Key.Year,
                        Mes = x.Key.Month
                    }).ToListAsync();
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de obtener el resumen de productos", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de obtener el resumen de productos";

                return response;
            }
        }
    }
}
