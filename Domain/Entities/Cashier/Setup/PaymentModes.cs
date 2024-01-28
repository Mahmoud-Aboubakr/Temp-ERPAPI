using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Cashier.Setup
{
    public class PaymentModes : BaseEntity
    {
        public string PaymentModeName { get; set; }
        public int PaymentModesTypeId  { get; set; }
        public PaymentModesType PaymentModesType { get; set; }

    }
}
