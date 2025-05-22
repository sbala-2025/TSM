using System.Collections.Generic;
using System.Threading.Tasks;
using TSM.Core.Models;

namespace TSM.Core.Interfaces
{
    public interface ITechnologyStatusRepository : IRepository<TechnologyStatus>
    {
        Task<IEnumerable<TechnologyStatus>> GetStatusesByTeamAsync(int teamId);
        Task<IEnumerable<TechnologyStatus>> GetStatusesByTechnologyAsync(int technologyId);
        Task<TechnologyStatus> GetStatusByTeamAndTechnologyAsync(int teamId, int technologyId);
    }
} 