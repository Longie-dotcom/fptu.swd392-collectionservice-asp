using API.Helper;
using Application.DTO;
using Application.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollectorProfileController : ControllerBase
    {
        #region Attributes
        private readonly ICollectionService collectorProfileService;
        #endregion

        #region Properties
        #endregion

        public CollectorProfileController(ICollectionService collectorProfileService)
        {
            this.collectorProfileService = collectorProfileService;
        }

        #region Methods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectorProfileDTO>>> GetCollectorProfiles(
            [FromQuery] QueryCollectorProfileDTO dto)
        {
            var result = await collectorProfileService.GetCollectorProfiles(
                dto);
            return Ok(result);
        }

        [HttpGet("{collectorProfileID:guid}")]
        public async Task<ActionResult<CollectorProfileDetailDTO>> GetCollectorProfileDetail(
            Guid collectorProfileID)
        {
            var result = await collectorProfileService.GetCollectorProfileDetail(
                collectorProfileID);
            return Ok(result);
        }

        [HttpPost("task")]
        public async Task<ActionResult<CollectorProfileDetailDTO>> GetCollectionTasks(
            [FromBody] QueryCollectorTaskDTO dto)
        {
            var claim = CheckClaimHelper.CheckClaim(User);

            var result = await collectorProfileService.GetCollectionTasks(
                claim.userId,
                dto);
            return Ok(result);
        }
        #endregion
    }
}
