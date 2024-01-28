using Application.Contracts.Persistence;
using Domain.Entities;
using Persistence.Data;
using Persistence.Repositories;

namespace Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, object>();
        }

        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity
        {
            if (_repositories.TryGetValue(typeof(T), out var repository))
            {
                return (IGenericRepository<T>)repository;
            }

            var newRepository = new GenericRepository<T>(_dbContext);
            _repositories.Add(typeof(T), newRepository);
            return newRepository;
        }

        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
