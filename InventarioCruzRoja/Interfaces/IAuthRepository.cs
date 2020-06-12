﻿using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Interfaces
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<LoginResponseDto>> Login(string username, string password);
        Task<bool> UserExists(string username);
        Task<ServiceResponse<bool>> AddRoleToUser(int idUser, params string[] roles);
        Task<ServiceResponse<IEnumerable<UserTableDto>>> GetUsers();
        Task<ServiceResponse<IEnumerable<string>>> GetRoles();
        Task EditUser(int id, UserDto user);
        Task<ServiceResponse<int>> DeleteUser(int id);
        Task RemoveRole(int id);
    }
}
