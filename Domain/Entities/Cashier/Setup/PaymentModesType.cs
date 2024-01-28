using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Cashier.Setup
{
    public class PaymentModesType :BaseEntity
    {
        public string PaymentName { get; set; }
        public List<PaymentModes> PaymentModes{ get; set; }
    }
}
