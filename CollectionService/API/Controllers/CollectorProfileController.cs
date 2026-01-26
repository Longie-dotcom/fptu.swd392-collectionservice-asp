using Application.DTO;
using Application.Helper;
using Application.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollectorProfileController : ControllerBase
    {
        #region Attributes
        private readonly ICollectorProfileService collectorProfileService;
        #endregion

        public CollectorProfileController(ICollectorProfileService collectorProfileService)
        {
            this.collectorProfileService = collectorProfileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectorProfileDTO>>> GetAllCollectorProfile([FromQuery] string? sortBy, [FromQuery] QueryCollectorProfileDTO dto)
        {
            var result = await collectorProfileService.GetAllCollectorProfile(sortBy ?? string.Empty, dto);
            if (result.IsSuccess)
            {
                if(result.Data == null || !result.Data.Any())
                {
                    return Ok(GenericResult<IEnumerable<CollectorProfileDTO>>.Success(
                        new List<CollectorProfileDTO>(),
                        "No data found"));
                }
                return Ok(result.Data);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("{collectorProfileID:guid}")]
        public async Task<ActionResult<CollectorProfileDTO>> GetCollectorProfileById(Guid collectorProfileID)
        {
            var result = await collectorProfileService.GetCollectorProfileByIdAsync(collectorProfileID);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return NotFound(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCollectorProfile([FromBody] CollectorProfileCreateDTO dto)
        {
            string createdBy = User?.Identity?.Name ?? "System";
            var result = await collectorProfileService.CreateCollectorProfileAsync(createdBy, dto);

            if(result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetCollectorProfileById), new { collectorProfileID = result.Data.CollectorProfileID }, result.Data);
            }

            return BadRequest(result.Errors);
        }

        [HttpPut("{collectorProfileID:guid}")]
        public async Task<IActionResult> UpdateCollectorProfile(Guid collectorProfileID, [FromBody] CollectorProfileUpdateDTO dto)
        {
            string updatedBy = User?.Identity?.Name ?? "System";
            var result = await collectorProfileService.UpdateCollectorProfileAsync(collectorProfileID, dto, updatedBy);

            if(result.IsSuccess)
            {
                return Ok(result.Data);
            }

            if(result.Errors.Contains("Collector Profile not found"))
            {
                return NotFound(result.Errors);
            }
            return BadRequest(result.Errors);
        }

        [HttpDelete("{collectorProfileID:guid}")]
        public async Task<IActionResult> DeleteCollectorProfile(Guid
             collectorProfileID)
        {
            string deletedBy = User?.Identity.Name ?? "System";
            var (success, error) = await collectorProfileService.DeleteCollectorProfileAsync(collectorProfileID, deletedBy);

            if (!success)
            {
                if(error == "Collector Profile not found")
                    return NotFound(error);
                return BadRequest(error);
            }
            return Ok("Collector Profile deleted successfully");
        }

        [HttpGet("active-collectors")]
        public async Task<ActionResult<IEnumerable<CollectorProfileDTO>>> GetActiveCollectors([FromQuery] string? wardCode)
        {
            string userWard = User.FindFirst("wardCode")?.Value ?? wardCode;
            var result = await collectorProfileService.GetActiveCollectorsAsync(userWard);

            if(result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("my-profile")]
        public async Task<ActionResult<CollectorProfileDTO>> GetMyProfile()
        {
            string userId = User.Identity?.Name ?? string.Empty;
            var result = await collectorProfileService.GetCollectorProfileByUserIdAsync(userId);

            if(result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound("Collector Profile not found");
        }

    }
}
