using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Setup.StoreAdjustment
{
    public class ReadStoreAdjustmentDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
