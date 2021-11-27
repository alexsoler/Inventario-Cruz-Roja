using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Models;

namespace InventarioCruzRoja.Interfaces
{
    public interface IReportesService
    {
        Task<ServiceResponse<byte[]>> ObtenerReporteIngreso(int id, ReportQuery reportQuery);
    }
}
