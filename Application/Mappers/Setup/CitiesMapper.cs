using AutoMapper;
using Domain.Entities.Setup;

namespace Application;

public class CitiesMapper : Profile
{
    public CitiesMapper()
    {
        CreateMap<CreateCityDto, City>();
        CreateMap<City, ReadCityDto>();
        CreateMap<UpdateCityDto, City>();
    }
}
