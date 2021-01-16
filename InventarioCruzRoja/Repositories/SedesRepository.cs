using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.Extensions.Logging;

namespace InventarioCruzRoja.Repositories
{
    public class SedesRepository : BaseRepository<Sede, int>, ISedesRepository
    {
        public SedesRepository(DataContext dataContext,
            ILogger<SedesRepository> logger) : base(dataContext, logger)
        {
            
        }
    }
}