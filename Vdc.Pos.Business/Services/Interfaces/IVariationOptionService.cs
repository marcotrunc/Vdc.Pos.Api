using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;

namespace Vdc.Pos.Business.Services.Interfaces
{
    public interface IVariationOptionService
    {
        Task<bool> DeleteByIdAsync(int id);
        Task<IEnumerable<VariationOptionResponseDto>> GetAllVariationOptions();
        Task<IEnumerable<VariationOptionResponseDto>> GetOptionsByVariationId(int variationId);
        Task<IEnumerable<VariationOptionResponseDto>> InsertMultipleOptions(MultiOptionRequestDto multiOptionRequest);
        Task<VariationOptionResponseDto> UpdateVariationOptionAsync(int id, VariationOptionRequestDto variationOptionRequest);
    }
}