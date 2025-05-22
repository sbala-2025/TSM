using System.Collections.Generic;
using System.Threading.Tasks;
using TSM.Core.Models;

namespace TSM.Core.Interfaces
{
    public interface ITechnologyRepository : IRepository<Technology>
    {
        Task<IEnumerable<Technology>> GetTechnologiesWithCategoryAsync();
        Task<Technology> GetTechnologyWithDetailsAsync(int id);
        Task<IEnumerable<Technology>> GetTechnologiesByCategoryAsync(int categoryId);
    }
} 