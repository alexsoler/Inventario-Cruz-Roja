using AutoMapper;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioCruzRoja.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductosRepository _repository;
        private readonly IEventosProductosRepository _eventosProductosRepository;
        private readonly IMapper _mapper;

        public ProductosController(IProductosRepository repository,
            IEventosProductosRepository eventosProductosRepository,
            IMapper mapper)
        {
            _repository = repository;
            _eventosProductosRepository = eventosProductosRepository;
            _mapper = mapper;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos([FromQuery] string filter)
        {
            var response = new ServiceResponse<IEnumerable<Producto>>();

            if (string.IsNullOrEmpty(filter))
            {
                response = await _repository.GetAll("Estado", "Fabricante", "Categoria");
            }
            else
            {
                response = await _repository.GetSearch(filter);
            }

            return Ok(_mapper.Map<IEnumerable<ProductoDto>>(response.Data));
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            var response = await _repository.Get(id, "Sedes");

            if (response.Data == null)
                return NotFound(response.Message);


            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<ProductoDto>(response.Data);
        }

        // PUT: api/Productos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> PutProducto(int id, [FromForm] ProductoDto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    await _repository.GuardarImagen(file);
                }
            }

            producto.UsuarioModifica = User.Identity.Name;
            producto.FechaModificacion = DateTime.Now;

            var response = await _repository.Update(_mapper.Map<Producto>(producto));

            if (!response.Success)
                return Conflict(response.Message);

            await _eventosProductosRepository.EventoEditarProducto(response.Data.Id, User.Identity.Name);

            return response.Data.Id;
        }

        // POST: api/Productos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> PostProducto([FromForm] ProductoDto producto)
        {
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];

                await _repository.GuardarImagen(file);
            }
            producto.UsuarioModifica = User.Identity.Name;

            var response = await _repository.Add(_mapper.Map<Producto>(producto));

            if (!response.Success)
                return Conflict(response.Message);

            await _eventosProductosRepository.EventoAgregarProducto(response.Data.Id, User.Identity.Name);

            return CreatedAtAction("GetProducto", new { id = producto.Id }, response.Data);
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductoDto>> DeleteProducto(int id)
        {
            var response = await _repository.Delete(id);

            if (response.Data == null)
            {
                return NotFound();
            }

            if (!response.Success)
                return Conflict(response.Message);

            return _mapper.Map<ProductoDto>(response.Data);
        }
    }
}