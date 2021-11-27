using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Interfaces
{
    public interface IInventariosRepository
    {
        Task<ServiceResponse<IEnumerable<InventarioDto>>> GetInventario(int? sedeId);
    }
}
