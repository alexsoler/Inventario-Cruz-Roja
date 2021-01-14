using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.Extensions.Logging;

namespace InventarioCruzRoja.Repositories
{
    public class FabricantesRepository : BaseRepository<Fabricante, int>, IFabricantesRepository
    {
        public FabricantesRepository(DataContext context,
            ILogger<FabricantesRepository> logger)
            : base(context, logger)
        {   
        }
    }
}