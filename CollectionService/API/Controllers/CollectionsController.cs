using API.Helper;
using Application.DTO;
using Application.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollectionsController : ControllerBase
    {
        #region Attributes
        private readonly ICollectionService collectionService;
        #endregion

        #region Properties
        #endregion

        public CollectionsController(ICollectionService collectionService)
        {
            this.collectionService = collectionService;
        }

        #region Methods
        [AuthorizePrivilege("ViewCollectorProfile")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectorProfileDTO>>> GetCollectorProfiles(
            [FromQuery] QueryCollectorProfileDTO dto)
        {
            var claim = CheckClaimHelper.CheckClaim(User);
            var result = await collectionService.GetCollectorProfiles(
                dto);
            return Ok(result);
        }

        [AuthorizePrivilege("ViewCollectorProfile")]
        [HttpGet("{collectorProfileId:guid}")]
        public async Task<ActionResult<CollectorProfileDetailDTO>> GetCollectorProfileDetail(
            Guid collectorProfileId)
        {
            var claim = CheckClaimHelper.CheckClaim(User);
            var result = await collectionService.GetCollectorProfileDetail(
                collectorProfileId);
            return Ok(result);
        }

        // ============================================================

        [AuthorizePrivilege("ViewMyCollectionTask")]
        [HttpGet("my-collection-task")]
        public async Task<ActionResult<CollectorProfileDetailDTO>> GetMyCollectionTasks(
            [FromQuery] QueryMyCollectionTaskDTO dto)
        {
            var claim = CheckClaimHelper.CheckClaim(User);
            var result = await collectionService.GetMyCollectionTasks(
                claim.userId,
                dto);
            return Ok(result);
        }

        [AuthorizePrivilege("SubmitProof")]
        [HttpPost("submit-proof")]
        public async Task<ActionResult<CollectorProfileDetailDTO>> SubmitProof(
            [FromBody] SubmitProofDTO dto)
        {
            var claim = CheckClaimHelper.CheckClaim(User);
            var result = await collectionService.SubmitProof(
                claim.userId,
                dto);
            return Ok(result);
        }
        // ============================================================
        #endregion
    }
}
