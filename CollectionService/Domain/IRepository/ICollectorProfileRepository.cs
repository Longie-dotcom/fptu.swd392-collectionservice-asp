using Domain.Aggregate;
namespace Domain.IRepository
{
    public interface ICollectorProfileRepository :
        IGenericRepository<CollectorProfile>,
        IRepositoryBase
    {

    }
}
