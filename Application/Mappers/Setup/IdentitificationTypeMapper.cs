using Application.Dtos.ApplicationPagePrefix;
using Application.Dtos.Setup.IdentitificationTypes.Requests;
using Application.Dtos.Setup.IdentitificationTypes.Responses;
using AutoMapper;
using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.IdentificationType
{
    public class IdentitificationTypeMapper: Profile
    {
        public IdentitificationTypeMapper()
        {
            CreateMap<IdentitificationTypeCreationRequestDto, IdentityType>();
            CreateMap<IdentitificationTypeUpdateRequestDto, IdentityType>();
            CreateMap<IdentityType, IdentitificationTypeResponseDto>().ReverseMap();
        }
    }
}
