using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InventarioCruzRoja.Repositories
{
    public class ProductosRepository : BaseRepository<Producto, string>, IProductosRepository
    {
        public ProductosRepository(DataContext context,
            ILogger<ProductosRepository> logger)
            : base(context, logger)
        {

        }

        public async Task<ServiceResponse<string>> GuardarImagen(IFormFile file)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        response.Success = true;
                        response.Data = dbPath;
                    }
                }

                return response;
            }
            catch (System.Exception)
            {
                response.Success = false;
                response.Data = string.Empty;
                
                return response;
            }
        }
    }
}