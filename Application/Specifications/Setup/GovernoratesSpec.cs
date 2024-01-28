using Domain.Entities.Setup;

namespace Application.Specifications.Setup
{
    public class GovernoratesSpec : BaseSpecification<Governorate>
    {
        public GovernoratesSpec(int id):base(x=>x.Id == id)
        {}
        public GovernoratesSpec(int pageSize , int pageIndex , bool isPagingEnabled = true,string sort = null)
           : base()
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.EnglishName);
                        break;
                    default:
                        AddOrederBy(b => b.EnglishName);
                        break;
                }
            }
        }
    }
}
