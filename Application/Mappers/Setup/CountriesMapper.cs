using AutoMapper;
using Domain.Entities.Setup;

namespace Application;

public class CountriesMapper : Profile
{
    public CountriesMapper()
    {
        CreateMap<CreateCountryDto, Country>();
        CreateMap<Country, ReadCountryDto>();
        CreateMap<UpdateCountryDto, Country>();
    }
}
