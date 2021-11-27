using AutoMapper;
using FastReport;
using FastReport.Export.Html;
using FastReport.Export.Image;
using FastReport.Export.PdfSimple;
using FastReport.Utils;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using System.Collections;

namespace InventarioCruzRoja.Services
{
    public class ReportesService : IReportesService
    {
        private readonly IIngresosRepository _ingresosRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;
        public ReportesService(IIngresosRepository ingresoRepository,
            IWebHostEnvironment environment,
            IMapper mapper)
        {
            _ingresosRepository = ingresoRepository;
            _environment = environment;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<byte[]>> ObtenerReporteIngreso(int id, ReportQuery reportQuery)
        {
            var response = new ServiceResponse<byte[]>();
            var ingresoResponse = await _ingresosRepository.Get(id, "Proveedor", "Sede", "Producto", "User");

            if (ingresoResponse.Data == null || !ingresoResponse.Success)
            {
                response.Success = false;
                response.Message = ingresoResponse.Message;
                return response;
            }

            var ingreso = _mapper.Map<IngresoDto>(ingresoResponse.Data);

            try
            {
                response.Data = GenerarReporte(new List<IngresoDto> { ingreso }, "Ingreso", "Ingreso", reportQuery);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

        private byte[] GenerarReporte(IEnumerable datos, string nombreDatos, string nombreReporte, ReportQuery query)
        {
            string mime = "application/" + query.Format;

            var reportPath = Path.Combine(_environment.ContentRootPath, "Resources", "Reports", $"{nombreReporte}.frx");
            using MemoryStream stream = new();
            Config.WebMode = true;
            using (Report report = new())
            {
                report.Load(reportPath);
                report.RegisterData(datos, nombreDatos);
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

            return stream.ToArray();
        }
    }
}
