using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.DataContext;
using Vdc.Pos.Persistence.IRepositories;

namespace Vdc.Pos.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDataContext _dbContext;
        public CategoryRepository(ApplicationDataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("Db Context Non generato");
        }
        public async Task DeleteByPrimaryKeyAsync(Guid id)
        {
            var categoryToDelete = await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if(categoryToDelete != null)
            {
                _dbContext.Categories.Remove(categoryToDelete);
            }
        }

        public async Task<IEnumerable<Category>> FindAllChildCategory()
        {
            return await _dbContext.Categories.AsNoTracking().Where(c => c.ParentId != null).ToListAsync();
        }

        public async Task<IEnumerable<Category>> FindAllParentCategory()
        {
            return await _dbContext.Categories.AsNoTracking().Where(c => c.ParentId == null ).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbContext.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetByPrimaryKeyAsync(Guid id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public async ValueTask<EntityEntry<Category>?> InsertAsync(Category entity)
        {
            return await _dbContext.Categories.AddAsync(entity);
        }

        public async Task<bool> IsUniqueCategoryNameForThisParentCategory(string name, Guid parentId)
        {
            return await _dbContext.Categories.AsNoTracking().Where(c => c.ParentId == parentId && c.Name.ToLower().Trim() == name.ToLower().Trim()).AnyAsync();
        }

        public async Task<bool> IsUniqueParentCategoryName(string name)
        {
            return await _dbContext.Categories.AsNoTracking().Where(c => c.ParentId == null && c.Name.ToLower().Trim() == name.ToLower().Trim()).AnyAsync();
        }

    }
}
