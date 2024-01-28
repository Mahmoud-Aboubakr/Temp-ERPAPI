using Domain.Entities.Inventory;
using Domain.Entities.Inventory.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Inventory.Setup
{
    public class ContactsSpec : BaseSpecification<ContactTypes>
    {
        public ContactsSpec(int id) : base(x => x.Id == id)
        { }
        public ContactsSpec(int pageSize, int pageIndex, string term, bool isPagingEnabled = true, string sort = null)
           : base(a => a.EnglishName.Contains(term) || a.ArabicName.Contains(term))
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
