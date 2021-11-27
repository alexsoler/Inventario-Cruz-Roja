using AutoMapper;
using InventarioCruzRoja.Data;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            response.Message = "El usuario fue registro con exito.";
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

            var roles = await _context.Roles.Where(x => rolesToAsign.Contains(x.Name))
                .ToListAsync();
            var user = await _context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == idUser);

            if (user == null)
            {
                response.Success = false;
                response.Message = $"- El usuario de Id '{idUser}' no existe.\n";
                response.Data = false;

                return response;
            }

            bool isRoleAssigned = false;

            foreach (var role in roles)
            {
                if (user.UserRoles.Exists(x => x.Role == role))
                {
                    response.Message += $"- El usuario ya tiene asignado el rol '{role.Name}'.\n";
                }
                else
                {
                    user.UserRoles.Add(new UserRole
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    });

                    response.Message += $"- Se agrego el rol '{role.Name}'.\n";

                    isRoleAssigned = true;
                }
            }

            if (isRoleAssigned)
            {
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

        public async Task EditUser(int id, UserEditDto user)
        {
            var userToUpdate = await _context.Users.FindAsync(id);

            await _context.Entry(userToUpdate).Collection(x => x.UserRoles)
                .Query().Include(x => x.Role).LoadAsync();

            var roles = await _context.Roles.Where(x => user.Roles.Contains(x.Name))
                .ToListAsync();

            //Remover roles
            userToUpdate.UserRoles.RemoveAll(x => !roles.Contains(x.Role));

            //Agregar roles
            foreach (var role in roles)
            {
                if (!userToUpdate.UserRoles.Any(x => x.Role == role))
                {
                    userToUpdate.UserRoles.Add(new UserRole
                    {
                        RoleId = role.Id,
                        UserId = id
                    });
                }

            }

            if (!string.IsNullOrEmpty(user.Password))
            {
                CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

                userToUpdate.PasswordHash = passwordHash;
                userToUpdate.PasswordSalt = passwordSalt;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ServiceResponse<int>> DeleteUser(int id)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                response.Success = false;
                response.Message = $"El usuario de id '{id}' ya ha sido eliminado o no existe";

                return response;
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            response.Data = user.Id;
            response.Message = $"El usuario '{user.Username}' fue eliminado con exito.";

            return response;
        }

        public Task RemoveRole(int id)
        {
            throw new NotImplementedException();
        }
    }
}
