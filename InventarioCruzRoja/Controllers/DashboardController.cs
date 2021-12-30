
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
    }
}
