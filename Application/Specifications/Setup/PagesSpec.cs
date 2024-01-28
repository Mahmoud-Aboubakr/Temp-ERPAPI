﻿using Domain.Entities.Setup;

namespace Application.Specifications.Setup
{
    public class PagesSpec : BaseSpecification<AppPage>
    {
        public PagesSpec(int id) : base(x => x.Id == id)
        { }
        public PagesSpec(int pageSize, int pageIndex, bool isPagingEnabled = true, string sort = null)
           : base()
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.Sort);
                        break;
                    default:
                        AddOrederBy(b => b.Sort);
                        break;
                }
            }
        }
    }
}
