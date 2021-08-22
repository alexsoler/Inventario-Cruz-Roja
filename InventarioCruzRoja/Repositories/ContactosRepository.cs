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
    public class ContactosRepository : BaseRepository<Contacto, int>, IContactosRepository
    {
        public ContactosRepository(DataContext context,
            ILogger<ContactosRepository> logger) : base(context, logger)
        {

        }
    }
}
