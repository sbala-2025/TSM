using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TSM.Core.Interfaces;
using TSM.Core.Models;
using TSM.Infrastructure.Data;

namespace TSM.Infrastructure.Repositories
{
    public class StatusTypeRepository : Repository<StatusType>, IStatusTypeRepository
    {
        public StatusTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StatusType>> GetStatusTypesWithTechStatusesAsync()
        {
            return await _context.StatusTypes
                .Include(st => st.TechnologyStatuses)
                .ToListAsync();
        }
    }
} 