using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;
using Vdc.Pos.Domain.Entities;

namespace Vdc.Pos.Business.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
        Task<IEnumerable<CategoryResponseDto>> GetAllChildCategoriesAsync();
        Task<IEnumerable<CategoryResponseDto>> GetAllParentCategoriesAsync();
        Task<CategoryResponseDto> GetCategoryByIdAsync(Guid id);
        Task<CategoryResponseDto> InsertCategoryAsync(CategoryRequestDto categoryToInsert);
        Task<CategoryResponseDto> UpdateCategory(Guid id, CategoryRequestDto categoryToUpdate);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}