
using System.ComponentModel.DataAnnotations;


namespace Application.Dtos.Cashier.Setup.PaymentModes;

public class UpdatePaymentModesDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string PaymentModeName { get; set; }
    [Required]
    public int PaymentModesTypeId { get; set; }
    public bool IsActive { get; set; }
}
