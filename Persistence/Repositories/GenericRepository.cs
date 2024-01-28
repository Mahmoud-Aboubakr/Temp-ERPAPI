using Application.Contracts.Persistence;
using Application.Contracts.Specifications;
using Application.Specifications;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _entity;

        public ApplicationDbContext Context { get; }

        public GenericRepository(ApplicationDbContext context)
        {
            _entity = context.Set<T>();
            Context = context;
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default) => await _entity.FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) => await _entity.ToListAsync();
        public async Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification, CancellationToken cancellationToken = default) => await ApplySpecification(specification).Where(x=>x.IsDeleted == false).ToListAsync();
        public async Task<T> GetAsync(ISpecification<T> specification, CancellationToken cancellationToken = default) => await ApplySpecification(specification).FirstOrDefaultAsync(x=>x.IsDeleted == false);
        public async Task InsertAsync(T entity, CancellationToken cancellationToken = default) => await _entity.AddAsync(entity); 
        public async Task<int> InsertWithIdAsync(T entity, CancellationToken cancellationToken = default) {
            await _entity.AddAsync(entity);
            await  Context.SaveChangesAsync();
            return entity.Id;
        }
        public async Task<T> InsertWithEntityAsync(T entity, CancellationToken cancellationToken = default) {
            await _entity.AddAsync(entity);
            await  Context.SaveChangesAsync();
            return entity;
        }
        public async Task InsertRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default) => await _entity.AddRangeAsync(entities);
        public void Update(T entity) => _entity.Update(entity);
        public void UpdateRange(IEnumerable<T> entities) => _entity.UpdateRange(entities);
        public void Delete(T entity) => _entity.Remove(entity);
        public void DeleteRange(IEnumerable<T> entities) => _entity.RemoveRange(entities);
        public void Activate(T entity)
        {
            entity.IsDeleted = false;
            entity.IsActive = true;
        }
        public Task ActivateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default) => entities.AsQueryable().ForEachAsync(e => e.IsDeleted = false);
        public void Deactivate(T entity) => entity.IsActive = false;
        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
            entity.IsActive = false;
        }
        public Task DeactivateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default) => entities.AsQueryable().ForEachAsync(e => e.IsDeleted = true);
        public Task<bool> ContainsAsync(int id, CancellationToken cancellationToken = default) => _entity.AnyAsync(e => e.Id == id);
        public Task<bool> ContainsAsync(ISpecification<T> specification, CancellationToken cancellationToken = default) => ApplySpecification(specification).AnyAsync();
        public async Task<int> CountAsync( CancellationToken cancellationToken = default) => await _entity.CountAsync(x=>x.IsDeleted == false);
        public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default) => await ApplySpecification(specification).CountAsync(x => x.IsDeleted == false);
        private IQueryable<T> ApplySpecification(ISpecification<T> spec, CancellationToken cancellationToken = default) => SpecificationEvaluator<T>.GetQuery(_entity.AsQueryable().Where(a => a.IsDeleted == false), spec);
        public void Detach(T entity)
        {
            var entry = Context.Entry(entity);
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }
        }
    }
}
