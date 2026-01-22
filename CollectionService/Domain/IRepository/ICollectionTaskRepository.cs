using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;

namespace Domain.IRepository
{
    public interface ICollectionTaskRepository : IGenericRepository<CollectionTask>, IRepositoryBase
    {
        Task<IEnumerable<CollectionTask>> PagingCollectionTask(
            string search,
            CollectionReportStatus? status,
            string sortBy,
            int pageSize,
            int pageIndex);
    }
}
