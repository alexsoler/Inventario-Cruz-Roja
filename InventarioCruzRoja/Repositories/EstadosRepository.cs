using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Repositories
{
    public class EstadosRepository : BaseRepository<Estado, int>, IEstadosRepository
    {
        public EstadosRepository(DataContext context,
            ILogger<EstadosRepository> logger)
            : base(context, logger)
        {

        }
    }
}
