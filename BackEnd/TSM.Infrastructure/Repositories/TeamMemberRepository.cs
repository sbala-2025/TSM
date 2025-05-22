using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSM.Core.Interfaces;
using TSM.Core.Models;
using TSM.Infrastructure.Data;

namespace TSM.Infrastructure.Repositories
{
    public class TeamMemberRepository : Repository<TeamMember>, ITeamMemberRepository
    {
        public TeamMemberRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TeamMember>> GetMembersWithSkillsAsync()
        {
            return await _context.TeamMembers
                .Include(tm => tm.TechnologySkills)
                    .ThenInclude(ts => ts.Technology)
                        .ThenInclude(t => t.Category)
                .ToListAsync();
        }

        public async Task<TeamMember> GetMemberWithDetailsAsync(int id)
        {
            return await _context.TeamMembers
                .Include(tm => tm.User)
                .Include(tm => tm.TechnologySkills)
                    .ThenInclude(ts => ts.Technology)
                        .ThenInclude(t => t.Category)
                .Include(tm => tm.ProjectTeamMembers)
                    .ThenInclude(ptm => ptm.Project)
                .FirstOrDefaultAsync(tm => tm.Id == id);
        }

        public async Task<IEnumerable<TeamMember>> GetMembersByProjectAsync(int projectId)
        {
            return await _context.TeamMembers
                .Include(tm => tm.ProjectTeamMembers)
                .Where(tm => tm.ProjectTeamMembers.Any(ptm => ptm.ProjectId == projectId))
                .ToListAsync();
        }

        public async Task<IEnumerable<TeamMember>> GetMembersByTechnologySkillAsync(int technologyId, int minProficiency = 1)
        {
            return await _context.TeamMembers
                .Include(tm => tm.TechnologySkills)
                .Where(tm => tm.TechnologySkills.Any(ts => 
                    ts.TechnologyId == technologyId && ts.ProficiencyLevel >= minProficiency))
                .ToListAsync();
        }
    }
} 