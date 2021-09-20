using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Repositories
{
    public class IngresosRepository : BaseRepository<Ingreso, int>, IIngresosRepository
    {
        public IngresosRepository(DataContext context, ILogger<IngresosRepository> logger) : base(context, logger)
        {
        }

        public override Task<ServiceResponse<Ingreso>> Add(Ingreso entity)
        {
            entity.Fecha = DateTime.Now;
            return base.Add(entity);
        }
    }
}
