﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vdc.Pos.Domain.Entities;

namespace Vdc.Pos.Persistence.IRepositories
{
    public interface IVariationRepository
    {
        Task DeleteByPrimaryKeyAsync(int id);
        Task<IEnumerable<Variation>> GetAllVariationsAsync();
        Task<Variation?> GetByPrimaryKeyAsync(int id);
        Task<IEnumerable<Variation>> GetVaritionsByParentCategoryIdAsync(Guid parentCategoryId);
        ValueTask<EntityEntry<Variation>?> InsertAsync(Variation variation);
        Task<bool> IsUniqueNameAsync(string name);
        Task<bool> IsUniqueNameForParentCategoryAsync(string name, Guid? parentCategoryId);
    }
}