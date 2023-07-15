using API_Eventos.DataContext;
using API_Eventos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Eventos.Controllers
{
    [ApiController]
    [Route("v1")]
    [Authorize(Roles = "Admin")]
    public class RegistrationsController : ControllerBase
    {
        private readonly ApiDataContext _context;

        public RegistrationsController(ApiDataContext context)
        {
            _context = context;
        }

        [HttpGet("registrations")]
        public async Task<ActionResult<IEnumerable<Registrations>>> GetAllRegistrations()
        {
            var registrations = await _context.Registrations.ToListAsync();
            return Ok(registrations);
        }

        [HttpPost("registrations")]
        public async Task<ActionResult<Registrations>> CreateRegistration([FromBody] Registrations registration)
        {
            if (ModelState.IsValid)
            {
                _context.Registrations.Add(registration);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetRegistrationById", new { id = registration.Id }, registration);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("registrations/{id}")]
        public async Task<ActionResult<Registrations>> GetRegistrationById(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);

            if (registration == null)
            {
                return NotFound();
            }

            return Ok(registration);
        }

        [HttpPut("registrations/{id}")]
        public async Task<IActionResult> UpdateRegistrationById(int id, [FromBody] Registrations updatedRegistration)
        {
            if (id != updatedRegistration.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedRegistration).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistrationExists(id))
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

        [HttpDelete("registrations/{id}")]
        public async Task<IActionResult> DeleteRegistrationById(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);

            if (registration == null)
            {
                return NotFound();
            }

            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(r => r.Id == id);
        }
    }
}
