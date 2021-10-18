using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioCruzRoja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class InventariosController : ControllerBase
    {
        private readonly IInventariosRepository _inventariosRepository;

        public InventariosController(IInventariosRepository inventariosRepository)
        {
            _inventariosRepository = inventariosRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventarioDto>>> Get([FromQuery]int? sedeId)
        {
            var response = await _inventariosRepository.GetInventario(sedeId);

            if (!response.Success)
                return Conflict(response.Message);

            return Ok(response.Data);
        }
    }
}
