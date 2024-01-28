using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;
using System.Linq.Expressions;

namespace Application.Specifications.HR.Setup
{
    public class EmployeeSpec : BaseSpecification<Employee>
    {
        public EmployeeSpec(int id):base(x=>x.Id == id)
        {}
        public EmployeeSpec(int pageSize , int pageIndex , bool isPagingEnabled = true,string sort = null)
           : base()
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.FirstNameEn);
                        break;
                    default:
                        AddOrederBy(b => b.FirstNameEn);
                        break;
                }
            }
        }
    }
}
