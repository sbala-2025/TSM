using System.Collections.Generic;
using System.Threading.Tasks;
using TSM.Core.Models;

namespace TSM.Core.Interfaces
{
    public interface IStatusTypeRepository : IRepository<StatusType>
    {
        Task<IEnumerable<StatusType>> GetStatusTypesWithTechStatusesAsync();
    }
} 