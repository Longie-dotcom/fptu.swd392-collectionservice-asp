using Domain.Entity;
using Domain.Enum;

namespace Domain.IRepository
{
    public interface ICollectionTaskRepository:
        IGenericRepository<CollectionTask>,
        IRepositoryBase
    {
        Task<IEnumerable<CollectionTask>> GetCollectionTasksByCollectorIdAsync(
            Guid collectorProfileId,
            string? sortBy,
            int pageIndex,
            int pageLength,
            DateTime? assignedAt,
            DateTime? startAt,
            CollectionReportStatus? status);
    }
}
