using Application.Dtos.Setup.UnitTemplate;
using Application.Dtos.UnitTemplate;
using AutoMapper;
using Domain.Entities.Setup;

namespace Application.Mappers.Setup
{
    public class UnitTemplateMapper : Profile
    {
        public UnitTemplateMapper()
        {
            CreateMap<UnitTemplate, ReadUnitTemplateDto>();
            CreateMap<AddUnitTemplateDto, UnitTemplate>();
            CreateMap<UpdateUnitTemplateDto, UnitTemplate>();
        }
    }
}
