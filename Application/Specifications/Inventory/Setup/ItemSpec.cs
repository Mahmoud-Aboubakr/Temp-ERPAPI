using Domain.Entities.HR.Setup;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;

namespace Application.Specifications.Inventory.Setup
{
    public class ItemSpec : BaseSpecification<Item>
    {
        public ItemSpec(int id) : base(x => x.Id == id)
        { }
        public ItemSpec(int pageSize, int pageIndex, string term, bool isPagingEnabled = true, string sort = null)
           : base(a => a.ServiceName.Contains(term) ||
                       a.EnglishName.Contains(term) ||
                       a.ArabicName.Contains(term))
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.ServiceName);
                        break;
                    default:
                        AddOrederBy(b => b.ServiceName);
                        break;
                }
            }
        }
    }
}
