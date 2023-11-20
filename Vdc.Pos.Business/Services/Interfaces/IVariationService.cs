using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;

namespace Vdc.Pos.Business.Services.Interfaces
{
    public interface IVariationService
    {
        Task<bool> DeleteByIdAsync(int id);
        Task<IEnumerable<VariationResponseDto>> GetAllVariationsAsync();
        Task<VariationResponseDto> GetVariationByIdAsync(int id);
        Task<IEnumerable<VariationResponseDto>> GetVaritionsByParentCategoryIdAsync(Guid parentCategoryId);
        Task<IEnumerable<VariationResponseDto>> InsertMultipleVariationAsync(MultiVariationRequestDto multiVarationsRequest);
        Task<VariationResponseDto> InsertVariationAsync(VariationRequestDto variationRequestDto);
        Task<VariationResponseDto> UpdateVariationAsync(int id, VariationRequestDto variationRequestDto);
    }
}