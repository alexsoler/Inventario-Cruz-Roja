using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Data
{
    public class SeedDatabase
    {
        public static async Task Seed(IServiceProvider services)
        {
            var _db = services.GetRequiredService<DataContext>();
            var _authRepository = services.GetRequiredService<IAuthRepository>();

            if (!await _db.Roles.AnyAsync())
                _db.Roles.AddRange(
                    new Role { Name = "admin" },
                    new Role { Name = "employee" }
                );

            await _db.SaveChangesAsync();

            var resultCreateUser1 = await _authRepository.Register(new User { Username = "demouser" }, "Password");

            if(resultCreateUser1.Success)
            {
                await _authRepository.AddRoleToUser(resultCreateUser1.Data, "admin");
            }


        }
    }
}
