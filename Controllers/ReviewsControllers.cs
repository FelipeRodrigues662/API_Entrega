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
    public class ReviewsController : ControllerBase
    {
        private readonly ApiDataContext _context;

        public ReviewsController(ApiDataContext context)
        {
            _context = context;
        }

        [HttpGet("reviews")]
        public async Task<ActionResult<IEnumerable<Reviews>>> GetAllReviews()
        {
            var reviews = await _context.Reviews.ToListAsync();
            return Ok(reviews);
        }

        [HttpPost("reviews")]
        public async Task<ActionResult<Reviews>> CreateReview([FromBody] Reviews review)
        {
            if (ModelState.IsValid)
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetReviewById", new { id = review.Id }, review);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("reviews/{id}")]
        public async Task<ActionResult<Reviews>> GetReviewById(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        [HttpPut("reviews/{id}")]
        public async Task<IActionResult> UpdateReviewById(int id, [FromBody] Reviews updatedReview)
        {
            if (id != updatedReview.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedReview).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        [HttpDelete("reviews/{id}")]
        public async Task<IActionResult> DeleteReviewById(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(r => r.Id == id);
        }
    }
}
