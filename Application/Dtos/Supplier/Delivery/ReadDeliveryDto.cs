using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Supplier.Delivery
{
    public class ReadDeliveryDto
    { 
            public int Id { get; set; }
            public string DeliveryTerm { get; set; }
 
    }
}
