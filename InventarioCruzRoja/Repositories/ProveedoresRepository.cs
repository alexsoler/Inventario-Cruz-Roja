using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Repositories
{
    public class ProveedoresRepository : BaseRepository<Proveedor, int>, IProveedoresRepository
    {
        public ProveedoresRepository(DataContext context,
            ILogger<ProveedoresRepository> logger) : base(context, logger)
        {

        }

        public async Task<ServiceResponse<IEnumerable<Proveedor>>> GetSearch(string filter, params string[] includes)
        {
            var response = new ServiceResponse<IEnumerable<Proveedor>>();

            try
            {
                var query = _context.Proveedores.AsNoTracking();
                query = includes.Aggregate(query, (query, path) => query.Include(path));
                var proveedores = await query.Where(x =>
                        EF.Functions.Like(x.Nombre, $"%{filter}%")
                    ).AsNoTracking().ToListAsync();

                response.Message = "Lista de registros obtenida con exito";
                response.Data = proveedores;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de buscar el proveedor.", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de buscar el proveedor.";

                return response;
            }
        }
    }
}
