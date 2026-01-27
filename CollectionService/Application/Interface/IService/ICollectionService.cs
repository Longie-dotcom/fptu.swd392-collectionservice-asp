using Application.DTO;

namespace Application.Interface.IService
{
    public interface ICollectionService
    {
        Task<IEnumerable<CollectorProfileDTO>> GetCollectorProfiles(
            QueryCollectorProfileDTO dto);
        
        Task<CollectorProfileDetailDTO> GetCollectorProfileDetail(
            Guid collectorProfileId);

        Task<IEnumerable<CollectionTaskDTO>> GetCollectionTasks(
            Guid callerId,
            QueryCollectorTaskDTO dto);

        Task CreateCollectionTaskAsync(
            SWD392.MessageBroker.CollectionTaskCreateDTO dto);

        Task CreateCollectorProfileAsync(
            SWD392.MessageBroker.CollectorProfileDTO dto);

        Task UserSyncDeleting(
            SWD392.MessageBroker.UserDeleteDTO dto);
    }
}
