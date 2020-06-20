using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Interfaces
{
    public interface IRolesRepository
    {
        Task<ServiceResponse<IEnumerable<RoleDto>>> GetAll();
        Task<ServiceResponse<RoleDto>> Get(int id);
        Task<ServiceResponse<RoleDto>> Add(Role role);
        Task<ServiceResponse<int>> Update(Role role);
        Task<ServiceResponse<RoleDto>> Delete(int id);
    }
}