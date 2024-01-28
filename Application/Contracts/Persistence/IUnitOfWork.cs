using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : BaseEntity;
        Task<int> Commit();
    }
}
