using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventarioCruzRoja.Data;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventarioCruzRoja.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<RolesRepository> _logger;

        public RolesRepository(DataContext context,
            IMapper mapper,
            ILogger<RolesRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<RoleDto>> Add(Role role)
        {
            var response = new ServiceResponse<RoleDto>();

            try
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<RoleDto>(role);
                response.Message = "Rol agregado con exito";

                return response;   
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("No se pudo agregar el rol", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de agregar el rol";

                return response;
            }
        }

        public async Task<ServiceResponse<RoleDto>> Delete(int id)
        {
            var response = new ServiceResponse<RoleDto>();

            var role = await _context.Roles.FindAsync(id);

            if (role == null) 
            {
                response.Success = false;
                response.Message = "El role a eliminar no fue encontrado";
                return response;
            }

            try
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<RoleDto>(role);
                response.Message = "Rol eliminado con exito";

                return response;   
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("No se pudo eliminar el rol", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de eliminar el rol";

                return response;
            }
        }

        public async Task<ServiceResponse<RoleDto>> Get(int id)
        {
            var response = new ServiceResponse<RoleDto>();

            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                response.Success = false;
                response.Message = "No se encontro el rol";
                return response;
            }

            response.Message = "Rol encontrado con exito";
            response.Data = _mapper.Map<RoleDto>(role);

            return response;
        }

        public async Task<ServiceResponse<IEnumerable<RoleDto>>> GetAll()
        {
            var response = new ServiceResponse<IEnumerable<RoleDto>>();
            
            try
            {
                var roles = await _context.Roles.AsNoTracking().ToListAsync();

                response.Message = "Lista de roles obtenida con exito";
                response.Data = _mapper.Map<IEnumerable<RoleDto>>(roles);

                return response;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Ocurrio un error al momento de obtener todos los roles", ex);
                response.Success = false;
                response.Message = "Ocurrio un error al momento de obtener todos los roles";

                return response;
            }
        }

        public async Task<ServiceResponse<int>> Update(Role role)
        {
            var response = new ServiceResponse<int>();

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                response.Data = role.Id;
                response.Message = "El role fue modificado con exito.";

                return response;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!RoleExists(role.Id))
                {
                    response.Success = false;
                    response.Message = "El role no pudo ser modificado porque no existe.";

                    return response;
                }
                else
                {
                    _logger.LogError("Ocurrio una excepcion al momento de editar un role", ex);
                    response.Success = false;
                    response.Message = "Ocurrio un error al momento de eliminar el role.";

                    return response;
                }
            }
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}