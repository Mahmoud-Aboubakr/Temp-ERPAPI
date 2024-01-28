using Domain.Entities.Setup;

namespace Application.Specifications.Setup
{
    public class UnitSpec : BaseSpecification<Unit>
    {
        public UnitSpec(int id) : base(x => x.Id == id)
        { }
        public UnitSpec(int pageSize, int pageIndex, bool isPagingEnabled = true, string sort = null)
           : base()
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.UnitCode);
                        break;
                    default:
                        AddOrederBy(b => b.UnitCode);
                        break;
                }
            }
        }
    }
}
