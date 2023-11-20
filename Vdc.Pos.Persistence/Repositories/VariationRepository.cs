using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.DataContext;

namespace Vdc.Pos.Persistence.Repositories
{
    public class VariationRepository : IVariationRepository
    {
        private readonly ApplicationDataContext _dbContext;
        public VariationRepository(ApplicationDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Variation?> GetByPrimaryKeyAsync(int id)
        {
            return await _dbContext.Variations.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<IEnumerable<Variation>> GetVaritionsByParentCategoryIdAsync(Guid parentCategoryId)
        {
            return await _dbContext.Variations.AsNoTracking().Where(v => v.ParentCategoryId == parentCategoryId).ToListAsync();
        }

        public async Task<IEnumerable<Variation>> GetAllVariationsAsync()
        {
            return await _dbContext.Variations.AsNoTracking().ToListAsync();
        }
        public async Task<bool> IsUniqueNameAsync(string name)
        {
            return !await _dbContext.Variations.AnyAsync(v => v.Name == name);
        }

        public async ValueTask<EntityEntry<Variation>?> InsertAsync(Variation variation)
        {
            return await _dbContext.Variations.AddAsync(variation);
        }
        public async Task DeleteByPrimaryKeyAsync(int id)
        {
            var variationToDelete = await _dbContext.Variations.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            if (variationToDelete != null)
            {
                _dbContext.Variations.Remove(variationToDelete);
            }
        }
    }
}
