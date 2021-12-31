
using InventarioCruzRoja.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventarioCruzRoja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet]
        [Route("ResumenDeProductos")]
        public async Task<IActionResult> ResumenDeProductos()
        {
            var response = await _dashboardRepository.ObtenerResumenDeProductos();

            if (!response.Success)
                return Conflict(response.Message);

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ResumenDeIngresos")]
        public async Task<IActionResult> ResumenDeIngresos()
        {
            var response = await _dashboardRepository.ObtenerResumenDeIngresos();

            if (!response.Success)
                return Conflict(response.Message);

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("ResumenDeEgresos")]
        public async Task<IActionResult> ResumenDeEgresos()
        {
            var response = await _dashboardRepository.ObtenerResumenDeEgresos();

            if (!response.Success)
                return Conflict(response.Message);

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("CantidadDeUsuarios")]
        public async Task<IActionResult> CantidadDeUsuarios()
        {
            var response = await _dashboardRepository.ObtenerCantidadDeUsuarios();

            if (!response.Success)
                return Conflict(response.Message);

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("CantidadDeSedes")]
        public async Task<IActionResult> CantidadDeSedes()
        {
            var response = await _dashboardRepository.ObtenerCantidadDeSedes();

            if (!response.Success)
                return Conflict(response.Message);

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("CantidadDeProductos")]
        public async Task<IActionResult> CantidadDeProductos()
        {
            var response = await _dashboardRepository.ObtenerCantidadDeProductos();

            if (!response.Success)
                return Conflict(response.Message);

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("CantidadDeProveedores")]
        public async Task<IActionResult> CantidadDeProveedores()
        {
            var response = await _dashboardRepository.ObtenerCantidadDeProveedores();

            if (!response.Success)
                return Conflict(response.Message);

            return Ok(response.Data);
        }
    }
}
