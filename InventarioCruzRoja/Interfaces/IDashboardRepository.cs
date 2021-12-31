using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Interfaces
{
    public interface IDashboardRepository
    {
        Task<ServiceResponse<IEnumerable<ResumenProductosDto>>> ObtenerResumenDeProductos();
        Task<ServiceResponse<IEnumerable<ResumenIngresosDto>>> ObtenerResumenDeIngresos();
        Task<ServiceResponse<IEnumerable<ResumenEgresosDto>>> ObtenerResumenDeEgresos();
        Task<ServiceResponse<int>> ObtenerCantidadDeUsuarios();
        Task<ServiceResponse<int>> ObtenerCantidadDeProveedores();
        Task<ServiceResponse<int>> ObtenerCantidadDeSedes();
        Task<ServiceResponse<int>> ObtenerCantidadDeProductos();
        Task<ServiceResponse<IEnumerable<EventoProducto>>> ObtenerUltimosEventos();
    }
}
