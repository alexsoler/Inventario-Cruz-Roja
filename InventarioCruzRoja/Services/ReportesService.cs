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
        private readonly IEgresosRepository _egresosRepository;
        private readonly ITrasladosRepository _trasladosRepository;
        private readonly IInventariosRepository _inventariosRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;
        public ReportesService(IIngresosRepository ingresosRepository,
            IEgresosRepository egresosRepository,
            ITrasladosRepository trasladosRepository,
            IInventariosRepository inventariosRepository,
            IWebHostEnvironment environment,
            IMapper mapper)
        {
            _ingresosRepository = ingresosRepository;
            _egresosRepository = egresosRepository;
            _trasladosRepository = trasladosRepository;
            _inventariosRepository = inventariosRepository;
            _environment = environment;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<byte[]>> ObtenerReporteIngreso(int id, ReportQuery reportQuery)
        {
            var response = new ServiceResponse<byte[]>();
            var ingresoResponse = await _ingresosRepository.Get(id, "Proveedor", "Sede", "Producto", "User", "UserAnula");

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

        public async Task<ServiceResponse<byte[]>> ObtenerReporteEgreso(int id, ReportQuery reportQuery)
        {
            var response = new ServiceResponse<byte[]>();
            var egresoResponse = await _egresosRepository.Get(id, "Sede", "Producto", "User", "UserAnula");

            if (egresoResponse.Data == null || !egresoResponse.Success)
            {
                response.Success = false;
                response.Message = egresoResponse.Message;
                return response;
            }

            var egreso = _mapper.Map<EgresoDto>(egresoResponse.Data);

            try
            {
                response.Data = GenerarReporte(new List<EgresoDto> { egreso }, "Egreso", "Egreso", reportQuery);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

        public async Task<ServiceResponse<byte[]>> ObtenerReporteTraslado(int id, ReportQuery reportQuery)
        {
            var response = new ServiceResponse<byte[]>();
            var trasladoResponse = await _trasladosRepository.Get(id, "IngresoDestino", "EgresoOrigen", "Producto", "User", "UserAnula");

            if (trasladoResponse.Data == null || !trasladoResponse.Success)
            {
                response.Success = false;
                response.Message = trasladoResponse.Message;
                return response;
            }

            var traslado = _mapper.Map<TrasladoDto>(trasladoResponse.Data);

            try
            {
                response.Data = GenerarReporte(new List<TrasladoDto> { traslado }, "Traslado", "Traslado", reportQuery);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

        public async Task<ServiceResponse<byte[]>> ObtenerReporteInventario(int? sedeId, DateTime? fechaDesde, DateTime? fechaHasta, ReportQuery reportQuery)
        {
            var response = new ServiceResponse<byte[]>();
            var trasladoResponse = await _inventariosRepository.GetInventario(sedeId, fechaDesde, fechaHasta);

            if (trasladoResponse.Data == null || !trasladoResponse.Success)
            {
                response.Success = false;
                response.Message = trasladoResponse.Message;
                return response;
            }

            try
            {
                response.Data = GenerarReporte(trasladoResponse.Data, "Inventario", "Inventario", reportQuery);
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
