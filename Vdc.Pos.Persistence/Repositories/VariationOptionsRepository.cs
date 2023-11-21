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
    public class VariationOptionsRepository : IVariationOptionsRepository
    {
        private readonly ApplicationDataContext _dbContext;
        public VariationOptionsRepository(ApplicationDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<VariationOption?> GetByPrimaryKeyAsync(int id)
        {
            return await _dbContext.VariationOptions.AsNoTracking().FirstOrDefaultAsync(vo => vo.Id == id);
        }
        public async Task<IEnumerable<VariationOption>> GetOptionsByVariationIdAsync(int variationId)
        {
            return await _dbContext.VariationOptions.AsNoTracking().Where(vo => vo.VariationId == variationId).ToListAsync();
        }
        public async Task<IEnumerable<VariationOption>> GetAllOptionsAsync()
        {
            return await _dbContext.VariationOptions.AsNoTracking().ToListAsync();
        }
        public async Task<bool> IsUniqueValueForVariationAsync(string value, int variationId)
        {
            return !await _dbContext.VariationOptions.AnyAsync(vo => vo.Value.Trim().ToLower() == value.Trim().ToLower() && vo.VariationId == variationId);
        }
        public async ValueTask<EntityEntry<VariationOption>?> InsertAsync(VariationOption variationOption)
        {
            return await _dbContext.VariationOptions.AddAsync(variationOption);
        }
        public async Task DeleteByPrimaryKeyAsync(int id)
        {
            var variationOptionToDelete = await _dbContext.VariationOptions.AsNoTracking().FirstOrDefaultAsync(vo => vo.Id == id);
            if (variationOptionToDelete != null)
            {
                _dbContext.VariationOptions.Remove(variationOptionToDelete);
            }
        }
    }
}
