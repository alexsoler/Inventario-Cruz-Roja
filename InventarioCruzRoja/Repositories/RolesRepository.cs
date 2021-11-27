using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Repositories
{
    public class RolesRepository : BaseRepository<Role, int>, IRolesRepository
    {
        public RolesRepository(DataContext context,
            ILogger<RolesRepository> logger)
            : base(context, logger)
        {
        }
    }
}