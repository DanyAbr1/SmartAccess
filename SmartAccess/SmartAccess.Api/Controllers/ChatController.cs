using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SmartAccess.Api.Hubs;
using SmartAccess.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartAccess.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private SmartAccessContext _context;
        IHubContext<MessagesHub> _hub;


        public ChatController(SmartAccessContext context, IHubContext<MessagesHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        [HttpGet("GetMessage")]
        public ActionResult<IEnumerable<Chat>> GetAll()
        {
            return _context.Chats.ToList();

        }

        [HttpPost("NewMessage")]
        public async Task<IActionResult> Post([FromBody] Chat chat)
        {

            var NewMessage = new Chat()
            {                
                Name = chat.Name,
                Message = chat.Message,
                DateMessage = DateTime.Now,
            };
                       
            _context.Chats.Update(NewMessage);
            _context.SaveChanges();

            await _hub.Clients.All.SendAsync("NewMessage", JsonConvert.SerializeObject(NewMessage));

            return NoContent();
        }

    }
}
