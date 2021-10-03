using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Interfaces
{
    public interface IReportesService
    {
        Task<ServiceResponse<byte[]>> ObtenerReporteIngreso(int id, ReportQuery reportQuery);
    }
}
