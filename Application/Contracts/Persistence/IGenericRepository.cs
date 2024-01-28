using System.Linq.Expressions;
using Application.Contracts.Specifications;
using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : BaseEntity
	{
		Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
		Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
		Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification, CancellationToken cancellationToken = default(CancellationToken));
		Task<T> GetAsync(ISpecification<T> specification, CancellationToken cancellationToken = default(CancellationToken));
		Task InsertAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));
		Task<int> InsertWithIdAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));
		Task<T> InsertWithEntityAsync(T entity, CancellationToken cancellationToken = default);
        Task InsertRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));
		void Update(T entity);
		void UpdateRange(IEnumerable<T> entities );
		void Delete(T entity);
		void DeleteRange(IEnumerable<T> entities );
		void Activate(T entity  );
		Task ActivateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));
		void Deactivate(T entity );
		void SoftDelete(T entity );
		Task DeactivateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));
		Task<bool> ContainsAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
		Task<bool> ContainsAsync(ISpecification<T> specification, CancellationToken cancellationToken = default(CancellationToken));
		Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken));
		Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default(CancellationToken));
		void Detach(T entity);

    }
}
