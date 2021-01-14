using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventarioCruzRoja.Data;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventarioCruzRoja.Repositories
{
    public class RolesRepository : BaseRepository<Role, int>, IRolesRepository
    {
        public RolesRepository(DataContext context,
            ILogger<RolesRepository> logger)
            : base(context, logger)
        {           
        }     
    }
}