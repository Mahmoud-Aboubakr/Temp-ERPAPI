﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Cashier.Setup.PaymentModesType
{
    public class CreationPaymentModesTypeDto
    {
        [StringLength(40, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string PaymentName { get; set; }
    }
}
