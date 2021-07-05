using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventarioCruzRoja.Repositories
{
    public class ProductosRepository : BaseRepository<Producto, int>, IProductosRepository
    {
        private readonly IWebHostEnvironment _environment;

        public ProductosRepository(DataContext context,
            IWebHostEnvironment environment,
            ILogger<ProductosRepository> logger)
            : base(context, logger)
        {
            _environment = environment;
        }

        public override async Task<ServiceResponse<Producto>> Add(Producto entity)
        {
            entity.FechaCreacion = DateTime.Now;
            entity.FechaModificacion = DateTime.Now;

            return await base.Add(entity);
        }

        public override async Task<ServiceResponse<Producto>> Update(Producto entidad)
        {
            var urlImagenPrevia = _context.Productos.AsNoTracking().FirstOrDefault(x => x.Id == entidad.Id).ImagenUrl;

            var response = await base.Update(entidad);

            if (!string.IsNullOrEmpty(urlImagenPrevia))
            {
                var fileToDelete = Path.Combine(_environment.ContentRootPath, urlImagenPrevia.Remove(0,1).Replace("/", "\\"));

                if (File.Exists(fileToDelete) && response.Success && urlImagenPrevia != entidad.ImagenUrl)
                {
                    File.Delete(fileToDelete);
                }
            }

            return response;
        }

        public override async Task<ServiceResponse<Producto>> Delete(object id)
        {
            var response = await base.Delete(id);

            if (response.Success)
            {
                var fileToDelete = Path.Combine(_environment.ContentRootPath, response.Data.ImagenUrl.Remove(0, 1).Replace("/", "\\"));

                if (File.Exists(fileToDelete))
                    File.Delete(fileToDelete);
            }

            return response;
        }        

        public async Task<ServiceResponse<string>> GuardarImagen(IFormFile file)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(_environment.ContentRootPath, folderName);
                Directory.CreateDirectory(pathToSave);
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
                else
                {
                    response.Success = true;
                    response.Data = string.Empty;
                }

                return response;
            }
            catch (System.Exception ex)
            {
                response.Success = false;
                response.Data = string.Empty;
                
                return response;
            }
        }
    }
}