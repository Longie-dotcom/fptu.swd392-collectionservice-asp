namespace Domain.IRepository
{
    public interface IUnitOfWork
    {
        T GetRepository<T>() where T : IRepositoryBase;

        Task BeginTransactionAsync();
        Task<int> CommitAsync(string? performedBy = null);
        Task RollbackAsync();
    }

    public interface IRepositoryBase
    {

    }
}
