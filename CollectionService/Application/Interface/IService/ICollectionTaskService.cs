using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Helper;

namespace Application.Interface.IService
{
    public interface ICollectionTaskService
    {
        Task<GenericResult<IEnumerable<CollectionTaskDTO>>> GetAllCollectionTask(string sortBy, QueryCollectionTaskDTO dto);
        Task<(bool Success, string? ErrorMessage)> DeleteCollectionTaskAsync(Guid collectionTaskId, string deletedBy);
        Task<GenericResult<CollectionTaskDTO>> GetCollectionTaskByIdAsync(Guid collectionTaskId);
        Task<GenericResult<CollectionTaskDTO>> CreateCollectionTaskAsync(CollectionTaskCreateDTO dto);
        Task<GenericResult<CollectionTaskDTO>> UpdateCollectionTaskAsync(Guid collectionTaskId, UpdateCollectionTaskDTO dto);
        Task<GenericResult<CollectionTaskDTO>> StartCollectionTaskAsync(Guid collectionTaskId);
        Task<GenericResult<CollectionTaskDTO>> CompleteCollectionTaskAsync(Guid collectionTaskId);
        Task<GenericResult<CollectionTaskDTO>> FailCollectionTaskAsync(Guid collectionTaskId);
    }
}
