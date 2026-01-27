using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class CollectionTaskRepository :
        GenericRepository<CollectionTask>,
        ICollectionTaskRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion
        public CollectionTaskRepository(CollectionDBContext context) : base(context)
        {
        }

        #region Methods
        public async Task<IEnumerable<CollectionTask>> GetCollectionTasksByCollectorIdAsync(Guid collectorProfileId, string? sortBy, int PageIndex, int PageLength, DateTime? AssignedAt, DateTime? StartAt, CollectionReportStatus? Status)
        {
            IQueryable<CollectionTask> query = context.CollectionTasks
                .Where(x => x.CollectorProfileID == collectorProfileId).AsNoTracking();

            // Filter: AssignedAt
            if (AssignedAt.HasValue)
            {
                query = query.Where(x => x.AssignedAt.Date == AssignedAt.Value.Date);
            }

            // Filter: StartAt
            if (StartAt.HasValue)
            {
                query = query.Where(x => x.StartedAt.Date == StartAt.Value.Date);
            }

            // Filter: Status
            if (Status.HasValue)
            {
                query = query.Where(x => x.Status == Status.Value);
            }

            // Sorting
            query = sortBy switch
            {
                "AssignedAt" => query.OrderBy(x => x.AssignedAt),
                _ => query.OrderBy(x => x.StartedAt),
            };

            // Paging
            query = query
                .Skip(PageIndex * PageLength)
                .Take(PageLength);

            return await query.ToListAsync();
        }

            #endregion


        }
    }
