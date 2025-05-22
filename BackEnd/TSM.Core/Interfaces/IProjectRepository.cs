using System.Collections.Generic;
using System.Threading.Tasks;
using TSM.Core.Models;

namespace TSM.Core.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IEnumerable<Project>> GetProjectsWithTechnologiesAsync();
        Task<Project> GetProjectWithDetailsAsync(int id);
        Task<IEnumerable<Project>> GetProjectsByStatusAsync(string status);
        Task<IEnumerable<Project>> GetProjectsByTechnologyAsync(int technologyId);
        Task<IEnumerable<Project>> GetProjectsByTeamMemberAsync(int memberId);
    }
} 