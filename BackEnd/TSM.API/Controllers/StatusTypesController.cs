using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSM.Infrastructure.Data;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatusTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StatusTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StatusTypes
        [HttpGet]
        public async Task<IActionResult> GetStatusTypes()
        {
            var statusTypes = await _context.StatusTypes.ToListAsync();
            return Ok(statusTypes);
        }

        // GET: api/StatusTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStatusType(int id)
        {
            var statusType = await _context.StatusTypes.FindAsync(id);

            if (statusType == null)
            {
                return NotFound();
            }

            return Ok(statusType);
        }
    }
} 