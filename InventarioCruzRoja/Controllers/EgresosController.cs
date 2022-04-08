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
    public class EgresosController : ControllerBase
    {
        private readonly IEgresosRepository _repository;
        private readonly IEventosProductosRepository _eventosProductosRepository;
        private readonly IMapper _mapper;

        public EgresosController(IEgresosRepository repository,
            IEventosProductosRepository eventosProductosRepository,
            IMapper mapper)
        {
            _repository = repository;
            _eventosProductosRepository = eventosProductosRepository;
            _mapper = mapper;
        }

        // GET: api/Egresos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EgresoDto>>> GetEgresos()
        {
            var response = await _repository.GetAll("Sede", "Producto", "User", "UserAnula");
            return Ok(_mapper.Map<IEnumerable<EgresoDto>>(response.Data));
        }

        // GET: api/Egresos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EgresoDto>> GetEgreso(int id)
        {
            var response = await _repository.Get(id, "Sede", "Producto", "User");

            if (response.Data == null)
                return NotFound(response.Message);


            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<EgresoDto>(response.Data);
        }

        // PUT: api/Egresos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> PutEgreso(int id, EgresoDto egreso)
        {
            if (id != egreso.Id)
            {
                return BadRequest();
            }

            var response = await _repository.Update(_mapper.Map<Egreso>(egreso));

            if (!response.Success)
                return Conflict(response.Message);

            if (response.Data.Anulado)
                await _eventosProductosRepository.EventoAnulacionEgreso(response.Data.ProductoId, response.Data.Cantidad, User.Identity.Name);

            return response.Data.Id;
        }

        // POST: api/Egresos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EgresoDto>> PostEgreso(EgresoDto egreso)
        {
            var response = await _repository.Add(_mapper.Map<Egreso>(egreso));

            if (!response.Success)
                return Conflict(response.Message);

            await _eventosProductosRepository.EventoEgresoDeProducto(response.Data.ProductoId, response.Data.Cantidad, User.Identity.Name);

            return CreatedAtAction("GetEgreso", new { id = egreso.Id }, response.Data);
        }

        // DELETE: api/Egresos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EgresoDto>> DeleteEgreso(int id)
        {
            var response = await _repository.Delete(id);

            if (response.Data == null)
            {
                return NotFound();
            }

            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<EgresoDto>(response.Data);
        }
    }
}
