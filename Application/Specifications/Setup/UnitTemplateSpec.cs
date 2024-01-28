using Domain.Entities.Setup;

namespace Application.Specifications.Setup
{
    public class UnitTemplateSpec : BaseSpecification<UnitTemplate>
    {
        public UnitTemplateSpec(int id) : base(x => x.Id == id)
        { }
        public UnitTemplateSpec(int pageSize, int pageIndex, bool isPagingEnabled = true, string sort = null)
           : base()
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.UnitTemplateCode);
                        break;
                    default:
                        AddOrederBy(b => b.UnitTemplateCode);
                        break;
                }
            }
        }
    }
}
