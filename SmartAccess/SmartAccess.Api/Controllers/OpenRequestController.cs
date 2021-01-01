using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAccess.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartAccess.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenRequestController : ControllerBase
    {
        private SmartAccessContext _context;        


        public OpenRequestController(SmartAccessContext context)
        {
            _context = context;
            
        }


        [HttpGet("GetOpenRequest")]
        public ActionResult<IEnumerable<OpenRequest>> GetAll()
        {
            return _context.OpenRequests.ToList();
        }

        [HttpPut("{id}")]
        public ActionResult ChangeStatus(int id, [FromBody] OpenRequest value)
        {            
            if (id != value.Id)
            {
                return BadRequest();
            }

            _context.Entry(value).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

    }
}
