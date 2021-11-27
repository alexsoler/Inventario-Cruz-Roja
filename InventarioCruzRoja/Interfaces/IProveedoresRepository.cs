using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Interfaces
{
    public interface IProveedoresRepository : IBaseRepository<Proveedor>
    {
        Task<ServiceResponse<IEnumerable<Proveedor>>> GetSearch(string filter, params string[] includes);
    }
}
