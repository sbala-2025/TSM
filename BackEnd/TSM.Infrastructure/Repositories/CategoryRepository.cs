using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TSM.Core.Interfaces;
using TSM.Core.Models;
using TSM.Infrastructure.Data;

namespace TSM.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithTechnologiesAsync()
        {
            return await _context.Categories
                .Include(c => c.Technologies)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryWithTechnologiesAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Technologies)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
} 