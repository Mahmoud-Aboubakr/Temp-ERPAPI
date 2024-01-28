using Application.Dtos.ApplicationPagePrefix;
using AutoMapper;
using Domain.Entities.Setup;

namespace Application.Mappers.Setup
{
    public class ApplicationPagePrefixMapper : Profile
    {
        public ApplicationPagePrefixMapper()
        {
            CreateMap<ApplicationPagePrefix, ReadApplicationPagePrefixDto>();
            CreateMap<AddApplicationPagePrefixDto, ApplicationPagePrefix>();
            CreateMap<UpdateApplicationPagePrefixDto, ApplicationPagePrefix>();
        }
    }
}
