using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;

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
    }
}
