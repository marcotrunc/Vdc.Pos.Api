using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vdc.Pos.Business.Services;
using Vdc.Pos.Business.Services.Interfaces;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;

namespace Vdc.Pos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariationController : ControllerBase
    {
        private readonly IVariationService _variationService;
        public VariationController(IVariationService variationService)
        {
            _variationService = variationService;
        }
        [HttpGet("GetAllVariations")]
        public async Task<ActionResult<IEnumerable<VariationResponseDto>>> GetAllVariationsAsync()
        {
            try
            {
                var variations = await _variationService.GetAllVariationsAsync();
                return Ok(variations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetVariationById")]
        public async Task<ActionResult<VariationResponseDto>> GetVariationByIdAsync(int id)
        {
            try
            {
                var variation = await _variationService.GetVariationByIdAsync(id);
                return Ok(variation);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("GetVariationByParentCategoryId")]
        public async Task<ActionResult<VariationResponseDto>> GetVaritionsByParentCategoryIdAsync(Guid parentCategoryId)
        {
            try
            {
                var variation = await _variationService.GetVaritionsByParentCategoryIdAsync(parentCategoryId);
                return Ok(variation);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("CreateVariationAsync")]
        public async Task<ActionResult<VariationResponseDto>> CreateVariationAsync(VariationRequestDto variationRequestDto)
        {
            try
            {
                var variationCreated = await _variationService.InsertVariationAsync(variationRequestDto);
                return Ok(variationCreated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("CreateMultiVariationAsync")]
        public async Task<ActionResult<VariationResponseDto>> CreateMultiVariationAsync(MultiVariationRequestDto multiVariationRequestDto)
        {
            try
            {
                var variationCreated = await _variationService.InsertMultipleVariationAsync(multiVariationRequestDto);
                return Ok(variationCreated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("Update/{id}")]
        public async Task<ActionResult<VariationResponseDto>> UpdateVariationAsync(int id, VariationRequestDto variationRequestDto)
        {
            try
            {
                var variationUpdated = await _variationService.UpdateVariationAsync(id, variationRequestDto);  
                return Ok(variationUpdated);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<bool>> DeleteVariationAsync(int id)
        {
            try
            {
                return await _variationService.DeleteByIdAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ex.Message);
            }
        }
    }   
}
