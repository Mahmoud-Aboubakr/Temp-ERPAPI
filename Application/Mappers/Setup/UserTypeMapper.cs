using Application.Dtos.UserType;
using AutoMapper;
using Domain.Entities.Setup;

namespace Application.Mappers.Setup
{
    public class UserTypeMapper : Profile
    {
        public UserTypeMapper()
        {
            CreateMap<UserType, ReadUserTypeDto>();
            CreateMap<AddUserTypeDto, UserType>();
            CreateMap<UpdateUserTypeDto, UserType>();
        }
    }
}
