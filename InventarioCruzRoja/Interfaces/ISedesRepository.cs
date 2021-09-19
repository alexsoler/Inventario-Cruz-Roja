using InventarioCruzRoja.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Interfaces
{
    public interface ISedesRepository : IBaseRepository<Sede>
    {
        Task<ServiceResponse<IEnumerable<Sede>>> GetSearch(string filter, params string[] includes);
    }
}