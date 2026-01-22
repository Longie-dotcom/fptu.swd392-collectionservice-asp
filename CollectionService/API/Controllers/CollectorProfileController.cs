using Application.DTO;
using Application.Helper;
using Application.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<CollectorProfileDTO>>> GetAllCollectorProfile([FromBody] string? sortBy, [FromBody] QueryCollectorProfileDTO dto)
        {
            var result = await collectorProfileService.GetAllCollectorProfile(sortBy ?? string.Empty, dto);
            if (result.IsSuccess)
            {
                if(result.Data == null || !result.Data.Any())
                {
                    return Ok(GenericResult<IEnumerable<CollectorProfileDTO>>.Success(
                        new List<CollectorProfileDTO>(),
                        "No Data"));
                }
                return Ok(result);
            }
            return BadRequest(result);
        }

        //[HttpPost]
        //public async Task<ActionResult> AddCollectorProfile([FromBody] CollectorProfileCreateDTO dto)
        //{
        //    var performedBy = "System";
        //    var result = await collectorProfileService.AddCollectorProfileAsync(performedBy, dto);
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}
    }
}
