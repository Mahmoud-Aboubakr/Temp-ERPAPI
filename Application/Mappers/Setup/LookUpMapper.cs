using Application.Dtos.Setup.LookUps;
using AutoMapper;
using Domain.Entities.LookUps;

namespace Application.Mappers.Setup
{
    public class LookUpMapper : Profile
    {
        public LookUpMapper()
        {
            CreateMap<ApplicationTbl, ApplicationTblDto>();
            CreateMap<AppModule, AppModuleDto>();
        }
    }
}
