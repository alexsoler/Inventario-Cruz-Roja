using AutoMapper;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventarioCruzRoja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private readonly IEstadosRepository _estadosRepository;
        private readonly IMapper _mapper;

        public EstadosController(IEstadosRepository estadosRepository,
            IMapper mapper)
        {
            _estadosRepository = estadosRepository;
            _mapper = mapper;
        }

        // GET: api/Estados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoDto>>> GetEstados()
        {
            var response = await _estadosRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<EstadoDto>>(response.Data));
        }
    }
}
