using Domain.Entities.Supplier.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Supplier.Setup
{
    public class SuppliersSpec :BaseSpecification<SupplierType>
    {
        public SuppliersSpec(int id) : base(x => x.Id == id)
        { }
        public SuppliersSpec(int pageSize, int pageIndex, string term, bool isPagingEnabled = true, string sort = null)
           : base(a => a.SupplierName.Contains(term))
        {
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "Desc":
                        AddOrederByDescending(b => b.SupplierName);
                        break;
                    default:
                        AddOrederBy(b => b.SupplierName);
                        break;
                }
            }
        }
    }
}
