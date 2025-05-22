using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSM.Core.Interfaces;
using TSM.Core.Models;
using TSM.Infrastructure.Data;

namespace TSM.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Project>> GetProjectsWithTechnologiesAsync()
        {
            return await _context.Projects
                .Include(p => p.ProjectTechnologies!)
                    .ThenInclude(pt => pt.Technology!)
                        .ThenInclude(t => t.Category)
                .ToListAsync();
        }

        public async Task<Project?> GetProjectWithDetailsAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.ProjectTechnologies!)
                    .ThenInclude(pt => pt.Technology!)
                        .ThenInclude(t => t.Category)
                .Include(p => p.ProjectTeamMembers!)
                    .ThenInclude(ptm => ptm.TeamMember)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Project>> GetProjectsByStatusAsync(string status)
        {
            return await _context.Projects
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsByTechnologyAsync(int technologyId)
        {
            return await _context.Projects
                .Include(p => p.ProjectTechnologies!)
                .Where(p => p.ProjectTechnologies!.Any(pt => pt.TechnologyId == technologyId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsByTeamMemberAsync(int memberId)
        {
            return await _context.Projects
                .Include(p => p.ProjectTeamMembers!)
                .Where(p => p.ProjectTeamMembers!.Any(ptm => ptm.MemberId == memberId))
                .ToListAsync();
        }
    }
} 