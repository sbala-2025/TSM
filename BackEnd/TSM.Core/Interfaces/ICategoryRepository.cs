using System.Collections.Generic;
using System.Threading.Tasks;
using TSM.Core.Models;

namespace TSM.Core.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesWithTechnologiesAsync();
        Task<Category> GetCategoryWithTechnologiesAsync(int id);
    }
} 