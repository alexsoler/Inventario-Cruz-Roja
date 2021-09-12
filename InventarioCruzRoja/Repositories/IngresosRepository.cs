using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.Extensions.Logging;

namespace InventarioCruzRoja.Repositories
{
    public class IngresosRepository : BaseRepository<Ingreso, int>, IIngresosRepository
    {
        public IngresosRepository(DataContext context, ILogger<IngresosRepository> logger) : base(context, logger)
        {
        }
    }
}
