using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vdc.Pos.Domain.Entities;

namespace Vdc.Pos.Persistence.IRepositories
{
    public interface IVariationOptionsRepository
    {
        Task DeleteByPrimaryKeyAsync(int id);
        Task<IEnumerable<VariationOption>> GetAllOptionsAsync();
        Task<VariationOption?> GetByPrimaryKeyAsync(int id);
        Task<IEnumerable<VariationOption>> GetOptionsByVariationIdAsync(int variationId);
        ValueTask<EntityEntry<VariationOption>?> InsertAsync(VariationOption variationOption);
        Task<bool> IsUniqueValueForVariationAsync(string value, int variationId);
    }
}