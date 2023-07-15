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
    public class CategoriesController : ControllerBase
    {
        private readonly ApiDataContext _context;

        public CategoriesController(ApiDataContext context)
        {
            _context = context;
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<Categories>>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpPost("categories")]
        public async Task<ActionResult<Categories>> CreateCategory([FromBody] Categories category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCategoryById", new { id = category.Id }, category);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("categories/{id}")]
        public async Task<ActionResult<Categories>> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategoryById(int id, [FromBody] Categories updatedCategory)
        {
            if (id != updatedCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }
    }
}
