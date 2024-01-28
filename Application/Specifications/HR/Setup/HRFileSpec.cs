using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;

namespace Application.Specifications.Setup
{
    public class HRFileSpec : BaseSpecification<HRFile>
    {
        public HRFileSpec(int id):base(x=>x.Id == id)
        {}
        public HRFileSpec(int pageSize , int pageIndex , bool isPagingEnabled = true,string sort = null)
           : base()
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.Id);
                        break;
                    default:
                        AddOrederBy(b => b.Id);
                        break;
                }
            }
        }
    }
}
