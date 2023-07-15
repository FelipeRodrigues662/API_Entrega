using API_Eventos.DataContext;
using API_Eventos.Models;
using API_Eventos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Eventos.Controllers
{
    [Route("v1")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApiDataContext _context;
        private readonly UserService _userService;

        public UsersController(ApiDataContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // POST: api/Users
        [HttpPost("users")]
        public async Task<ActionResult<Users>> CreateUser([FromBody] Users user)
        {
            if (ModelState.IsValid)
            {
                // Hash da senha
                user.Password = _userService.HashPassword(user.Password);

                // Salvar o usuário no banco de dados
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUserById", new { id = user.Id }, user);
            }

            return BadRequest(ModelState);
        }

        // GET: api/Users/1
        [HttpGet("users/{id}")]
        public async Task<ActionResult<Users>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // DELETE: api/Users/1
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Users/1
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUserById(int id, [FromBody] Users updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(u => u.Id == id);
        }
    }
}
