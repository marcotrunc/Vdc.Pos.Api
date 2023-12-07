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
    public class BrandRepository : IBrandrepository
    {
        private readonly ApplicationDataContext _dbContext;
        public BrandRepository(ApplicationDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteByPrimaryKeyAsync(Guid id)
        {
            var brandToDelete = await _dbContext.Brands.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            if(brandToDelete != null)
            {
                _dbContext.Brands.Remove(brandToDelete);
            }
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _dbContext.Brands.AsNoTracking().ToListAsync();    
        }

        public async Task<IEnumerable<Brand>> GetBrandsNotDeletedAsync()
        {
            return await _dbContext.Brands.AsNoTracking().Where(b => b.IsDeleted == false).ToListAsync();
        }
        public async Task<Brand?> GetByPrimaryKeyAsync(Guid id)
        {
            return await _dbContext.Brands.AsNoTracking().FirstOrDefaultAsync(b =>  (b.Id == id));  
        }

        public async ValueTask<EntityEntry<Brand>?> InsertAsync(Brand entity)
        {
            return await _dbContext.Brands.AddAsync(entity);
        }
        public async Task<bool> IsUniqueBrandName(string name)
        {
            return !await _dbContext.Brands.AnyAsync(b => b.Name.Trim().ToLower() == name.Trim().ToLower());
        }
    }
}
