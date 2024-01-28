using AutoMapper;
using Domain.Entities.Setup;

namespace Application;

public class CurrencyMapper : Profile
{
    public CurrencyMapper()
    {
        CreateMap<CreateCurrencyDto, CurrencySetup>();
        CreateMap<CurrencySetup, ReadCurrencyDto>();
        CreateMap<UpdateCurrencyDto, CurrencySetup>();
    }
}
