using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Supplier.Delivery
{
    public class CreationDeliveryDto
    {
        [StringLength(40, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string DeliveryTerm { get; set; }
    }
}
