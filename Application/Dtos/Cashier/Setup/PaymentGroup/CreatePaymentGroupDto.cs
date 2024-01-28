using Domain.Entities.Inventory;
using Domain.Entities.Setup;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Cashier.Setup.PaymentGroup;

public class CreatePaymentGroupDto
{
    [Required]
    public string Name { get; set; }
}
