using Domain.Aggregate;

namespace Domain.IRepository
{
    public interface ICollectorProfileRepository :
        IGenericRepository<CollectorProfile>,
        IRepositoryBase
    {
        Task<IEnumerable<CollectorProfile>> QueryCollectorProfiles(
            int pageIndex, 
            int pageLength, 
            string? search, 
            bool? isActive);

        Task<CollectorProfile?> GetCollectorProfileDetailById(
            Guid collectorProfileId);

        Task<CollectorProfile?> GetCollectorProfileByContactInfo(
            string contactInfo);

        Task<CollectorProfile?> GetCollectorProfileByUserId(
            Guid userId);
    }
}
