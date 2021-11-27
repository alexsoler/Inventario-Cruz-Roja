using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Repositories
{
    public class CategoriasRepository : BaseRepository<Categoria, int>, ICategoriasRepository
    {
        public CategoriasRepository(DataContext context,
            ILogger<CategoriasRepository> logger) : base(context, logger)
        {

        }
    }
}
