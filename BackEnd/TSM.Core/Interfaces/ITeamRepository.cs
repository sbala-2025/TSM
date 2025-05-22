using System.Collections.Generic;
using System.Threading.Tasks;
using TSM.Core.Models;

namespace TSM.Core.Interfaces
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<IEnumerable<Team>> GetTeamsWithUsersAsync();
        Task<Team> GetTeamWithDetailsAsync(int id);
    }
} 