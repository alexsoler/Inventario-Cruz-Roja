using AutoMapper;
using InventarioCruzRoja.Dtos;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventarioCruzRoja.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesRepository _repository;
        private readonly IMapper _mapper;

        public MessagesController(IMessagesRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages()
        {
            var response = await _repository.GetAll();
            return Ok(_mapper.Map<IEnumerable<MessageDto>>(response.Data));
        }

        // POST: api/Messages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MessageDto>> PostMessage(MessageDto message)
        {
            var response = await _repository.Add(_mapper.Map<Message>(message));

            if (!response.Success)
                return Conflict(response.Message);

            return CreatedAtAction("GetMessage", new { id = message.Id }, response.Data);
        }

    }
}
