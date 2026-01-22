using Application.DTO;
using Application.Helper;
using Application.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpDelete("{collectionTaskId:guid}")]
        public async Task<IActionResult> DeleteCollectionTask(Guid collectionTaskId)
        {
            string deletedBy = User.Identity?.Name ?? "Anonymous";  // Sửa typo "Anonymoust"
            var (success, error) = await _collectionTaskService.DeleteCollectionTaskAsync(collectionTaskId, deletedBy);

            if (!success)
            {
                if (error == "Collection Task not found")
                    return NotFound(error);
                else if (error == "Collection Task is already deactivated.")
                    return BadRequest(error);
                return BadRequest(error);  // Sửa: return thay vì gọi BadRequest()
            }

            return Ok("Collection Task deleted successfully");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectionTaskDTO>>> GetAllCollectionTasks([FromQuery] string? sortBy, [FromQuery] QueryCollectionTaskDTO dto)
        {
            var result = await _collectionTaskService.GetAllCollectionTask(sortBy ?? string.Empty, dto);

            if (result.IsSuccess)
            {
                if (result.Data == null || !result.Data.Any())
                {
                    return Ok(GenericResult<IEnumerable<CollectionTaskDTO>>.Success(
                        new List<CollectionTaskDTO>(),
                        "No data found"));  // Sửa "No Data" thành lowercase
                }
                return Ok(result.Data);  // Sửa: return Ok(result.Data) thay vì Ok(result)
            }

            return BadRequest(result.Errors);  // Sửa: dùng result.Errors
        }

        // Thêm action này vào controller
        [HttpGet("{collectionTaskId:guid}")]
        public async Task<ActionResult<CollectionTaskDTO>> GetCollectionTaskById(Guid collectionTaskId)
        {
            var result = await _collectionTaskService.GetCollectionTaskByIdAsync(collectionTaskId);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return NotFound(result.Errors);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCollectionTask([FromBody] CollectionTaskCreateDTO dto)
        {
            var result = await _collectionTaskService.CreateCollectionTaskAsync(dto);

            if (result.IsSuccess)
            {
                return CreatedAtAction(
                    nameof(GetCollectionTaskById),  // ← Thay đổi này
                    new { collectionTaskId = result.Data!.CollectionTaskID },
                    result.Data);
            }

            return BadRequest(result.Errors);
        }


        [HttpPut("{collectionTaskId:guid}")]
        public async Task<IActionResult> UpdateCollectionTask(Guid collectionTaskId, [FromBody] UpdateCollectionTaskDTO dto)
        {
            var result = await _collectionTaskService.UpdateCollectionTaskAsync(collectionTaskId, dto);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            if (result.Errors.Contains("Collection Task not found"))  // Sửa: dùng result.Errors thay vì ErrorMessage
            {
                return NotFound(result);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("{collectionTaskId:guid}/start")]
        public async Task<IActionResult> StartCollectionTask(Guid collectionTaskId)
        {
            var result = await _collectionTaskService.StartCollectionTaskAsync(collectionTaskId);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            if (result.Errors.Any(e => e.Contains("not found")))  // Sửa: dùng result.Errors
            {
                return NotFound(result);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("{collectionTaskId:guid}/complete")]
        public async Task<IActionResult> CompleteCollectionTask(Guid collectionTaskId)
        {
            var result = await _collectionTaskService.CompleteCollectionTaskAsync(collectionTaskId);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            if (result.Errors.Any(e => e.Contains("not found")))  // Sửa: dùng result.Errors
            {
                return NotFound(result);
            }

            return BadRequest(result.Errors);
        }
    }
}
