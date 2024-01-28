using AutoMapper;
using Domain.Entities.Setup;

namespace Application;

public class BranchMapper : Profile
{
    public BranchMapper()
    {
        CreateMap<CreateBranchDto, Branch>();
        CreateMap<Branch, ReadBranchDto>();
        CreateMap<UpdateBranchDto, Branch>();
    }
}
