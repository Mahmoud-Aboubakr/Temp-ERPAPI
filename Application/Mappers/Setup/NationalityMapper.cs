using Application.Dtos.Nationality;
using AutoMapper;
using Domain.Entities.Setup;

namespace Application.Mappers.Setup
{
    public class NationalityMapper : Profile
    {
        public NationalityMapper()
        {
            CreateMap<Nationality, ReadNationalityDto>();
            CreateMap<AddNationalityDto,Nationality>();
            CreateMap<UpdateNationalityDto, Nationality>();
        }
    }
}
