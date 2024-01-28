using System.ComponentModel.DataAnnotations;


namespace Application.Dtos.Cashier.Setup.PaymentModes;

public class CreatePaymentModesDto
{
    [Required]
    public string PaymentModeName { get; set; }
    [Required]
    public int PaymentModesTypeId { get; set; }
    public bool IsActive { get; set; }
}
