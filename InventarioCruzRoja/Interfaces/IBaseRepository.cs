using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<ServiceResponse<IEnumerable<T>>> GetAll(params string[] includes);
        Task<ServiceResponse<T>> Get(object id, params string[] includes);
        Task<ServiceResponse<T>> Add(T role);
        Task<ServiceResponse<T>> Update(T role);
        Task<ServiceResponse<T>> Delete(object id);
    }
}