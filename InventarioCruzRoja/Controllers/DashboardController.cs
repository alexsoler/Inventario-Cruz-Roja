
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
    }
}
