using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAccess.Api.Helper;
using SmartAccess.Api.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartAccess.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        
        private readonly SmartAccessContext _context;
        private string _resultado;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var usuario = _context.Users.Where(u => u.Id == id).ToList();
            if (usuario == null)
            {
                return BadRequest("No se encontraron datos para este usuario");
            }
            return Ok(usuario);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User user)
        {

            if (user == null)
            {
                return BadRequest("Debe enviar un usuario y contraseña.");
            }

            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Usuario o contraseña incorrecto");
            }

            var usuario = Authenticate(user.UserName, user.Password);
            if (usuario == null)
            {
                return BadRequest(new { message = "El usuario y/o password son incorrectos" });
            }

            return Ok(usuario);
        }


        private User Authenticate(string login, string password)
        {

            User usuario = _context.Users.FirstOrDefault(o => o.UserName == login && o.Password == password);
            if (usuario == null)
            {
                return null;
            }

            // remove password before returning
            usuario.Password = null;

            return usuario;
        }

        [HttpPost("AddUser")]
        public IActionResult Post([FromBody] User usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = ValidarModelo(usuario);
                if (!resultado)
                {
                    return BadRequest(_resultado);
                }



                var vm = new User()
                {
                    Name = usuario.Name,
                    UserName = usuario.UserName,
                    LastName = usuario.LastName,
                    DNI = usuario.DNI,
                    BirthDate = usuario.BirthDate,
                    Password = usuario.Password
                };

                _context.Users.Update(vm);
                _context.SaveChanges();


                //    var currentUser = _context.Users.FirstOrDefault(o => o.Nombre1 == usuario.UserName && o.Contrasena == usuario.Password);
                //    var vehiculoObj = new Vehiculo()
                //    {
                //        Matricula = usuario.Matricula,
                //        Idusuario1 = currentUser.Idusuario
                //    };

                //    _context.Vehiculo.Update(vehiculoObj);
                //    _context.SaveChanges();

                //    var licenciaObj = new Licencia()
                //    {
                //        Tipo = usuario.Tipo,
                //        Vence = usuario.Vence,
                //        Idusuario = currentUser.Idusuario

                //    };

                //    _context.Licencia.Update(licenciaObj);
                //    _context.SaveChanges();
            }
            catch (Exception e)
            {
                return StatusCode(500, "No se pudo completar la operación.");
            }

            return NoContent();
        }

        private bool ValidarModelo(User vm)
        {
            var valid = true;

            if (string.IsNullOrWhiteSpace(vm.Name))
            {
                _resultado = "Debe especificar el nombre.";
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(vm.LastName))
            {
                _resultado = "Debe especificar el apellido.";
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(vm.UserName))
            {
                _resultado = "Debe especificar el Usuario.";
                valid = false;
            }

            //var obj1 = _context.SolicitudUsuario.FirstOrDefault(o => o.SolicitudUsuarioId != vm.SolicitudUsuarioId &&
            //o.NombreEstablecimiento == vm.NombreEstablecimiento &&
            //o.Ciudad == vm.Ciudad);
            //if (obj1 != null)
            //{
            //    resultado.Agregar("El nombre del establecimiento existe para la misma ciudad.");
            //}

            return valid;
        }

        [HttpPost("olvidePass")]
        public IActionResult OlvidePass([FromBody] User usuario)
        {
            var user = _context.Users.FirstOrDefault(o => o.UserName == usuario.UserName);
            if (user != null)
            {
                user.Password = usuario.Password;
            }
            else
            {
                return BadRequest("Usuario o Contraseña invalidos.");
            }

            _context.Users.Update(user);
            _context.SaveChanges();

            return NoContent();
        }


        [HttpPost("FacialReconection")]
        public async Task<IActionResult> FacialReconection([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.LastName))
            {
                return BadRequest($"El campo Name y el campo LastName son requeridos.");
            }

            var Orequest = new OpenRequest()
            {
                Name = $"{ user.Name } { user.LastName }",
                Message = $"{user.Name } quiere entrar, Deseas abrirle la puerta?",
                Status = "P",
                DateRequest = DateTime.UtcNow.AddHours(-4),
                Picture = user.Picture
                
            };

            _context.OpenRequests.Update(Orequest);
            _context.SaveChanges();

            await NotificationsSend.SendNotificationToAll("Smart Access", $"{user.Name} ha realizado una solicitud de ingreso.");
            return NoContent();
        }


    }
}
