using Application.Dtos.Cashier.Setup.PaymentModesType;
using AutoMapper;
using Domain.Entities.Cashier.Setup;


namespace Application.Mappers.Cashier.Setup
{
    public class PaymentModesTypeMapper :Profile
    {
        public PaymentModesTypeMapper()
        {
            CreateMap<CreationPaymentModesTypeDto, PaymentModesType>();
            CreateMap<UpdatePaymentModesTypeDto, PaymentModesType>();
            CreateMap<PaymentModesType, ReadPaymentModesTypeDto>();
        }
    }
}
