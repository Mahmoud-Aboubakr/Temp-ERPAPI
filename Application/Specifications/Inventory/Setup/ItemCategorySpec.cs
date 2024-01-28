using Domain.Entities.HR.Setup;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;

namespace Application.Specifications.Inventory.Setup
{
    public class ItemCategorySpec : BaseSpecification<ItemCategory>
    {
        public ItemCategorySpec(int id) : base(x => x.Id == id)
        { }
        public ItemCategorySpec(int pageSize, int pageIndex, string term, bool isPagingEnabled = true, string sort = null)
           : base(a => a.Name.Contains(term))
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.Name);
                        break;
                    default:
                        AddOrederBy(b => b.Name);
                        break;
                }
            }
        }
    }
}
