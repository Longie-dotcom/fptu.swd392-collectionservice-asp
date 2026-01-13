using Application.DTO;
using Application.Helper;
using Application.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.Metrics;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CollectionTasksController : ControllerBase
    {
        private readonly ICollectionTaskService _collectionTaskService;

        public CollectionTasksController(ICollectionTaskService service)
        {
            _collectionTaskService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectionTaskDTO>>> GetAllInstrument([FromQuery] string? sortBy, [FromQuery] QueryCollectionTaskDTO dto)
        {
            var result = await _collectionTaskService.GetAllCollectionTask(sortBy ?? string.Empty, dto);
            if (result.IsSuccess)
            {
                if(result.Data == null || !result.Data.Any())
                {
                    return Ok(GenericResult<IEnumerable<CollectionTaskDTO>>.Success(
                        new List<CollectionTaskDTO>(),
                        "No Data"));
                }

                return Ok(result);
            }

            return BadRequest(result);
        } 
    }
}
