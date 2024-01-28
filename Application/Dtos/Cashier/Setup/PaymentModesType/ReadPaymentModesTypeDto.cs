using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Cashier.Setup.PaymentModesType
{
    public class ReadPaymentModesTypeDto
    {
        public int Id { get; set; }
        public string PaymentName { get; set; }
    }
}
