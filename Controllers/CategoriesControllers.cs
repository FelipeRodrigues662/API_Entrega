using API_Eventos.DataContext;
using API_Eventos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Eventos.Controllers
{
    [ApiController]
    [Route("v1")]
    public class CategoriesControllers : ControllerBase
    {
        [HttpGet("events")]
        public async Task<IActionResult> GetAsync([FromServices] ApiDataContext context)
        {
            var events = await context.Events 
                .Include(u => u.Categories)
                .ToListAsync();
            return Ok(events);
        }

        [HttpPost("events")]
        public async Task<IActionResult> PostAsync(
            [FromServices] ApiDataContext context,
            [FromBody] Events events
        ){
            await context.Events.AddAsync(events);
            await context.SaveChangesAsync();

            return Created($"/{events.Id}", events);
        }
    }
}
