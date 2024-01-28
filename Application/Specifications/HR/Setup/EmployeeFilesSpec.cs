using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;

namespace Application.Specifications.HR.Setup
{
    public class EmployeeFilesSpec : BaseSpecification<EmployeeFiles>
    {
        public EmployeeFilesSpec(int id):base(x=>x.Id == id)
        {}
        public EmployeeFilesSpec(int pageSize , int pageIndex , bool isPagingEnabled = true,string sort = null)
           : base()
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.ReciveDate);
                        break;
                    default:
                        AddOrederBy(b => b.ReciveDate);
                        break;
                }
            }
        }
    }
}
