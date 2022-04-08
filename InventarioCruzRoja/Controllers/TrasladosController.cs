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
    public class TrasladosController : ControllerBase
    {
        private readonly ITrasladosRepository _repository;
        private readonly IEventosProductosRepository _eventosRepository;
        private readonly IMapper _mapper;

        public TrasladosController(ITrasladosRepository repository,
            IEventosProductosRepository eventosRepository,
            IMapper mapper)
        {
            _repository = repository;
            _eventosRepository = eventosRepository;
            _mapper = mapper;
        }

        // GET: api/Traslados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrasladoDto>>> GetTraslados()
        {
            var response = await _repository.GetAll("IngresoDestino", "EgresoOrigen", "Producto", "User", "UserAnula");
            return Ok(_mapper.Map<IEnumerable<TrasladoDto>>(response.Data));
        }

        // GET: api/Traslados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrasladoDto>> GetTraslado(int id)
        {
            var response = await _repository.Get(id, "IngresoDestino", "EgresoOrigen", "Producto", "User", "UserAnula");

            if (response.Data == null)
                return NotFound(response.Message);


            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<TrasladoDto>(response.Data);
        }

        // PUT: api/Traslados/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> PutTraslado(int id, TrasladoDto traslado)
        {
            if (id != traslado.Id)
            {
                return BadRequest();
            }

            var response = await _repository.Update(_mapper.Map<Traslado>(traslado));

            if (!response.Success)
                return Conflict(response.Message);

            if (response.Data.Anulado)
                await _eventosRepository.EventoAnulacionTraslado(response.Data.ProductoId, User.Identity.Name);

            return response.Data.Id;
        }

        // POST: api/Traslados
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TrasladoDto>> PostTraslado(TrasladoDto traslado)
        {
            var response = await _repository.AddFromDto(traslado);

            if (!response.Success)
                return Conflict(response.Message);

            await _eventosRepository.EventoTrasladoProducto(response.Data.ProductoId, User.Identity.Name);

            return CreatedAtAction("GetTraslado", new { id = traslado.Id }, _mapper.Map<TrasladoDto>(response.Data));
        }

        // DELETE: api/Traslados/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TrasladoDto>> DeleteTraslado(int id)
        {
            var response = await _repository.Delete(id);

            if (response.Data == null)
            {
                return NotFound();
            }

            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<TrasladoDto>(response.Data);
        }
    }
}
