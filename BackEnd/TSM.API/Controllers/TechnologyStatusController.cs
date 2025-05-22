using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TSM.Core.DTOs;
using TSM.Core.Interfaces;
using TSM.Core.Models;
using TSM.Infrastructure.Data;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TechnologyStatusController : ControllerBase
    {
        private readonly IRepository<TechnologyStatus> _statusRepository;
        private readonly ApplicationDbContext _context;

        public TechnologyStatusController(IRepository<TechnologyStatus> statusRepository, ApplicationDbContext context)
        {
            _statusRepository = statusRepository;
            _context = context;
        }

        // GET: api/TechnologyStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechnologyStatusDto>>> GetAllTechnologyStatuses()
        {
            var statuses = await _context.TechnologyStatus
                .Include(ts => ts.Team)
                .Include(ts => ts.Technology)
                .Include(ts => ts.Status)
                .Include(ts => ts.Technology.Category)
                .Include(ts => ts.User)
                .ToListAsync();
            
            var statusDtos = statuses.Select(ts => new TechnologyStatusDto
            {
                Id = ts.Id,
                TeamId = ts.TeamId,
                TechnologyId = ts.TechnologyId,
                StatusId = ts.StatusId,
                LastUpdated = ts.LastUpdated,
                Comments = ts.Comments,
                UpdatedBy = ts.UpdatedBy,
                TeamName = ts.Team?.Name ?? string.Empty,
                TechnologyName = ts.Technology?.Name ?? string.Empty,
                StatusName = ts.Status?.Name ?? string.Empty,
                CategoryName = ts.Technology?.Category?.Name ?? string.Empty,
                UserId = ts.UserId,
                UserName = ts.User != null ? ts.User.FirstName + " " + ts.User.LastName : string.Empty
            });

            return Ok(statusDtos);
        }

        // GET: api/TechnologyStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TechnologyStatusDto>> GetTechnologyStatus(int id)
        {
            var status = await _context.TechnologyStatus
                .Include(ts => ts.Team)
                .Include(ts => ts.Technology)
                .Include(ts => ts.Status)
                .Include(ts => ts.Technology.Category)
                .Include(ts => ts.User)
                .FirstOrDefaultAsync(ts => ts.Id == id);

            if (status == null)
            {
                return NotFound();
            }

            var statusDto = new TechnologyStatusDto
            {
                Id = status.Id,
                TeamId = status.TeamId,
                TechnologyId = status.TechnologyId,
                StatusId = status.StatusId,
                LastUpdated = status.LastUpdated,
                Comments = status.Comments,
                UpdatedBy = status.UpdatedBy,
                TeamName = status.Team?.Name ?? string.Empty,
                TechnologyName = status.Technology?.Name ?? string.Empty,
                StatusName = status.Status?.Name ?? string.Empty,
                CategoryName = status.Technology?.Category?.Name ?? string.Empty,
                UserId = status.UserId,
                UserName = status.User != null ? status.User.FirstName + " " + status.User.LastName : string.Empty
            };

            return statusDto;
        }

        // GET: api/TechnologyStatus/ByTeam/5
        [HttpGet("ByTeam/{teamId}")]
        public async Task<ActionResult<IEnumerable<TechnologyStatusDto>>> GetTechnologyStatusesByTeam(int teamId)
        {
            var statuses = await _context.TechnologyStatus
                .Include(ts => ts.Team)
                .Include(ts => ts.Technology)
                .Include(ts => ts.Status)
                .Include(ts => ts.Technology.Category)
                .Include(ts => ts.User)
                .Where(ts => ts.TeamId == teamId)
                .ToListAsync();
            
            var statusDtos = statuses.Select(ts => new TechnologyStatusDto
            {
                Id = ts.Id,
                TeamId = ts.TeamId,
                TechnologyId = ts.TechnologyId,
                StatusId = ts.StatusId,
                LastUpdated = ts.LastUpdated,
                Comments = ts.Comments,
                UpdatedBy = ts.UpdatedBy,
                TeamName = ts.Team?.Name ?? string.Empty,
                TechnologyName = ts.Technology?.Name ?? string.Empty,
                StatusName = ts.Status?.Name ?? string.Empty,
                CategoryName = ts.Technology?.Category?.Name ?? string.Empty,
                UserId = ts.UserId,
                UserName = ts.User != null ? ts.User.FirstName + " " + ts.User.LastName : string.Empty
            });

            return Ok(statusDtos);
        }

        // GET: api/TechnologyStatus/ByTechnology/5
        [HttpGet("ByTechnology/{technologyId}")]
        public async Task<ActionResult<IEnumerable<TechnologyStatusDto>>> GetTechnologyStatusesByTechnology(int technologyId)
        {
            var statuses = await _context.TechnologyStatus
                .Include(ts => ts.Team)
                .Include(ts => ts.Technology)
                .Include(ts => ts.Status)
                .Include(ts => ts.Technology.Category)
                .Include(ts => ts.User)
                .Where(ts => ts.TechnologyId == technologyId)
                .ToListAsync();
            
            var statusDtos = statuses.Select(ts => new TechnologyStatusDto
            {
                Id = ts.Id,
                TeamId = ts.TeamId,
                TechnologyId = ts.TechnologyId,
                StatusId = ts.StatusId,
                LastUpdated = ts.LastUpdated,
                Comments = ts.Comments,
                UpdatedBy = ts.UpdatedBy,
                TeamName = ts.Team?.Name ?? string.Empty,
                TechnologyName = ts.Technology?.Name ?? string.Empty,
                StatusName = ts.Status?.Name ?? string.Empty,
                CategoryName = ts.Technology?.Category?.Name ?? string.Empty,
                UserId = ts.UserId,
                UserName = ts.User != null ? ts.User.FirstName + " " + ts.User.LastName : string.Empty
            });

            return Ok(statusDtos);
        }

        // GET: api/TechnologyStatus/Matrix
        [HttpGet("Matrix")]
        public async Task<ActionResult<IEnumerable<TechnologyStatusMatrixDto>>> GetTechnologyStatusMatrix()
        {
            var statuses = await _context.TechnologyStatus
                .Include(ts => ts.Team)
                .Include(ts => ts.Technology)
                .Include(ts => ts.Status)
                .Include(ts => ts.User)
                .ToListAsync();
            
            var matrix = statuses.Select(ts => new TechnologyStatusMatrixDto
            {
                TeamId = ts.TeamId,
                TeamName = ts.Team?.Name ?? string.Empty,
                TechnologyId = ts.TechnologyId,
                TechnologyName = ts.Technology?.Name ?? string.Empty,
                StatusId = ts.StatusId,
                StatusName = ts.Status?.Name ?? string.Empty,
                Comments = ts.Comments,
                LastUpdated = ts.LastUpdated,
                UserId = ts.UserId,
                UserName = ts.User != null ? ts.User.FirstName + " " + ts.User.LastName : string.Empty
            });

            return Ok(matrix);
        }

        // POST: api/TechnologyStatus
        [HttpPost]
        public async Task<ActionResult<TechnologyStatusDto>> CreateTechnologyStatus(TechnologyStatusCreateDto statusDto)
        {
            // Check if the team exists
            var teamExists = await _context.Teams.AnyAsync(t => t.Id == statusDto.TeamId);
            if (!teamExists)
            {
                return BadRequest("Invalid Team ID");
            }

            // Check if the technology exists
            var technologyExists = await _context.Technologies.AnyAsync(t => t.Id == statusDto.TechnologyId);
            if (!technologyExists)
            {
                return BadRequest("Invalid Technology ID");
            }

            // Check if the status exists
            var statusTypeExists = await _context.StatusTypes.AnyAsync(s => s.Id == statusDto.StatusId);
            if (!statusTypeExists)
            {
                return BadRequest("Invalid Status ID");
            }

            // Check if the status already exists for this team and technology
            var existingStatus = await _context.TechnologyStatus
                .FirstOrDefaultAsync(ts => ts.TeamId == statusDto.TeamId && ts.TechnologyId == statusDto.TechnologyId);
            
            if (existingStatus != null)
            {
                return BadRequest("A status for this team and technology already exists. Please use the update endpoint.");
            }

            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? "System";

            var status = new TechnologyStatus
            {
                TeamId = statusDto.TeamId,
                TechnologyId = statusDto.TechnologyId,
                StatusId = statusDto.StatusId,
                Comments = statusDto.Comments,
                UpdatedBy = statusDto.UpdatedBy ?? username,
                LastUpdated = DateTime.UtcNow,
                UserId = statusDto.UserId
            };

            await _statusRepository.AddAsync(status);

            var createdStatus = await _context.TechnologyStatus
                .Include(ts => ts.Team)
                .Include(ts => ts.Technology)
                .Include(ts => ts.Status)
                .Include(ts => ts.Technology.Category)
                .Include(ts => ts.User)
                .FirstOrDefaultAsync(ts => ts.Id == status.Id);

            if (createdStatus == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve created status");
            }

            var createdStatusDto = new TechnologyStatusDto
            {
                Id = createdStatus.Id,
                TeamId = createdStatus.TeamId,
                TechnologyId = createdStatus.TechnologyId,
                StatusId = createdStatus.StatusId,
                LastUpdated = createdStatus.LastUpdated,
                Comments = createdStatus.Comments,
                UpdatedBy = createdStatus.UpdatedBy,
                TeamName = createdStatus.Team?.Name ?? string.Empty,
                TechnologyName = createdStatus.Technology?.Name ?? string.Empty,
                StatusName = createdStatus.Status?.Name ?? string.Empty,
                CategoryName = createdStatus.Technology?.Category?.Name ?? string.Empty,
                UserId = createdStatus.UserId,
                UserName = createdStatus.User != null ? createdStatus.User.FirstName + " " + createdStatus.User.LastName : string.Empty
            };

            return CreatedAtAction(nameof(GetTechnologyStatus), new { id = status.Id }, createdStatusDto);
        }

        // PUT: api/TechnologyStatus/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTechnologyStatus(int id, TechnologyStatusUpdateDto statusDto)
        {
            var status = await _statusRepository.GetByIdAsync(id);
            if (status == null)
            {
                return NotFound();
            }

            // Check if the user has permission to update this status
            // Admin users can update any status, but non-admin users can only update statuses for their team
            if (!User.IsInRole("Admin"))
            {
                // Get the user's team ID
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
                
                if (user == null || user.TeamId != status.TeamId)
                {
                    return Forbid();
                }
            }

            // Check if the status exists
            var statusTypeExists = await _context.StatusTypes.AnyAsync(s => s.Id == statusDto.StatusId);
            if (!statusTypeExists)
            {
                return BadRequest("Invalid Status ID");
            }

            var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value ?? "System";

            status.StatusId = statusDto.StatusId;
            status.Comments = statusDto.Comments;
            status.UpdatedBy = statusDto.UpdatedBy ?? currentUsername;
            status.LastUpdated = DateTime.UtcNow;
            status.UserId = statusDto.UserId;

            await _statusRepository.UpdateAsync(status);

            return NoContent();
        }

        // DELETE: api/TechnologyStatus/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTechnologyStatus(int id)
        {
            var status = await _statusRepository.GetByIdAsync(id);
            if (status == null)
            {
                return NotFound();
            }

            await _statusRepository.DeleteAsync(status);

            return NoContent();
        }
    }
} 