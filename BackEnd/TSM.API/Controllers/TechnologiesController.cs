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
    public class TechnologiesController : ControllerBase
    {
        private readonly IRepository<Technology> _technologyRepository;
        private readonly ApplicationDbContext _context;

        public TechnologiesController(IRepository<Technology> technologyRepository, ApplicationDbContext context)
        {
            _technologyRepository = technologyRepository;
            _context = context;
        }

        // GET: api/Technologies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechnologyDto>>> GetTechnologies()
        {
            var technologies = await _context.Technologies
                .Include(t => t.Category)
                .ToListAsync();
            
            var technologyDtos = technologies.Select(t => new TechnologyDto
            {
                Id = t.Id,
                Name = t.Name,
                CategoryId = t.CategoryId,
                CategoryName = t.Category?.Name ?? string.Empty,
                Description = t.Description,
                Version = t.Version
            });

            return Ok(technologyDtos);
        }

        // GET: api/Technologies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TechnologyDto>> GetTechnology(int id)
        {
            var technology = await _context.Technologies
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (technology == null)
            {
                return NotFound();
            }

            var technologyDto = new TechnologyDto
            {
                Id = technology.Id,
                Name = technology.Name,
                CategoryId = technology.CategoryId,
                CategoryName = technology.Category?.Name ?? string.Empty,
                Description = technology.Description,
                Version = technology.Version
            };

            return technologyDto;
        }

        // GET: api/Technologies/ByCategory/5
        [HttpGet("ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<TechnologyDto>>> GetTechnologiesByCategory(int categoryId)
        {
            var technologies = await _context.Technologies
                .Include(t => t.Category)
                .Where(t => t.CategoryId == categoryId)
                .ToListAsync();
            
            var technologyDtos = technologies.Select(t => new TechnologyDto
            {
                Id = t.Id,
                Name = t.Name,
                CategoryId = t.CategoryId,
                CategoryName = t.Category?.Name ?? string.Empty,
                Description = t.Description,
                Version = t.Version
            });

            return Ok(technologyDtos);
        }

        // POST: api/Technologies
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TechnologyDto>> CreateTechnology(TechnologyCreateDto technologyDto)
        {
            // Check if the category exists
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == technologyDto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Invalid Category ID");
            }

            var technology = new Technology
            {
                Name = technologyDto.Name,
                CategoryId = technologyDto.CategoryId,
                Description = technologyDto.Description,
                Version = technologyDto.Version
            };

            await _technologyRepository.AddAsync(technology);

            return CreatedAtAction(nameof(GetTechnology), new { id = technology.Id }, new TechnologyDto
            {
                Id = technology.Id,
                Name = technology.Name,
                CategoryId = technology.CategoryId,
                Description = technology.Description,
                Version = technology.Version
            });
        }

        // PUT: api/Technologies/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTechnology(int id, TechnologyUpdateDto technologyDto)
        {
            var technology = await _technologyRepository.GetByIdAsync(id);
            if (technology == null)
            {
                return NotFound();
            }

            // Check if the category exists
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == technologyDto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Invalid Category ID");
            }

            technology.Name = technologyDto.Name;
            technology.CategoryId = technologyDto.CategoryId;
            technology.Description = technologyDto.Description;
            technology.Version = technologyDto.Version;

            await _technologyRepository.UpdateAsync(technology);

            return NoContent();
        }

        // DELETE: api/Technologies/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTechnology(int id)
        {
            var technology = await _technologyRepository.GetByIdAsync(id);
            if (technology == null)
            {
                return NotFound();
            }

            // Check if technology is being used by any team
            var hasRelatedStatus = await _context.TechnologyStatus.AnyAsync(ts => ts.TechnologyId == id);
            if (hasRelatedStatus)
            {
                return BadRequest("Cannot delete technology because it is being used by one or more teams.");
            }

            await _technologyRepository.DeleteAsync(technology);

            return NoContent();
        }
    }
} 