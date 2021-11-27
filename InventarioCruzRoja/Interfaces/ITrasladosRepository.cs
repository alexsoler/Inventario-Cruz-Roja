using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Interfaces
{
    public interface ITrasladosRepository : IBaseRepository<Traslado>
    {
        Task<ServiceResponse<Traslado>> AddFromDto(TrasladoDto entity);
    }
}
