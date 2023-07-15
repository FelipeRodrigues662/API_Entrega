using API_Eventos.DataContext;
using API_Eventos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Eventos.Controllers
{
    [ApiController]
    [Route("v1")]
    [Authorize(Roles = "Admin")]
    public class EventsControllers : ControllerBase
    {
        private readonly ApiDataContext _context;

        public EventsControllers(ApiDataContext context)
        {
            _context = context;
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetAsync()
        {
            var events = await _context.Events.Include(u => u.Categories).ToListAsync();
            return Ok(events);
        }

        [HttpPost("events")]
        public async Task<IActionResult> PostAsync([FromBody] Events events)
        {
            await _context.Events.AddAsync(events);
            await _context.SaveChangesAsync();

            return Created($"/v1/events/{events.Id}", events);
        }

        [HttpGet("events/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var events = await _context.Events.Include(u => u.Categories).FirstOrDefaultAsync(e => e.Id == id);

            if (events == null)
            {
                return NotFound();
            }

            return Ok(events);
        }

        [HttpPut("events/{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] Events updatedEvents)
        {
            if (id != updatedEvents.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedEvents).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventsExists(id))
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

        [HttpDelete("events/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var events = await _context.Events.FindAsync(id);

            if (events == null)
            {
                return NotFound();
            }

            _context.Events.Remove(events);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventsExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
