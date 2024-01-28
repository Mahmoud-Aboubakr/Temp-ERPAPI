using Domain.Entities.HR.Setup;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;

namespace Application.Specifications.Inventory.Setup
{
    public class StoreSpec : BaseSpecification<Store>
    {
        public StoreSpec(int id) : base(x => x.Id == id)
        { }
        public StoreSpec(int pageSize, int pageIndex, string term, bool isPagingEnabled = true, string sort = null)
           : base(a => a.StoreNameAr.Contains(term) ||
                       a.StoreNameEn.Contains(term))
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.StoreNameEn);
                        break;
                    default:
                        AddOrederBy(b => b.StoreNameEn);
                        break;
                }
            }
        }
    }
}
