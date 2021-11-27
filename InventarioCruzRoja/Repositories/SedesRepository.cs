using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioCruzRoja.Repositories
{
    public class SedesRepository : BaseRepository<Sede, int>, ISedesRepository
    {
        public SedesRepository(DataContext dataContext,
            ILogger<SedesRepository> logger) : base(dataContext, logger)
        {

        }

        public async Task<ServiceResponse<IEnumerable<Sede>>> GetSearch(string filter, params string[] includes)
        {
            var response = new ServiceResponse<IEnumerable<Sede>>();

            try
            {
                var sedes = await _context.Sedes.Where(x =>
                        EF.Functions.Like(x.Nombre, $"%{filter}%")
                    ).AsNoTracking().ToListAsync();

                response.Message = "Lista de registros obtenida con exito";
                response.Data = sedes;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de buscar la sede.", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de buscar la sede.";

                return response;
            }
        }
    }
}