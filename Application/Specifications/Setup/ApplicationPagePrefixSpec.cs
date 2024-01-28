using Domain.Entities.Setup;

namespace Application.Specifications.Setup
{
    public class ApplicationPagePrefixSpec : BaseSpecification<ApplicationPagePrefix>
    {
        public ApplicationPagePrefixSpec(int id):base(x=>x.Id == id)
        {}
        public ApplicationPagePrefixSpec(int pageSize , int pageIndex , bool isPagingEnabled = true,string sort = null)
           : base()
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.PageName);
                        break;
                    default:
                        AddOrederBy(b => b.PageName);
                        break;
                }
            }
        }
    }
}
