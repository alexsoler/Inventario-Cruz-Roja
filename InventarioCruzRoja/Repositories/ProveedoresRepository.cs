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
    public class ProveedoresRepository : BaseRepository<Proveedor, int>, IProveedoresRepository
    {
        public ProveedoresRepository(DataContext context,
            ILogger<ProveedoresRepository> logger) : base(context, logger)
        {

        }
    }
}
