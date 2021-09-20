using System;
using System.Collections.Generic;
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

            if (entity.Sedes != null && entity.Sedes.Count > 0)
            {
                _context.AttachRange(entity.Sedes);
            }

            return await base.Add(entity);
        }

        public override async Task<ServiceResponse<Producto>> Update(Producto entidad)
        {
            var producto = await _context.Productos.Include(x => x.Sedes).FirstOrDefaultAsync(x => x.Id == entidad.Id);
            var urlImagenPrevia = producto.ImagenUrl;
            _context.Entry(producto).CurrentValues.SetValues(entidad);

            var sedesToDelete = producto.Sedes.Where(s => !entidad.Sedes.Any(x => x.Id == s.Id)).ToList();
            sedesToDelete.ForEach(x => producto.Sedes.Remove(x));

            var sedesToAdd = entidad.Sedes.Where(s => !producto.Sedes.Any(x => x.Id == s.Id)).ToList();
            _context.AttachRange(sedesToAdd);
            sedesToAdd.ForEach(x => producto.Sedes.Add(x));

            var response = await base.Update(producto);

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

            if (response.Success && !string.IsNullOrEmpty(response.Data.ImagenUrl))
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
                _logger.LogError(ex.Message, ex);
                response.Success = false;
                response.Data = string.Empty;
                
                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<Producto>>> GetSearch(string filter, params string[] includes)
        {
            var response = new ServiceResponse<IEnumerable<Producto>>();

            try
            {
                var query = _context.Productos.AsNoTracking();
                query = includes.Aggregate(query, (query, path) => query.Include(path));
                var productos = await query.Where(x =>
                        EF.Functions.Like(x.Nombre, $"%{filter}%") ||
                        EF.Functions.Like(x.Codigo, $"%{filter}%")
                    ).AsNoTracking().ToListAsync();

                response.Message = "Lista de registros obtenida con exito";
                response.Data = productos;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de buscar el producto.", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de buscar el producto.";

                return response;
            }
        }
    }
}