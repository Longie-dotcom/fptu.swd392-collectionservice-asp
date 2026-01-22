using Domain.Aggregate;
using Domain.Entity;
using Domain.IRepository;
using Infrastructure.InfrastructureException;
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
        public IQueryable<CollectorProfile> GetAllCollectorProfiles()
        {
            return context.CollectorProfiles
                          .Include(cp => cp.CollectionTasks).AsNoTracking();
        }
        #endregion
    }
}
