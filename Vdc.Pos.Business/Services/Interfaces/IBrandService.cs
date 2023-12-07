using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;

namespace Vdc.Pos.Business.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandResponseDto>> GetAllBrandsAsync();
        Task<BrandResponseDto> GetBandByPrimaryAsync(Guid id);
        Task<IEnumerable<BrandResponseDto>> GetBrandsNotDeletedAsync();
        Task<BrandResponseDto> InsertBrandAsync(BrandRequestDto brandRequestDto);
        Task<BrandResponseDto> UpdateBrandAsync(Guid id, BrandRequestDto brandToUpdate);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}