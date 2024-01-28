using Domain.Entities.Setup;

namespace Application.Specifications.Setup
{
    public class UserTypeSpec : BaseSpecification<UserType>
    {
        public UserTypeSpec(int id):base(x=>x.Id == id)
        {}
        public UserTypeSpec(int pageSize , int pageIndex , bool isPagingEnabled = true,string sort = null)
           : base()
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.DescNameEn);
                        break;
                    default:
                        AddOrederBy(b => b.DescNameEn);
                        break;
                }
            }
        }
    }
}
