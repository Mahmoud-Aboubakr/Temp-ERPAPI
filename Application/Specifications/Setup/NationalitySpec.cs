using Domain.Entities.Setup;

namespace Application.Specifications.Setup
{
    public class NationalitySpec : BaseSpecification<Nationality>
    {
        public NationalitySpec(int id):base(x=>x.Id == id)
        {}
        public NationalitySpec(int pageSize , int pageIndex , bool isPagingEnabled = true,string sort = null)
           : base()
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.CountryCode);
                        break;
                    default:
                        AddOrederBy(b => b.CountryCode);
                        break;
                }
            }
        }
    }
}
