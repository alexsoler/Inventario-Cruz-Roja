using AutoMapper;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioCruzRoja.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authRepository;

        public AuthController(IMapper mapper,
            IAuthRepository authRepository)
        {
            _mapper = mapper;
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(request);
            var response = await _authRepository.Register(user, request.Password);

            if (!response.Success)
                return BadRequest(response);
            else if (request.Roles.Any())
            {
                var responseRoles = await _authRepository
                    .AddRoleToUser(response.Data, request.Roles.ToArray());

                response.Message += "\n\n" + responseRoles.Message;
            }

            return Ok(response);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, UserEditDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id)
            {
                return BadRequest();
            }

            try
            {
                await _authRepository.EditUser(id, request);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _authRepository.DeleteUser(id);

            if (!response.Success)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            var response = await _authRepository.Login(
                request.Username, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Users()
        {
            var response = await _authRepository.GetUsers();

            return Ok(response);
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> Roles()
        {
            var response = await _authRepository.GetRoles();

            return Ok(response);
        }
    }
}
