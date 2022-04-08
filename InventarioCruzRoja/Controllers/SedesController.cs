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
    public class SedesController : ControllerBase
    {
        private readonly ISedesRepository _repository;
        private readonly IMapper _mapper;

        public SedesController(ISedesRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Sedes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SedeDto>>> GetSedes()
        {
            var response = await _repository.GetAll();
            return Ok(_mapper.Map<IEnumerable<SedeDto>>(response.Data));
        }

        // GET: api/Sedes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SedeDto>> GetSede(int id)
        {
            var response = await _repository.Get(id);

            if (response.Data == null)
                return NotFound(response.Message);


            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<SedeDto>(response.Data);
        }

        // PUT: api/Sedes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> PutSede(int id, SedeDto sede)
        {
            if (id != sede.Id)
            {
                return BadRequest();
            }

            var response = await _repository.Update(_mapper.Map<Sede>(sede));

            if (!response.Success)
                return Conflict(response.Message);

            return response.Data.Id;
        }

        // POST: api/Sedes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SedeDto>> PostSede(SedeDto sede)
        {
            var response = await _repository.Add(_mapper.Map<Sede>(sede));

            if (!response.Success)
                return Conflict(response.Message);

            return CreatedAtAction("GetSede", new { id = sede.Id }, response.Data);
        }

        // DELETE: api/Sedes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SedeDto>> DeleteSede(int id)
        {
            var response = await _repository.Delete(id);

            if (response.Data == null)
            {
                return NotFound();
            }

            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<SedeDto>(response.Data);
        }
    }
}