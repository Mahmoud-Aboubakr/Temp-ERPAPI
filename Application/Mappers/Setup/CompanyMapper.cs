using AutoMapper;
using Domain.Entities.Setup;

namespace Application;

public class CompanyMapper : Profile
{
    public CompanyMapper()
    {
        CreateMap<CreateCompanyDto, Company>();
        CreateMap<Company, ReadCompanyDto>();
        CreateMap<UpdateCompanyDto, Company>();
    }
}
