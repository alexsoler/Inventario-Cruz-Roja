using AutoMapper;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventarioCruzRoja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class EventosController : ControllerBase
    {
        private readonly IEventosProductosRepository _eventosProductosRepository;
        private readonly IMapper _mapper;

        public EventosController(IEventosProductosRepository eventosProductosRepository,
            IMapper mapper)
        {
            _eventosProductosRepository = eventosProductosRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Producto/{productoId}")]
        public async Task<IActionResult> Get(int productoId)
        {
            var response = await _eventosProductosRepository.ObtenerEventosPorProducto(productoId);

            if (response.Data == null)
                return NotFound(response.Message);


            if (!response.Success)
                return Conflict(response.Message);

            return Ok(_mapper.Map<IEnumerable<EventoDto>>(response.Data));
        }
    }
}
