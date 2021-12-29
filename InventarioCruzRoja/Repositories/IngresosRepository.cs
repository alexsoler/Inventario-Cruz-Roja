using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Repositories
{
    public class IngresosRepository : BaseRepository<Ingreso, int>, IIngresosRepository
    {
        public IngresosRepository(DataContext context, ILogger<IngresosRepository> logger) : base(context, logger)
        {
        }

        public async override Task<ServiceResponse<Ingreso>> Add(Ingreso entity)
        {
            entity.Fecha = DateTime.Now;
            return await base.Add(entity);
        }

        public async override Task<ServiceResponse<Ingreso>> Update(Ingreso entidad)
        {
            entidad.FechaAnula = DateTime.Now;
            return await base.Update(entidad);
        }
    }
}
