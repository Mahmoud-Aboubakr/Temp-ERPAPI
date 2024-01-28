using Application.Dtos.Nationality;
using AutoMapper;
using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;

namespace Application.Mappers.HR.Setup
{
    public class HRFileMapper : Profile
    {
        public HRFileMapper()
        {
            CreateMap<CreateHRFileDto, HRFile>();
            CreateMap<HRFile,ReadHRFileDto>();
        }
    }
}
