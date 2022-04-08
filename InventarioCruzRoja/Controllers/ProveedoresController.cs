using AutoMapper;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioCruzRoja.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedoresRepository _repository;
        private readonly IMapper _mapper;

        public ProveedoresController(IProveedoresRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorDto>>> GetProveedores([FromQuery] string filter)
        {
            var response = new ServiceResponse<IEnumerable<Proveedor>>();

            if (string.IsNullOrEmpty(filter))
            {
                response = await _repository.GetAll("Contactos");
            }
            else
            {
                response = await _repository.GetSearch(filter);
            }

            return Ok(_mapper.Map<IEnumerable<ProveedorDto>>(response.Data));
        }

        // GET: api/Proveedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorDto>> GetProveedor(int id)
        {
            var response = await _repository.Get(id);

            if (response.Data == null)
                return NotFound(response.Message);


            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<ProveedorDto>(response.Data);
        }

        // PUT: api/Proveedores/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> PutProveedor(int id, ProveedorDto proveedor)
        {
            if (id != proveedor.Id)
            {
                return BadRequest();
            }
            var rol = _mapper.Map<Proveedor>(proveedor);
            var response = await _repository.Update(rol);

            if (!response.Success)
                return Conflict(response.Message);

            return response.Data.Id;
        }

        // POST: api/Proveedores
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProveedorDto>> PostProveedor(ProveedorDto proveedor)
        {
            var response = await _repository.Add(_mapper.Map<Proveedor>(proveedor));

            if (!response.Success)
                return Conflict(response.Message);

            return CreatedAtAction("GetProveedor", new { id = proveedor.Id }, response.Data);
        }

        // DELETE: api/Proveedores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProveedorDto>> DeleteProveedor(int id)
        {
            var response = await _repository.Delete(id);

            if (response.Data == null)
            {
                return NotFound();
            }

            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<ProveedorDto>(response.Data);
        }
    }
}
