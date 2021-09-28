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
        private readonly IIngresosRepository _repository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;


        public ReportsController(IIngresosRepository repository,
            IWebHostEnvironment environment,
            IMapper mapper)
        {
            _repository = repository;
            _environment = environment;
            _mapper = mapper;
        }

        [HttpGet("ingresos/{id}")]
        public async Task<IActionResult> GetIngreso(int id, [FromQuery] ReportQuery query)
        {
            string mime = "application/" + query.Format;

            var response = await _repository.Get(id, "Proveedor", "Sede", "Producto", "User");

            if (response.Data == null)
                return NotFound(response.Message);


            if (!response.Success)
                return Conflict(response.Message);

            var ingreso = _mapper.Map<IngresoDto>(response.Data);

            var reportPath = Path.Combine(_environment.ContentRootPath, "Resources", "Reports", "Ingreso.frx");
            using MemoryStream stream = new();
            try
            {
                Config.WebMode = true;
                using (Report report = new())
                {
                    report.Load(reportPath);
                    report.RegisterData(new List<IngresoDto> { ingreso }, "Ingreso");
                    report.Prepare();

                    if (query.Format == "png")
                    {
                        ImageExport png = new();
                        png.ImageFormat = ImageExportFormat.Png;
                        png.SeparateFiles = false;
                        report.Export(png, stream);
                    }
                    else if (query.Format == "html")
                    {
                        HTMLExport html = new();
                        html.SinglePage = true;
                        html.Navigator = false;
                        html.EmbedPictures = true;
                        report.Export(html, stream);
                        mime = "text/" + query.Format;
                    }
                    else if (query.Format == "pdf")
                    {
                        PDFSimpleExport pdfExport = new();
                        report.Export(pdfExport, stream);
                        mime = "application/" + query.Format;
                    }
                }

                var file = string.Concat(Path.GetFileNameWithoutExtension(reportPath), ".", query.Format);

                if (query.Inline)
                    return File(stream.ToArray(), mime);
                else
                    return File(stream.ToArray(), mime, file); // attachment
            }
            catch (Exception ex)
            {
                return new NoContentResult();
            }
        }

    }
}
