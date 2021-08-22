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
    public class CategoriasRepository : BaseRepository<Categoria, int>, ICategoriasRepository
    {
        public CategoriasRepository(DataContext context,
            ILogger<CategoriasRepository> logger) : base (context, logger)
        {

        }
    }
}
