using Application.Dtos.Authentication.Requests;
using Application.Dtos.Authentication.Responses;
using Application.Dtos.General.Responses;
using Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto,
            CancellationTokenSource cts);
        Task<List<EnumAsListResponseDto>> GetAllLanguagesAsync(string lang);
        Task<List<EnumAsListResponseDto>> GetAllStatusesAsync(string lang);
        Task<List<EnumAsListResponseDto>> GetAllGendersAsync(string lang);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckUserNameExistsAsync(string userName);
        Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request );
        Task ChangeAccountPasswordAsync(ChangePasswordRequestDto request, ClaimsPrincipal claimsPrincipal);
        Task<IReadOnlyList<AppUserResponseDto>> GetAllUsersAsync();

        Task<ChangePasswordReasonResponseDto> AddChangePasswordReasonAsync(ChangePasswordReasonCreationRequestDto request);
        Task<ChangePasswordReasonResponseDto> UpdateChangePasswordReasonAsync(ChangePasswordReasonUpdateRequestDto request);
        Task<ChangePasswordReasonResponseDto> GetChangePasswordReasonByIdAsync(Guid reasonId);
        Task DeleteChangePasswordReasonByIdAsync(Guid reasonId);
        Task<IReadOnlyList<ChangePasswordReasonResponseDto>> GetAllChangePasswordReasonsByIdAsync();

        Task<AppUserRoleResponseDto> CreateUserRoleAsync(AppUserRoleCreationRequestDto request);
        Task<AppUserRoleResponseDto> UpdateUserRoleAsync(AppUserRoleUpdateRequestDto request);
        Task<AppUserRoleResponseDto> GetUserRoleByIdAsync(string roleId);
        Task<(IReadOnlyList<AppUserRoleResponseDto>, int)> GetUsersRolesAsync(int pageIndex,
            int pageSize);
        Task DeleteUserRoleAsync(string roleId);
        Task AddRolePrivilegeAsync(RolePrivilegeRequestDto request);
        Task<AppUserRoleWithPrivilegesResponseDto> GetRoleWithPrivilegesAsync(string roleId);
        Task<RolePrivilegeResponseDto> GetModulesWithPagesAsync();
    }
}
