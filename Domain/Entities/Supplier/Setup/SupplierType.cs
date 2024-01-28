using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Supplier.Setup
{
    public class SupplierType : BaseEntity
    {
        [StringLength(45)]
        public string SupplierName { get; set; }
    }
}
