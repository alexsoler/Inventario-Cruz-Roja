using AutoMapper;
using FastReport;
using FastReport.Export.Html;
using FastReport.Export.Image;
using FastReport.Export.PdfSimple;
using FastReport.Utils;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

    }
}
