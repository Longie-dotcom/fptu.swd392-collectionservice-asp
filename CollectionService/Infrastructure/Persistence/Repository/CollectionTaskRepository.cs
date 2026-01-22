using Domain.Entity;
using Domain.Enum;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class CollectionTaskRepository : GenericRepository<CollectionTask>, ICollectionTaskRepository
    {
        public CollectionTaskRepository(CollectionDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CollectionTask>> PagingCollectionTask(
            string search,
            CollectionReportStatus? status,
            string sortBy,
            int pageSize,
            int pageIndex)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;

            IQueryable<CollectionTask> query = context.CollectionTasks;

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                string keyword = search.Trim().ToLower();

                query = query.Where(x =>
                    (!string.IsNullOrEmpty(x.Note) && x.Note.ToLower().Contains(keyword)) ||
                    (!string.IsNullOrEmpty(x.ImageName) && x.ImageName.ToLower().Contains(keyword)) ||
                    x.AmountEstimated.ToString().Contains(keyword) ||
                    x.Status.ToString().ToLower().Contains(keyword)
                );
            }

            // Filter by status
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            // Sorting
            query = sortBy?.Trim().ToLower() switch
            {
                "assignedat" => query.OrderBy(x => x.AssignedAt),
                "assignedat_desc" => query.OrderByDescending(x => x.AssignedAt),
                "completedat" => query.OrderBy(x => x.CompletedAt),
                "completedat_desc" => query.OrderByDescending(x => x.CompletedAt),
                "amountestimated" => query.OrderBy(x => x.AmountEstimated),
                "amountestimated_desc" => query.OrderByDescending(x => x.AmountEstimated),
                _ => query.OrderBy(x => x.AssignedAt)
            };

            // Paging
            query = query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }
    }
}
