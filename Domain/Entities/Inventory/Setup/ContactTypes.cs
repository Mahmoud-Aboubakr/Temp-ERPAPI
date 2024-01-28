using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Inventory.Setup
{
    public class ContactTypes : BaseEntity
    {
        [StringLength(40)]
        public string ArabicName { get; set; }
        [StringLength(40)]
        public string EnglishName { get; set; }

    }
}
