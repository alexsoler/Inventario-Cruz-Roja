using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Models;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Interfaces
{
    public interface ITrasladosRepository : IBaseRepository<Traslado>
    {
        Task<ServiceResponse<Traslado>> AddFromDto(TrasladoDto entity);
    }
}
