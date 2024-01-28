using Application.Dtos.News;
using AutoMapper;
using Domain.Entities.Setup;

namespace Application.Mappers.Setup
{
    public class NewsMapper : Profile
    {
        public NewsMapper()
        {
            CreateMap<New, ReadNewsDto>();
            CreateMap<AddNewsDto, New>();
            CreateMap<UpdateNewsDto, New>();
        }
    }
}
