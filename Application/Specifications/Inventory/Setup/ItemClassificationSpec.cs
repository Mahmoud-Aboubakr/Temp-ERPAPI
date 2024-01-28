using Domain.Entities.HR.Setup;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;

namespace Application.Specifications.Inventory.Setup
{
    public class ItemClassificationSpec : BaseSpecification<ItemClassification>
    {
        public ItemClassificationSpec(int id) : base(x => x.Id == id)
        { }
        public ItemClassificationSpec(int pageSize, int pageIndex, string term, bool isPagingEnabled = true, string sort = null)
           : base(a => a.Name.Contains(term))
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.Code);
                        break;
                    default:
                        AddOrederBy(b => b.Code);
                        break;
                }
            }
        }
    }
}
