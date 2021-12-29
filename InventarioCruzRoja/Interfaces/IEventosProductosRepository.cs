using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Interfaces
{
    public interface IEventosProductosRepository : IBaseRepository<EventoProducto>
    {
        Task EventoIngresoDeProducto(int productoId, int cantidad, string autor);
        Task EventoEgresoDeProducto(int productoId, int cantidad, string autor);
        Task EventoAnulacionIngreso(int productoId, int cantidad, string autor);
        Task EventoAnulacionEgreso(int productoId, int cantidad, string autor);
        Task EventoAgregarProducto(int productoId, string autor);
        Task EventoEditarProducto(int productoId, string autor);
        Task<ServiceResponse<IEnumerable<EventoProducto>>> ObtenerEventosPorProducto(int productoId);
    }
}
