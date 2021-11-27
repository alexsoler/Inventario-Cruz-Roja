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
    public class FabricantesController : ControllerBase
    {
        private readonly IFabricantesRepository _repository;
        private readonly IMapper _mapper;

        public FabricantesController(IFabricantesRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Fabricantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FabricanteDto>>> GetFabricantes()
        {
            var response = await _repository.GetAll();
            return Ok(_mapper.Map<IEnumerable<FabricanteDto>>(response.Data));
        }

        // GET: api/Fabricantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FabricanteDto>> GetFabricante(int id)
        {
            var response = await _repository.Get(id);

            if (response.Data == null)
                return NotFound(response.Message);


            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<FabricanteDto>(response.Data);
        }

        // PUT: api/Fabricantes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> PutFabricante(int id, FabricanteDto fabricante)
        {
            if (id != fabricante.Id)
            {
                return BadRequest();
            }

            var response = await _repository.Update(_mapper.Map<Fabricante>(fabricante));

            if (!response.Success)
                return Conflict(response.Message);

            return response.Data.Id;
        }

        // POST: api/Fabricantes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FabricanteDto>> PostFabricante(FabricanteDto fabricante)
        {
            var response = await _repository.Add(_mapper.Map<Fabricante>(fabricante));

            if (!response.Success)
                return Conflict(response.Message);

            return CreatedAtAction("GetFabricante", new { id = fabricante.Id }, response.Data);
        }

        // DELETE: api/Fabricantes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FabricanteDto>> DeleteFabricante(int id)
        {
            var response = await _repository.Delete(id);

            if (response.Data == null)
            {
                return NotFound();
            }

            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<FabricanteDto>(response.Data);
        }
    }
}