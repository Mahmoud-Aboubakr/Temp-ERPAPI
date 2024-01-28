
using Application.Dtos.Setup.IdentitificationTypes.Requests;
using Application.Dtos.Setup.IdentitificationTypes.Responses;
using Application.Models;
using Domain.Entities.HR.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.IServices
{
    public interface IIdentitificationTypeService
    {
        Task<IdentityType> AddIdentityTypeAsync(IdentitificationTypeCreationRequestDto request );
        Task<IdentityType> UpdateIdentityTypeAsync(IdentitificationTypeUpdateRequestDto request);
        Task<IdentitificationTypeResponseDto> GetIdentityTypeByIdAsync(int id);
        Task DeleteIdentityTypeByIdAsync(int id);
        Task<IReadOnlyList<IdentitificationTypeResponseDto>> GetAllIdentityTypesAsync( );
    }
}
