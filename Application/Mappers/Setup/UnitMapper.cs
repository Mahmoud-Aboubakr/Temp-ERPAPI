using Application.Dtos.Unit;
using AutoMapper;
using Domain.Entities.Setup;

namespace Application.Mappers.Setup
{
    public class UnitMapper : Profile
    {
        public UnitMapper()
        {
            CreateMap<Unit, ReadUnitDto>();
            CreateMap<AddUnitDto, Unit>();
            CreateMap<UpdateUnitDto, Unit>();
        }
    }
}
