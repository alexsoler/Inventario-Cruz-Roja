using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Repositories
{
    public class EgresosRepository : BaseRepository<Egreso, int>, IEgresosRepository
    {
        public EgresosRepository(DataContext context, ILogger<EgresosRepository> logger) : base(context, logger)
        {
        }

        public override async Task<ServiceResponse<Egreso>> Add(Egreso entity)
        {
            entity.Fecha = DateTime.Now;

            var inventario = await  _context.Productos.Include(x => x.Ingresos).Include(x => x.Egresos)
                .Where(x => x.Id == entity.ProductoId).Select(x => new 
                {
                    Ingresos = x.Ingresos.Where(i => i.SedeId == entity.SedeId).Sum(i => i.Cantidad),
                    Egresos = x.Egresos.Where(e => e.SedeId == entity.SedeId).Sum(e => e.Cantidad)
                }).FirstOrDefaultAsync();


            if (entity.Cantidad > inventario.Ingresos - inventario.Egresos)
            {
                return new ServiceResponse<Egreso>()
                {
                    Success = false,
                    Message = $"El inventario actual del producto es de {inventario.Ingresos - inventario.Egresos}. No puede retirar una cantidad mayor."
                };
            }

            return await base.Add(entity);
        }
    }
}
