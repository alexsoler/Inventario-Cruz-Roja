using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Repositories
{
    public class ContactosRepository : BaseRepository<Contacto, int>, IContactosRepository
    {
        public ContactosRepository(DataContext context,
            ILogger<ContactosRepository> logger) : base(context, logger)
        {

        }
    }
}
