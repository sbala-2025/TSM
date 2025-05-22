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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var projects = await _projectRepository.GetAllAsync();
            return Ok(projects);
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _projectRepository.GetProjectWithDetailsAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // GET: api/Projects/technology/5
        [HttpGet("technology/{technologyId}")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectsByTechnology(int technologyId)
        {
            var projects = await _projectRepository.GetProjectsByTechnologyAsync(technologyId);
            return Ok(projects);
        }

        // GET: api/Projects/status/Active
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectsByStatus(string status)
        {
            var projects = await _projectRepository.GetProjectsByStatusAsync(status);
            return Ok(projects);
        }

        // POST: api/Projects
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            await _projectRepository.AddAsync(project);
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            await _projectRepository.UpdateAsync(project);
            return NoContent();
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            await _projectRepository.DeleteAsync(project);
            return NoContent();
        }
    }
} 