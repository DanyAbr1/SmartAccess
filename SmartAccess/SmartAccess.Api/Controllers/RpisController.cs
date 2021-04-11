using Microsoft.AspNetCore.Mvc;
using SmartAccess.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartAccess.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RpisController : ControllerBase
    {
        private readonly SmartAccessContext _context;

        public RpisController(SmartAccessContext context)
        {
            _context = context;
        }

        [HttpGet("GetRpiStatus")]
        public ActionResult<IEnumerable<RpiStatus>> GetAll()
        {
            return _context.Rpis.ToList();
        }

        [HttpPost("PostRpiStatus")]
        public IActionResult RpiSave([FromBody] RpiStatus status)
        {

            if (status == null)
            {
                return BadRequest("Debe mandar un RpiStatus válido");
            }

            var SaveStatus = new RpiStatus
            {
                TimeUp = status.TimeUp,
                Temperature = status.Temperature,
                Email = status.Email
            };

            _context.Rpis.Update(SaveStatus);
            _context.SaveChanges();

            return NoContent();
        }
    }


}
