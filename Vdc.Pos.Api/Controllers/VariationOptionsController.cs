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
    public class VariationOptionsController : ControllerBase
    {
        private readonly IVariationOptionService _variationOptionService;
        public VariationOptionsController(IVariationOptionService variationOptionService)
        {
            _variationOptionService = variationOptionService;
        }
        [HttpGet("GetAllVariationOptions")]
        public async Task<ActionResult<IEnumerable<VariationOptionResponseDto>>> GetAllVariationOptions()
        {
            try
            {
                var variationOptions = await _variationOptionService.GetAllVariationOptions();
                return Ok(variationOptions);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetVariationOptionsByVariationId")]
        public async Task<ActionResult<IEnumerable<VariationOptionResponseDto>>> GetVariationOptionsByVariationId(int variationId)
        {
            try
            {
                var variationOptions = await _variationOptionService.GetOptionsByVariationId(variationId); 
                return Ok(variationOptions);    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("CreateMultiVariationOption")]
        public async Task<ActionResult<IEnumerable<VariationOptionResponseDto>>> CreateMultiVariationOption(MultiOptionRequestDto multiOptionRequestDto)
        {
            try
            {
                var variationOptionsCreated = await _variationOptionService.InsertMultipleOptions(multiOptionRequestDto);
                return Ok(variationOptionsCreated);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }
        [HttpPut("Update/{id}")]
        public async Task<ActionResult<VariationOptionResponseDto>> UpdateVariationOption(int id, VariationOptionRequestDto variationOptionRequest)
        {
            try
            {
                var variationOptionsUpdated = await _variationOptionService.UpdateVariationOptionAsync(id, variationOptionRequest);
                return Ok(variationOptionsUpdated);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<bool>> DeleteVariationAsync(int id)
        {
            try
            {
                return await _variationOptionService.DeleteByIdAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ex.Message);
            }
        }
    }
}
