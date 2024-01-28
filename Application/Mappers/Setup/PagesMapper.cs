using Application.Dtos.Page;
using AutoMapper;
using Domain.Entities.Setup;

namespace Application.Mappers.Setup
{
    public class PagesMapper : Profile
    {
        public PagesMapper()
        {
            CreateMap<AppPage, ReadAppPageDto>();
            CreateMap<AddAppPageDto, AppPage>();
            CreateMap<UpdateAppPageDto, AppPage>();
        }
    }
}
