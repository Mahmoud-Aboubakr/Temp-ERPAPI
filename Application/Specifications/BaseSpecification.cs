using Application.Contracts.Specifications;
using System.Linq.Expressions;

namespace Application.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        { }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        protected void AddOrederBy(Expression<Func<T, object>> orderByExpression)
           => OrderBy = orderByExpression;

        protected void AddOrederByDescending(Expression<Func<T, object>> orderByDescExpression)
            => OrderByDescending = orderByDescExpression;

        protected void ApplyPanging(int skip, int take, bool isPagingEnabled = true)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = isPagingEnabled;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
