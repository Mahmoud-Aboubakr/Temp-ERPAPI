using Domain.Entities.Cashier.Setup;
using Domain.Entities.Supplier.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Cashier.Setup
{
    public class PaymentModesTypesSpec : BaseSpecification<PaymentModesType>
    {
        public PaymentModesTypesSpec(int id) : base(x => x.Id == id)
        { }
        public PaymentModesTypesSpec(int pageSize, int pageIndex, bool isPagingEnabled = true, string sort = null)
           : base()
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.PaymentName);
                        break;
                    default:
                        AddOrederBy(b => b.PaymentName);
                        break;
                }
            }
        }
    }
}
