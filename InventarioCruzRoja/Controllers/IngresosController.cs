﻿using AutoMapper;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
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
    [Authorize(Roles = "admin")]
    [ApiController]
    public class IngresosController : ControllerBase
    {
        private readonly IIngresosRepository _repository;
        private readonly IMapper _mapper;

        public IngresosController(IIngresosRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Ingresos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngresoDto>>> GetIngresos()
        {
            var response = await _repository.GetAll("Proveedor", "Sede", "Producto", "User");
            return Ok(_mapper.Map<IEnumerable<IngresoDto>>(response.Data));
        }

        // GET: api/Ingresos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngresoDto>> GetIngreso(int id)
        {
            var response = await _repository.Get(id);

            if (response.Data == null)
                return NotFound(response.Message);


            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<IngresoDto>(response.Data);
        }

        // PUT: api/Ingresos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> PutIngreso(int id, IngresoDto ingreso)
        {
            if (id != ingreso.Id)
            {
                return BadRequest();
            }

            var response = await _repository.Update(_mapper.Map<Ingreso>(ingreso));

            if (!response.Success)
                return Conflict(response.Message);

            return response.Data.Id;
        }

        // POST: api/Ingresos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<IngresoDto>> PostIngreso(IngresoDto ingreso)
        {
            var response = await _repository.Add(_mapper.Map<Ingreso>(ingreso));

            if (!response.Success)
                return Conflict(response.Message);

            return CreatedAtAction("GetIngreso", new { id = ingreso.Id }, response.Data);
        }

        // DELETE: api/Ingresos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IngresoDto>> DeleteIngreso(int id)
        {
            var response = await _repository.Delete(id);

            if (response.Data == null)
            {
                return NotFound();
            }

            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<IngresoDto>(response.Data);
        }
    }
}
