using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TSM.Core.Interfaces;
using TSM.Core.Models;
using TSM.Infrastructure.Data;

namespace TSM.Infrastructure.Repositories
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        public TeamRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Team>> GetTeamsWithUsersAsync()
        {
            return await _context.Teams
                .Include(t => t.Users)
                .ToListAsync();
        }

        public async Task<Team> GetTeamWithDetailsAsync(int id)
        {
            return await _context.Teams
                .Include(t => t.Users)
                .Include(t => t.TechnologyStatuses)
                    .ThenInclude(ts => ts.Technology)
                        .ThenInclude(tech => tech.Category)
                .Include(t => t.TechnologyStatuses)
                    .ThenInclude(ts => ts.Status)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
} 