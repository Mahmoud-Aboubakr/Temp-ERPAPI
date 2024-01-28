using Application.Dtos.Nationality;
using AutoMapper;
using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;

namespace Application.Mappers.HR.Setup
{
    public class EmployeeMapper : Profile
    {
        public EmployeeMapper()
        {
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<Employee,ReadEmployeeDto>();
        }
    }
}
