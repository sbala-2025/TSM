using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSM.Core.Interfaces;
using TSM.Core.Models;
using TSM.Infrastructure.Data;

namespace TSM.Infrastructure.Repositories
{
    public class TechnologyStatusRepository : Repository<TechnologyStatus>, ITechnologyStatusRepository
    {
        public TechnologyStatusRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TechnologyStatus>> GetStatusesByTeamAsync(int teamId)
        {
            return await _context.TechnologyStatus
                .Where(ts => ts.TeamId == teamId)
                .Include(ts => ts.Technology)
                    .ThenInclude(t => t.Category)
                .Include(ts => ts.Status)
                .ToListAsync();
        }

        public async Task<IEnumerable<TechnologyStatus>> GetStatusesByTechnologyAsync(int technologyId)
        {
            return await _context.TechnologyStatus
                .Where(ts => ts.TechnologyId == technologyId)
                .Include(ts => ts.Team)
                .Include(ts => ts.Status)
                .ToListAsync();
        }

        public async Task<TechnologyStatus> GetStatusByTeamAndTechnologyAsync(int teamId, int technologyId)
        {
            return await _context.TechnologyStatus
                .Where(ts => ts.TeamId == teamId && ts.TechnologyId == technologyId)
                .Include(ts => ts.Team)
                .Include(ts => ts.Technology)
                    .ThenInclude(t => t.Category)
                .Include(ts => ts.Status)
                .FirstOrDefaultAsync();
        }
    }
} 