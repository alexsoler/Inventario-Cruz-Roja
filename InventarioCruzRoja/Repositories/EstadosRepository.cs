using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Repositories
{
    public class EstadosRepository : BaseRepository<Estado, int>, IEstadosRepository
    {
        public EstadosRepository(DataContext context,
            ILogger<EstadosRepository> logger)
            : base(context, logger)
        {

        }
    }
}
