using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Repositories
{
    public class EgresosRepository : BaseRepository<Egreso, int>, IEgresosRepository
    {
        public EgresosRepository(DataContext context, ILogger<EgresosRepository> logger) : base(context, logger)
        {
        }

        public override Task<ServiceResponse<Egreso>> Add(Egreso entity)
        {
            entity.Fecha = DateTime.Now;
            return base.Add(entity);
        }
    }
}
