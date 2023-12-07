using Microsoft.AspNetCore.Mvc;
using Vdc.Pos.Business.Services;
using Vdc.Pos.Business.Services.Interfaces;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vdc.Pos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(
            IBrandService brandService)
        {
            _brandService = brandService;
        }
        
        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<IEnumerable<BrandResponseDto>>> GetAllBrandsAsync()
        {
            try
            {
                var brands = await _brandService.GetAllBrandsAsync();
                return Ok(brands);  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpGet("GetBrandById")]
        public async Task<ActionResult<BrandResponseDto>> GetBrandByIdAsync(Guid id)
        {
            try
            {
                var brand = await _brandService.GetBandByPrimaryAsync(id);
                return Ok(brand);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetBrandsAvailable")]
        public async Task<ActionResult<IEnumerable<BrandResponseDto>>> GetBrandsAvailableAsync()
        {
            try
            {
                var brand = await _brandService.GetBrandsNotDeletedAsync();
                return Ok(brand);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateBrand")]
        public async Task<ActionResult<BrandResponseDto>> CreateBrandAsync([FromForm] BrandRequestDto brandRequest)
        {
            try
            {
                var brandCreated = await _brandService.InsertBrandAsync(brandRequest);
                return Ok(brandCreated);    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
 
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BrandResponseDto>> UpdateBrandAsync(Guid id, [FromForm] BrandRequestDto brandRequest)
        {
            try
            {
                var brandUpdated = await _brandService.UpdateBrandAsync(id, brandRequest);
                return Ok(brandUpdated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } 

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCategoryAsync(Guid id)
        {
            try
            {
                return await _brandService.DeleteByIdAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ex.Message);
            }
        }
    }
}
