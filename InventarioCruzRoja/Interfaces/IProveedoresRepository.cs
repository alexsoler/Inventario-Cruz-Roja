using InventarioCruzRoja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Interfaces
{
    public interface IProveedoresRepository : IBaseRepository<Proveedor>
    {
        Task<ServiceResponse<IEnumerable<Proveedor>>> GetSearch(string filter, params string[] includes);
    }
}
