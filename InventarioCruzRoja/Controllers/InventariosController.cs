using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioCruzRoja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class InventariosController : ControllerBase
    {
        private readonly IInventariosRepository _inventariosRepository;

        public InventariosController(IInventariosRepository inventariosRepository)
        {
            _inventariosRepository = inventariosRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventarioDto>>> Get([FromQuery] int? sedeId, [FromQuery] DateTime? fechaDesde, [FromQuery] DateTime? fechaHasta)
        {
            var response = await _inventariosRepository.GetInventario(sedeId, fechaDesde, fechaHasta);

            if (!response.Success)
                return Conflict(response.Message);

            return Ok(response.Data);
        }
    }
}
