using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioCruzRoja.Repositories
{
    public class EventosProductosRepository : BaseRepository<EventoProducto, int>, IEventosProductosRepository
    {
        public EventosProductosRepository(DataContext context, ILogger<BaseRepository<EventoProducto, int>> logger) : base(context, logger)
        {
        }

        public async Task EventoIngresoDeProducto(int productoId, int cantidad, string autor)
        {
            var evento = new EventoProducto
            {
                ProductoId = productoId,
                Descripcion = $"{autor} ingreso una cantidad de {cantidad}",
                Fecha = DateTime.Now
            };

            await Add(evento);
        }

        public async Task EventoEgresoDeProducto(int productoId, int cantidad, string autor)
        {
            var evento = new EventoProducto
            {
                ProductoId = productoId,
                Descripcion = $"{autor} egreso una cantidad de {cantidad}",
                Fecha = DateTime.Now
            };

            await Add(evento);
        }

        public async Task EventoAnulacionIngreso(int productoId, int cantidad, string autor)
        {
            var evento = new EventoProducto
            {
                ProductoId = productoId,
                Descripcion = $"{autor} anulo ingreso de una cantidad de {cantidad}",
                Fecha = DateTime.Now
            };

            await Add(evento);
        }

        public async Task EventoAnulacionEgreso(int productoId, int cantidad, string autor)
        {
            var evento = new EventoProducto
            {
                ProductoId = productoId,
                Descripcion = $"{autor} anulo egreso de una cantidad de {cantidad}",
                Fecha = DateTime.Now
            };

            await Add(evento);
        }

        public async Task EventoAgregarProducto(int productoId, string autor)
        {
            var evento = new EventoProducto
            {
                ProductoId = productoId,
                Descripcion = $"{autor} agrego el producto.",
                Fecha = DateTime.Now
            };

            await Add(evento);
        }

        public async Task EventoEditarProducto(int productoId, string autor)
        {
            var evento = new EventoProducto
            {
                ProductoId = productoId,
                Descripcion = $"{autor} edito el producto.",
                Fecha = DateTime.Now
            };

            await Add(evento);
        }

        public async Task EventoTrasladoProducto(int productoId, string autor)
        {
            var evento = new EventoProducto
            {
                ProductoId = productoId,
                Descripcion = $"{autor} traslado producto a otra sede.",
                Fecha = DateTime.Now
            };

            await Add(evento);
        }

        public async Task EventoAnulacionTraslado(int productoId, string autor)
        {
            var evento = new EventoProducto
            {
                ProductoId = productoId,
                Descripcion = $"{autor} anulo un traslado de producto",
                Fecha = DateTime.Now
            };

            await Add(evento);
        }

        public async Task<ServiceResponse<IEnumerable<EventoProducto>>> ObtenerEventosPorProducto(int productoId)
        {
            var response = new ServiceResponse<IEnumerable<EventoProducto>>();

            try
            {
                response.Data = await _context.EventosProductos.Where(x => x.ProductoId == productoId).ToListAsync();
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de obtener los eventos", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de obtener los eventos";

                return response;
            }
        }
    }
}
