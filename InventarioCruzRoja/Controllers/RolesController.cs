using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventarioCruzRoja.Data;
using InventarioCruzRoja.Models;
using Microsoft.AspNetCore.Authorization;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Dtos;
using AutoMapper;

namespace InventarioCruzRoja.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesRepository _repository;
        private readonly IMapper _mapper;

        public RolesController(IRolesRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<RoleDto>>>> GetRoles()
        {
            return await _repository.GetAll();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<RoleDto>>> GetRole(int id)
        {
            var response = await _repository.Get(id);

            if (response.Data == null)
            {
                return NotFound();
            }

            return response;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<int>>> PutRole(int id, RoleDto role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }

            var response = await _repository.Update(_mapper.Map<Role>(role));

            return response;
        }

        // POST: api/Roles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<RoleDto>>> PostRole(RoleDto role)
        {
            var response = await _repository.Add(_mapper.Map<Role>(role));

            return CreatedAtAction("GetRole", new { id = role.Id }, response);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<RoleDto>>> DeleteRole(int id)
        {
            var response = await _repository.Delete(id);

            if (response.Data == null)
            {
                return NotFound();
            }


            return response;
        }
    }
}
