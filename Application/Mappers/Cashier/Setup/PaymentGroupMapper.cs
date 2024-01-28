using Application.Dtos.Cashier.Setup.PaymentGroup;
using AutoMapper;
using Domain.Entities.Cashier.Setup;

namespace Application.Mappers.Cashier.Setup
{
    public class PaymentGroupMapper : Profile
    {
        public PaymentGroupMapper()
        {
            CreateMap<PaymentGroup, ReadPaymentGroupDto>();
            CreateMap<CreatePaymentGroupDto, PaymentGroup>();
            CreateMap<UpdatePaymentGroupDto, PaymentGroup>();
        }
    }
}
