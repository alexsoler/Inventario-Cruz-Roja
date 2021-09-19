using System.Collections.Generic;
using System.Threading.Tasks;
using InventarioCruzRoja.Models;
using Microsoft.AspNetCore.Http;

namespace InventarioCruzRoja.Interfaces
{
    public interface IProductosRepository : IBaseRepository<Producto>
    {
        Task<ServiceResponse<string>> GuardarImagen(IFormFile file);
        Task<ServiceResponse<IEnumerable<Producto>>> GetSearch(string filter, params string[] includes);
    }
}