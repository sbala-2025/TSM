using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSM.Core.DTOs;
using TSM.Core.Interfaces;
using TSM.Core.Models;
using TSM.Infrastructure.Data;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly ApplicationDbContext _context;

        public CategoriesController(IRepository<Category> categoryRepository, ApplicationDbContext context)
        {
            _categoryRepository = categoryRepository;
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            
            var categoryDtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });

            return Ok(categoryDtos);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return categoryDto;
        }

        // POST: api/Categories
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryCreateDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            await _categoryRepository.AddAsync(category);

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryUpdateDto categoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            await _categoryRepository.UpdateAsync(category);

            return NoContent();
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Check if category is being used by any technology
            var hasRelatedTechnologies = await _context.Technologies.AnyAsync(t => t.CategoryId == id);
            if (hasRelatedTechnologies)
            {
                return BadRequest("Cannot delete category because it is being used by one or more technologies.");
            }

            await _categoryRepository.DeleteAsync(category);

            return NoContent();
        }
    }
} 