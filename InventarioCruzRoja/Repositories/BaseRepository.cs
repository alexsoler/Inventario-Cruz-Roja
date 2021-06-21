using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventarioCruzRoja.Repositories
{
    public class BaseRepository<T, E> : IBaseRepository<T> 
        where T : EntidadBase<E>
    {  
        protected readonly DataContext _context;
        protected readonly ILogger<BaseRepository<T,E>> _logger;

        public BaseRepository(DataContext context,
            ILogger<BaseRepository<T,E>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public virtual async Task<ServiceResponse<T>> Add(T entity)
        {
            var response = new ServiceResponse<T>();

            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();

                response.Data = entity;
                response.Message = "Registro agregado con exito";

                return response;   
            }
            catch (Exception ex)
            {
                _logger.LogError("No se pudo agregar el registro", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de agregar el registro";

                return response;
            }
        }

        public virtual async Task<ServiceResponse<T>> Delete(object id)
        {
            var response = new ServiceResponse<T>();

            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null) 
            {
                response.Success = false;
                response.Message = "El registro a eliminar no fue encontrado";
                return response;
            }

            try
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();

                response.Data = entity;
                response.Message = "Registro eliminado con exito";

                return response;   
            }
            catch (Exception ex)
            {
                _logger.LogError("No se pudo eliminar el registro", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de eliminar el registro";

                return response;
            }
        }

        public virtual async Task<ServiceResponse<T>> Get(object id)
        {
            var response = new ServiceResponse<T>();

            var entidad = await _context.Set<T>().FindAsync(id);

            if (entidad == null)
            {
                response.Success = false;
                response.Message = "No se encontro el registro";
                return response;
            }

            response.Message = "Registro encontrado con exito";
            response.Data = entidad;

            return response;
        }

        public virtual async Task<ServiceResponse<IEnumerable<T>>> GetAll(params string[] includes)
        {
            var response = new ServiceResponse<IEnumerable<T>>();
            
            try
            {

                var query = _context.Set<T>().AsNoTracking();
                query = includes.Aggregate(query, (query, path) => query.Include(path));
                var entidades = await query.ToListAsync();

                response.Message = "Lista de registros obtenida con exito";
                response.Data = entidades;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al momento de obtener todos los registros", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de obtener todos los registros";

                return response;
            }
        }

        public virtual async Task<ServiceResponse<T>> Update(T entidad)
        {
            var response = new ServiceResponse<T>();

            _context.Entry(entidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                response.Data = entidad;
                response.Message = "El registro fue modificado con exito.";

                return response;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!RegisterExists(entidad.Id))
                {
                    response.Success = false;
                    response.Message = "El role no pudo ser modificado porque no existe.";

                    return response;
                }
                else
                {
                    _logger.LogError("Ocurrio una excepcion al momento de editar un registro", ex);
                    response.Success = false;
                    response.Message = "Ocurrio un error al momento de editar el registro.";

                    return response;
                }
            }
        }

        private bool RegisterExists(E id)
        {
            return _context.Set<T>().Any(e => e.Id.Equals(id));
        }
    }
}