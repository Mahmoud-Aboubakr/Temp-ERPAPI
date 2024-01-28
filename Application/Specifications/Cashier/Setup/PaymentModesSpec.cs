using Domain.Entities.Cashier.Setup;

namespace Application.Specifications.Cashier.Setup
{
    public class PaymentModesSpec : BaseSpecification<PaymentModes>
    {
        public PaymentModesSpec(int id) : base(x => x.Id == id)
        { }
        public PaymentModesSpec(int pageSize, int pageIndex,string term,  bool isPagingEnabled = true, string sort = null)
           : base(a => a.PaymentModeName.Contains(term))
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.PaymentModeName);
                        break;
                    default:
                        AddOrederBy(b => b.PaymentModeName);
                        break;
                }
            }
        }
    }
}
