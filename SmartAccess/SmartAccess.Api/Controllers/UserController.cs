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
    public class UserController : Controller
    {
        private readonly SmartAccessContext _context;

        public UserController(SmartAccessContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<User>> Get(string nombre, string pass)
        {
            var usuario = await _context.Users.FirstOrDefaultAsync(x => x.UserName == nombre && x.Password == pass);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }
    }
}
