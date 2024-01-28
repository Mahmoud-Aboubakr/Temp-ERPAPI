using Domain.Entities.Cashier.Setup;
using Domain.Entities.Supplier.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Cashier.Setup
{
    public class PaymentGroupSpec : BaseSpecification<PaymentGroup>
    {
        public PaymentGroupSpec(int id) : base(x => x.Id == id)
        { }
        public PaymentGroupSpec(int pageSize, int pageIndex,string term,  bool isPagingEnabled = true, string sort = null)
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
