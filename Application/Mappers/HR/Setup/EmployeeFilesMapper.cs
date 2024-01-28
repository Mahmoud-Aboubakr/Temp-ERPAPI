using Application.Dtos.Nationality;
using AutoMapper;
using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;

namespace Application.Mappers.HR.Setup
{
    public class EmployeeFilesMapper : Profile
    {
        public EmployeeFilesMapper()
        {
            CreateMap<CreateEmployeeFilesDto, EmployeeFiles>();
            CreateMap<EmployeeFiles,ReadEmployeeFilesDto>();
        }
    }
}
