using Application.Dtos.Cashier.Setup.PaymentModes;
using AutoMapper;
using Domain.Entities.Cashier.Setup;

namespace Application.Mappers.Cashier.Setup
{
    public class PaymentModesMapper : Profile
    {
        public PaymentModesMapper()
        {
            CreateMap<PaymentModes, ReadPaymentModesDto>();
            CreateMap<CreatePaymentModesDto, PaymentModes>();
            CreateMap<UpdatePaymentModesDto, PaymentModes>();
        }
    }
}
