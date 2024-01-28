using Application.Dtos.Cashier.Setup.PaymentModesType;


namespace Application.Dtos.Cashier.Setup.PaymentModes;

public class ReadPaymentModesDto
{
    public int Id { get; set; }
    public string PaymentModeName { get; set; }
    public int PaymentModesTypeId { get; set; }
    public ReadPaymentModesTypeDto PaymentModesType { get; set; }
    public bool IsActive { get; set; }
}
