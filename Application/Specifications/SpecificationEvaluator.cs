using Application.Contracts.Specifications;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Specifications
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
                                                   ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);

            if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);

            if (spec.IsPagingEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.IncludeStrings.Count > 0)
                query = spec.IncludeStrings.Aggregate(query, (current, include) =>
                        current.Include(include));
            else if(spec.Includes.Count > 0)
                query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }

    }
}
