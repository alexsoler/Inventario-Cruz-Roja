using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Interfaces
{
    public interface ISedesRepository : IBaseRepository<Sede>
    {
        Task<ServiceResponse<IEnumerable<Sede>>> GetSearch(string filter, params string[] includes);
    }
}