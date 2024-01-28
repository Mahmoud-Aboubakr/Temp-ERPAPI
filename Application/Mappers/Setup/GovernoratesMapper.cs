using AutoMapper;
using Domain.Entities.Setup;

namespace Application;

public class GovernoratesMapper : Profile
{
	public GovernoratesMapper()
	{
		CreateMap<CreateGovernorateDto, Governorate>();
		CreateMap<Governorate, ReadGovernorateDto>();
		CreateMap<UpdateGovernorateDto, Governorate>();
	}
}
