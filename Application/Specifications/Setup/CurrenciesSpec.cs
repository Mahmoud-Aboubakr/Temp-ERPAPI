using Domain.Entities.Setup;

namespace Application.Specifications.Setup
{
    public class CurrenciesSpec : BaseSpecification<CurrencySetup>
    {
        public CurrenciesSpec(int id):base(x=>x.Id == id)
        {}
        public CurrenciesSpec(int pageSize , int pageIndex , string term, bool isPagingEnabled = true,string sort = null)
           : base(a => a.ArabicName.Contains(term) || a.EnglishName.Contains(term))
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
