using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Setup
{
    public class AppPageSpec: BaseSpecification<AppPage>
    {
        public AppPageSpec()
        {
            AddInclude(x => x.AppModule);
            AddInclude(x => x.PageType);
        }
    }
}
