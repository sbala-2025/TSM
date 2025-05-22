using System.Collections.Generic;
using System.Threading.Tasks;
using TSM.Core.Models;

namespace TSM.Core.Interfaces
{
    public interface ITeamMemberRepository : IRepository<TeamMember>
    {
        Task<IEnumerable<TeamMember>> GetMembersWithSkillsAsync();
        Task<TeamMember> GetMemberWithDetailsAsync(int id);
        Task<IEnumerable<TeamMember>> GetMembersByProjectAsync(int projectId);
        Task<IEnumerable<TeamMember>> GetMembersByTechnologySkillAsync(int technologyId, int minProficiency = 1);
    }
} 