using AutoMapper;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioCruzRoja.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class ContactosController : ControllerBase
    {
        private readonly IContactosRepository _repository;
        private readonly IMapper _mapper;

        public ContactosController(IContactosRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Contactos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactoDto>>> GetContactos()
        {
            var response = await _repository.GetAll();
            return Ok(_mapper.Map<IEnumerable<ContactoDto>>(response.Data));
        }

        // GET: api/Contactos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactoDto>> GetContacto(int id)
        {
            var response = await _repository.Get(id);

            if (response.Data == null)
                return NotFound(response.Message);


            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<ContactoDto>(response.Data);
        }

        // PUT: api/Contactos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> PutContacto(int id, ContactoDto contacto)
        {
            if (id != contacto.Id)
            {
                return BadRequest();
            }

            var response = await _repository.Update(_mapper.Map<Contacto>(contacto));

            if (!response.Success)
                return Conflict(response.Message);

            return response.Data.Id;
        }

        // POST: api/Contactos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ContactoDto>> PostContacto(ContactoDto contacto)
        {
            var response = await _repository.Add(_mapper.Map<Contacto>(contacto));

            if (!response.Success)
                return Conflict(response.Message);

            return CreatedAtAction("GetContacto", new { id = contacto.Id }, response.Data);
        }

        // DELETE: api/Contactos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactoDto>> DeleteContacto(int id)
        {
            var response = await _repository.Delete(id);

            if (response.Data == null)
            {
                return NotFound();
            }

            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<ContactoDto>(response.Data);
        }
    }
}
