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
                var fechaRestar = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var fechas = Enumerable.Range(0, 6).Select(x => fechaRestar.AddMonths(-x)).OrderBy(f => f).ToList();
                var fechaDesde = fechas.LastOrDefault();

                var datos = await _context.Productos.Where(x => x.FechaCreacion >= fechaDesde && x.EstadoId == 1)
                    .GroupBy(x => new { x.FechaCreacion.Month, x.FechaCreacion.Year })
                    .Select(x => new
                    {
                        x.Key.Month,
                        x.Key.Year,
                        Cantidad = x.Count()
                    }).ToListAsync();

                response.Data = fechas.GroupJoin(datos, f => new { f.Month, f.Year }, p => new { p.Month, p.Year }, (f, p_f) => new { f, p_f })
                    .SelectMany(t => t.p_f.DefaultIfEmpty(), (t, x)  => new ResumenProductosDto
                    {
                        Cantidad = x == null ? 0 : x.Cantidad,
                        Anio = t.f.Year,
                        Mes = t.f.Month
                    }).ToList();

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
