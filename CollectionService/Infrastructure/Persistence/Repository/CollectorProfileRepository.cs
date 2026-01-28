using Domain.Aggregate;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class CollectorProfileRepository :
        GenericRepository<CollectorProfile>,
        ICollectorProfileRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public CollectorProfileRepository(CollectionDBContext context) : base(context) { }

        #region Methods
        public async Task<IEnumerable<CollectorProfile>> QueryCollectorProfiles(
            int pageIndex,
            int pageLength,
            string? search,
            bool? isActive)
        {
            IQueryable<CollectorProfile> query = context.CollectorProfiles
                .AsNoTracking();

            // Filter: IsActive
            if (isActive.HasValue)
            {
                query = query.Where(x => x.IsActive == isActive.Value);
            }

            // Search (adjust field if needed)
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.ContactInfo.Contains(search));
            }

            // Sorting
            query.OrderBy(x => x.ContactInfo);

            // Paging
            query = query
                .Skip(pageIndex * pageLength)
                .Take(pageLength);

            return await query.ToListAsync();
        }

        public async Task<CollectorProfile?> GetCollectorProfileDetailById(
            Guid collectorProfileId)
        {
            return await context.CollectorProfiles
                .Include(x => x.CollectionTasks)
                .FirstOrDefaultAsync(x => x.CollectorProfileID == collectorProfileId);
        }

        public async Task<CollectorProfile?> GetCollectorProfileByContactInfo(
            string contactInfo)
        {
            return await context.CollectorProfiles
                .Include(x => x.CollectionTasks)
                .FirstOrDefaultAsync(x => x.ContactInfo == contactInfo);
        }

        public async Task<CollectorProfile?> GetCollectorProfileByUserId(
            Guid userId)
        {
            return await context.CollectorProfiles
                .Include(x => x.CollectionTasks)
                .FirstOrDefaultAsync(x => x.UserID == userId);
        }
        #endregion
    }
}
