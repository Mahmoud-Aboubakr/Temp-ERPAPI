using Domain.Entities.Supplier.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Supplier.Setup
{
    public class DeliveriesSpec : BaseSpecification<Delivery>
    {
        public DeliveriesSpec(int id) : base(x => x.Id == id)
        { }
        public DeliveriesSpec(int pageSize, int pageIndex, string term, bool isPagingEnabled = true, string sort = null)
           : base(a => a.DeliveryTerm.Contains(term))
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.DeliveryTerm);
                        break;
                    default:
                        AddOrederBy(b => b.DeliveryTerm);
                        break;
                }
            }
        }
    }
}

