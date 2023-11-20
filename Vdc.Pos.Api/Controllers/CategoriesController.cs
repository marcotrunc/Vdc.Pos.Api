using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vdc.Pos.Business.Services.Interfaces;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;
using Vdc.Pos.Domain.Entities;

namespace Vdc.Pos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController
            (
                ICategoryService categoryService
            ) 
        { 
            _categoryService = categoryService;
        }
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("GetCategoryById")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategoryByIdAsync(Guid id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);

                return Ok(category);
            }
            catch ( Exception ex )
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("GetAllParentCategories")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllParentCategoriesAsync()
        {
            try
            {
                var parentCategories = await _categoryService.GetAllCategoriesAsync();

                return Ok(parentCategories);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("GetAllChildCategories")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllChildCategoriesAsync()
        {
            try
            {
                var childCategories = await _categoryService.GetAllChildCategoriesAsync();

                return Ok(childCategories);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpPost("CreateCategory")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> CreateCategoryAsync(CategoryRequestDto category)
        {
            try
            {
                var categoryCreated = await _categoryService.InsertCategoryAsync(category);
                return Ok(categoryCreated);
            }
            catch ( Exception ex )
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> UpdateCategoryAsync(Guid id, CategoryRequestDto category)
        {
            try
            {
                var categoryUpdated = await _categoryService.UpdateCategory(id, category);
                return Ok(categoryUpdated);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCategoryAsync(Guid id)
        {
            try
            {
                return await _categoryService.DeleteByIdAsync(id);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ex.Message);
            }
        }
    }
}
