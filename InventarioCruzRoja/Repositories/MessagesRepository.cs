using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Repositories
{
    public class MessagesRepository : BaseRepository<Message, int>, IMessagesRepository
    {
        public MessagesRepository(DataContext context, ILogger<BaseRepository<Message, int>> logger) : base(context, logger)
        {
        }
    }
}
