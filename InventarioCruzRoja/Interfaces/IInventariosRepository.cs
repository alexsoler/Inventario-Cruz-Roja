using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Interfaces
{
    public interface IInventariosRepository
    {
        Task<ServiceResponse<IEnumerable<InventarioDto>>> GetInventario(int? sedeId);
    }
}
