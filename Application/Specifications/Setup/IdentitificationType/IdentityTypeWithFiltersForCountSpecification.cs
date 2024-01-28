using Domain.Entities.HR.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Setup.IdentitificationType
{
    public class IdentityTypeWithFiltersForCountSpecification : BaseSpecification<IdentityType>
    {
        public IdentityTypeWithFiltersForCountSpecification(IdentitificationTypeSpecParams specParams)
            : base(x => string.IsNullOrEmpty(specParams.Search) || x.NameAr.Contains(specParams.Search) ||
            x.NameEn.Contains(specParams.Search))
        {
            AddOrederBy(x => x.NameAr);
            ApplyPanging((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }
    }
}
