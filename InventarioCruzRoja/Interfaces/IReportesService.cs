using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Interfaces
{
    public interface IReportesService
    {
        Task<ServiceResponse<byte[]>> ObtenerReporteIngreso(int id, ReportQuery reportQuery);
        Task<ServiceResponse<byte[]>> ObtenerReporteEgreso(int id, ReportQuery reportQuery);
        Task<ServiceResponse<byte[]>> ObtenerReporteTraslado(int id, ReportQuery reportQuery);
        Task<ServiceResponse<byte[]>> ObtenerReporteInventario(int? sedeId, DateTime? fechaDesde, DateTime? fechaHasta, ReportQuery reportQuery);
    }
}
