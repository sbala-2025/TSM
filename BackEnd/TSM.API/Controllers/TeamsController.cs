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
    public class TeamsController : ControllerBase
    {
        private readonly IRepository<Team> _teamRepository;
        private readonly ApplicationDbContext _context;

        public TeamsController(IRepository<Team> teamRepository, ApplicationDbContext context)
        {
            _teamRepository = teamRepository;
            _context = context;
        }

        // GET: api/Teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams()
        {
            var teams = await _teamRepository.GetAllAsync();
            
            var teamDtos = teams.Select(t => new TeamDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Department = t.Department
            });

            return Ok(teamDtos);
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDto>> GetTeam(int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            var teamDto = new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                Department = team.Department
            };

            return teamDto;
        }

        // GET: api/Teams/ByDepartment/Engineering
        [HttpGet("ByDepartment/{department}")]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeamsByDepartment(string department)
        {
            var teams = await _context.Teams
                .Where(t => t.Department == department)
                .ToListAsync();
            
            var teamDtos = teams.Select(t => new TeamDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Department = t.Department
            });

            return Ok(teamDtos);
        }

        // POST: api/Teams
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TeamDto>> CreateTeam(TeamCreateDto teamDto)
        {
            var team = new Team
            {
                Name = teamDto.Name,
                Description = teamDto.Description,
                Department = teamDto.Department
            };

            await _teamRepository.AddAsync(team);

            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                Department = team.Department
            });
        }

        // PUT: api/Teams/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTeam(int id, TeamUpdateDto teamDto)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            team.Name = teamDto.Name;
            team.Description = teamDto.Description;
            team.Department = teamDto.Department;

            await _teamRepository.UpdateAsync(team);

            return NoContent();
        }

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            // Check if team has any technology statuses
            var hasRelatedStatus = await _context.TechnologyStatus.AnyAsync(ts => ts.TeamId == id);
            if (hasRelatedStatus)
            {
                return BadRequest("Cannot delete team because it has associated technology statuses.");
            }

            // Check if team has any users
            var hasUsers = await _context.Users.AnyAsync(u => u.TeamId == id);
            if (hasUsers)
            {
                return BadRequest("Cannot delete team because it has associated users.");
            }

            await _teamRepository.DeleteAsync(team);

            return NoContent();
        }
    }
} 