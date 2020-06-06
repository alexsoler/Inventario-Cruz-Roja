using AutoMapper;
using InventarioCruzRoja.Data;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthRepository(DataContext context,
            IConfiguration configuration,
            IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<LoginResponseDto>> Login(string username, string password)
        {
            ServiceResponse<LoginResponseDto> response = new ServiceResponse<LoginResponseDto>();
            User user = await _context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "Usuario no encontrado";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Contraseña incorrecta";
            }
            else
            {
                var loginResponse = _mapper.Map<LoginResponseDto>(user);
                loginResponse.AccessToken = CreateToken(user);
                response.Data = loginResponse;
            }

            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();

            if (await UserExists(user.Username))
            {
                response.Success = false;
                response.Message = "El usuario ya existe";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            user.UserRoles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
            });

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<ServiceResponse<bool>> AddRoleToUser(int idUser, params string[] rolesToAsign)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();

            var roles = await _context.Roles.AsNoTracking().ToListAsync();
            var user = await _context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == idUser);

            if (user == null)
            {
                response.Success = false;
                response.Message = $"El usuario de Id '{idUser}' no existe.-";
                response.Data = false;

                return response;
            }

            bool isRoleAssigned = false;

            foreach (var role in rolesToAsign)
            {
                var roleToAsign = roles.FirstOrDefault(x => x.Name.ToLower() == role.ToLower());

                if (roleToAsign == null)
                {
                    response.Message += $"El rol '{role}' no existe.-";
                }
                else
                {
                    if (user.UserRoles.Exists(x => x.Role.Name.ToLower() == role.ToLower()))
                    {
                        response.Message += $"El usuario ya tiene asignado el rol '{role}'.-";
                    }
                    else
                    {
                        user.UserRoles.Add(new UserRole
                        {
                            RoleId = roleToAsign.Id,
                            UserId = user.Id
                        });

                        response.Message += $"Se agrego el rol '{role}' con exito.-";

                        isRoleAssigned = true;
                    }
                }
            }

            if (isRoleAssigned) {
                await _context.SaveChangesAsync();
                response.Success = true;
            }

            return response;
        }

        public async Task<ServiceResponse<IEnumerable<UserTableDto>>> GetUsers()
        {
            var users = await _context.Users.Include(x => x.UserRoles)
                .ThenInclude(x => x.Role).AsNoTracking().ToListAsync();

            var usersDto = _mapper.Map<IEnumerable<UserTableDto>>(users);

            return new ServiceResponse<IEnumerable<UserTableDto>>
            {
                Data = usersDto,
                Success = true
            };
        }

        public async Task<ServiceResponse<IEnumerable<string>>> GetRoles()
        {
            var roles = await _context.Roles.AsNoTracking()
                .Select(x => x.Name).ToListAsync();

            return new ServiceResponse<IEnumerable<string>>
            {
                Data = roles,
                Success = true
            };
        }

        public Task EditUser(int id, UserDto user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRole(int id)
        {
            throw new NotImplementedException();
        }
    }
}
