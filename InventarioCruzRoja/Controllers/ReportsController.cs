using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventarioCruzRoja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportesService _reportesService;

        public ReportsController(IReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        [HttpGet("ingresos/{id}")]
        public async Task<IActionResult> GetIngreso(int id, [FromQuery] ReportQuery query)
        {
            string mime = "application/" + query.Format;

            var response = await _reportesService.ObtenerReporteIngreso(id, query);

            if (!response.Success)
                return Conflict(response.Message);

            return File(response.Data, mime);
        }

        [HttpGet("egresos/{id}")]
        public async Task<IActionResult> GetEgreso(int id, [FromQuery] ReportQuery query)
        {
            string mime = "application/" + query.Format;

            var response = await _reportesService.ObtenerReporteEgreso(id, query);

            if (!response.Success)
                return Conflict(response.Message);

            return File(response.Data, mime);
        }

        [HttpGet("traslados/{id}")]
        public async Task<IActionResult> GetTraslado(int id, [FromQuery] ReportQuery query)
        {
            string mime = "application/" + query.Format;

            var response = await _reportesService.ObtenerReporteTraslado(id, query);

            if (!response.Success)
                return Conflict(response.Message);

            return File(response.Data, mime);
        }

        [HttpGet("inventario")]
        public async Task<IActionResult> GetTraslado([FromQuery] int? sedeId, [FromQuery] DateTime? fechaDesde, [FromQuery] DateTime? fechaHasta, [FromQuery] ReportQuery query)
        {
            string mime = "application/" + query.Format;

            var response = await _reportesService.ObtenerReporteInventario(sedeId, fechaDesde, fechaHasta, query);

            if (!response.Success)
                return Conflict(response.Message);

            return File(response.Data, mime);
        }
    }
}
