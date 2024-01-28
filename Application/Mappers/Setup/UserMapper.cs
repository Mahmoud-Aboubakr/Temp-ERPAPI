using Application.Dtos.User;
using Application.Dtos.UserType;
using AutoMapper;
using Domain.Entities.Setup;

namespace Application.Mappers.Setup;

public class UserMapper : Profile
{
	public UserMapper()
	{
		CreateMap<CreateUserDto, User>();
		CreateMap<User, ReadUserDto>();
		CreateMap<UpdateUserDto, User>();
	}
}