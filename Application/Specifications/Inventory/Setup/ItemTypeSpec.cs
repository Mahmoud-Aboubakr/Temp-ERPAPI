using Domain.Entities.HR.Setup;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;

namespace Application.Specifications.Inventory.Setup
{
    public class ItemTypeSpec : BaseSpecification<ItemType>
    {
        public ItemTypeSpec(int id) : base(x => x.Id == id)
        { }
        public ItemTypeSpec(int pageSize, int pageIndex, string term, bool isPagingEnabled = true, string sort = null)
           : base(a => a.TypeName.Contains(term))
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);


            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.TypeName);
                        break;
                    default:
                        AddOrederBy(b => b.TypeName);
                        break;
                }
            }

        }
    }
}
