using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSM.Core.Interfaces;
using TSM.Core.Models;
using TSM.Infrastructure.Data;

namespace TSM.Infrastructure.Repositories
{
    public class TechnologyRepository : Repository<Technology>, ITechnologyRepository
    {
        public TechnologyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Technology>> GetTechnologiesWithCategoryAsync()
        {
            return await _context.Technologies
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task<Technology> GetTechnologyWithDetailsAsync(int id)
        {
            return await _context.Technologies
                .Include(t => t.Category)
                .Include(t => t.TechnologyStatuses)
                    .ThenInclude(ts => ts.Team)
                .Include(t => t.TechnologyStatuses)
                    .ThenInclude(ts => ts.Status)
                .Include(t => t.ProjectTechnologies)
                    .ThenInclude(pt => pt.Project)
                .Include(t => t.TechnologySkills)
                    .ThenInclude(ts => ts.TeamMember)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Technology>> GetTechnologiesByCategoryAsync(int categoryId)
        {
            return await _context.Technologies
                .Where(t => t.CategoryId == categoryId)
                .Include(t => t.Category)
                .ToListAsync();
        }
    }
} 