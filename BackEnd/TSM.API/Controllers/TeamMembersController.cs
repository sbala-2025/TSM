using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TSM.Core.Interfaces;
using TSM.Core.Models;

namespace TSM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMembersController : ControllerBase
    {
        private readonly ITeamMemberRepository _teamMemberRepository;

        public TeamMembersController(ITeamMemberRepository teamMemberRepository)
        {
            _teamMemberRepository = teamMemberRepository;
        }

        // GET: api/TeamMembers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamMember>>> GetTeamMembers()
        {
            var members = await _teamMemberRepository.GetAllAsync();
            return Ok(members);
        }

        // GET: api/TeamMembers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamMember>> GetTeamMember(int id)
        {
            var member = await _teamMemberRepository.GetMemberWithDetailsAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        // GET: api/TeamMembers/project/5
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<TeamMember>>> GetTeamMembersByProject(int projectId)
        {
            var members = await _teamMemberRepository.GetMembersByProjectAsync(projectId);
            return Ok(members);
        }

        // GET: api/TeamMembers/skill/5/3
        [HttpGet("skill/{technologyId}/{minProficiency=1}")]
        public async Task<ActionResult<IEnumerable<TeamMember>>> GetTeamMembersBySkill(int technologyId, int minProficiency = 1)
        {
            var members = await _teamMemberRepository.GetMembersByTechnologySkillAsync(technologyId, minProficiency);
            return Ok(members);
        }

        // POST: api/TeamMembers
        [HttpPost]
        public async Task<ActionResult<TeamMember>> CreateTeamMember(TeamMember teamMember)
        {
            await _teamMemberRepository.AddAsync(teamMember);
            return CreatedAtAction(nameof(GetTeamMember), new { id = teamMember.Id }, teamMember);
        }

        // PUT: api/TeamMembers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeamMember(int id, TeamMember teamMember)
        {
            if (id != teamMember.Id)
            {
                return BadRequest();
            }

            await _teamMemberRepository.UpdateAsync(teamMember);
            return NoContent();
        }

        // DELETE: api/TeamMembers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamMember(int id)
        {
            var teamMember = await _teamMemberRepository.GetByIdAsync(id);
            if (teamMember == null)
            {
                return NotFound();
            }

            await _teamMemberRepository.DeleteAsync(teamMember);
            return NoContent();
        }
    }
} 