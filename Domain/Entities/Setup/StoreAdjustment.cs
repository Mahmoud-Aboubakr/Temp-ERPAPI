using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class StoreAdjustment:BaseEntity
    {
        [Column(TypeName = "nvarchar(80)")]
        public string Description { get; set; }
    }
}
