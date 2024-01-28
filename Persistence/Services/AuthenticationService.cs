using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Identity;
using Application.Dtos.Authentication.Requests;
using Application.Dtos.Authentication.Responses;
using Application.Dtos.General.Responses;
using Application.Models.Authentication;
using Application.Specifications.Setup;
using Domain.Entities.Identity;
using Domain.Entities.Setup;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppUserRole> _roleManager;
        private readonly IChangePasswordReasonRepository _changePasswordReasonRepository;
        private readonly IAppUserChangePasswordReasonRepository _appUserChangePasswordReasonRepository;
        private readonly IAppUserRoleRepository _appUserRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRolePrivilegeRepository _rolePrivilegeRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppUserRole> roleManager,
            IChangePasswordReasonRepository changePasswordReasonRepository,
            IAppUserChangePasswordReasonRepository appUserChangePasswordReasonRepository,
            IOptions<JwtSettings> jwtSettings,
            IAppUserRoleRepository appUserRoleRepository,
            IUnitOfWork unitOfWork,
            IRolePrivilegeRepository rolePrivilegeRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _changePasswordReasonRepository = changePasswordReasonRepository;
            _appUserChangePasswordReasonRepository = appUserChangePasswordReasonRepository;
            _appUserRoleRepository = appUserRoleRepository;
            _unitOfWork = unitOfWork;
            _rolePrivilegeRepository = rolePrivilegeRepository;
            _jwtSettings = jwtSettings.Value;
        }

        #region AppUser
        public async Task<RegistrationResponseDto> RegisterAsync(RegistrationRequestDto request,
            CancellationTokenSource cts)
        {
            if (!await CheckEmailExistsAsync(request.Email) || !await CheckUserNameExistsAsync(request.UserName) ||
                !await CheckPhoneNumberExistsAsync(request.PhoneNumber))
                throw new Exception($"email '{request.Email}', user name '{request.UserName}'" +
                    $" or phone number '{request.PhoneNumber}' is registered");

            var appUser = new AppUser
            {
                UserName = request.UserName,
                EmployeeId = request.EmployeeId,
                FullNameAR = request.FullNameAR,
                FullNameEn = request.FullNameEn,
                Gender = (Gender)Enum.Parse(typeof(Gender), request.Gender),
                BirthDate = request.BirthDate,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                NationalityId = request.NationalityId,
                DepartmentId = request.DepartmentId,
                UserTypeId = request.UserTypeId,
                Status = request.Status is not null ? (UserStatus)Enum.Parse(typeof(UserStatus), request.Status) : null,
                DefaultModule = request.DefaultModule,
                Language = request.Language is not null ? (Language)Enum.Parse(typeof(Language), request.Language) : null,
                HasDiscount = request.HasDiscount,
                DiscountPercentage = request.DiscountPercentage,
                DiscountLimit = request.DiscountLimit
            };

            var creationIdentityResult = await _userManager.CreateAsync(appUser);
            if (!creationIdentityResult.Succeeded)
                throw new Exception($"user '{request.Email}' not saved because {creationIdentityResult.Errors}");

            var passwordIdentityResult = await _userManager.AddPasswordAsync(appUser, request.Password);
            if (!passwordIdentityResult.Succeeded)
                throw new Exception($"password not saved for user '{request.Email}'");

            var roles = _roleManager.Roles.Where(x => request.Roles.Contains(x.Id));
            var roleResult = await _userManager.AddToRolesAsync(appUser, roles.Select(x => x.Name).ToList());

            if (!roleResult.Succeeded)
                throw new Exception($"failed to add user '{request.Email}' to roles");

            var userBranches = new List<UserBranch>();
            foreach (var branch in request.Branches)
            {
                userBranches.Add(new UserBranch
                {
                    BranchId = branch,
                    AppUserId = appUser.Id,
                    CreateDate = DateTime.Now,
                    IsActive = true
                });
            }

            await _unitOfWork.GetRepository<UserBranch>().InsertRangeAsync(userBranches, cts.Token);

            if (request.DefaultModule is not null)
            {
                var userModule = new UserModule
                {
                    AppUserId = appUser.Id,
                    ModuleId = (int)request.DefaultModule,
                    CreateDate = DateTime.Now,
                    IsActive = true
                };
                await _unitOfWork.GetRepository<UserModule>().InsertAsync(userModule, cts.Token);
            }

            await _unitOfWork.Commit();

            return new RegistrationResponseDto
            {
                Id = appUser.Id,
                Email = appUser.Email,
                FullNameAr = appUser.FullNameAR,
                FullNameEn = appUser.FullNameEn
            };
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is null;
        }

        public async Task<bool> CheckUserNameExistsAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName) is null;
        }

        public async Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber)
        {
            return !await _userManager.Users.AnyAsync(x => string.Equals(x.PhoneNumber, phoneNumber));
        }

        public async Task<List<EnumAsListResponseDto>> GetAllLanguagesAsync(string lang)
        {
            return new List<EnumAsListResponseDto>
            {
                new EnumAsListResponseDto {
                    Key = Language.English.ToString(),
                    Value = string.Equals(lang, "En") ? "English": "الانجليزية"
                },
                new EnumAsListResponseDto {
                    Key = Language.Arabic.ToString(),
                    Value = string.Equals(lang, "En") ? "Arabic": "العربية"
                }
            };
        }

        public async Task<List<EnumAsListResponseDto>> GetAllStatusesAsync(string lang)
        {
            return new List<EnumAsListResponseDto>
            {
                new EnumAsListResponseDto {
                    Key = UserStatus.Active.ToString(),
                    Value = string.Equals(lang, "En") ? "Active": "نشط"
                },
                new EnumAsListResponseDto {
                    Key = UserStatus.Inactive.ToString(),
                    Value = string.Equals(lang, "En") ? "Active": "غير نشط"
                }
            };
        }

        public async Task<List<EnumAsListResponseDto>> GetAllGendersAsync(string lang)
        {
            return new List<EnumAsListResponseDto>
            {
                new EnumAsListResponseDto {
                    Key = Gender.Male.ToString(),
                    Value = string.Equals(lang, "En") ? "Male": "ذكر"
                },
                new EnumAsListResponseDto {
                    Key = Gender.Female.ToString(),
                    Value = string.Equals(lang, "En") ? "Female": "أنثى"
                },
                new EnumAsListResponseDto {
                    Key = Gender.Other.ToString(),
                    Value = string.Equals(lang, "En") ? "Other": "أخرى"
                }
            };
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null)
                user = await _userManager.FindByEmailAsync(request.UserName);

            if (user is null)
                throw new Exception($"user '{request.UserName}' not found");

            var validPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!validPasswordResult.Succeeded) throw new Exception($"creds for '{request.UserName}' are not correct");

            string token = await CreateTokenAsync(user);

            return new LoginResponseDto
            {
                Email = user.Email,
                UserName = user.UserName,
                AccessToken = token
            };
        }

        public async Task ChangeAccountPasswordAsync(ChangePasswordRequestDto request, ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(claimsPrincipal);
            if (user is null) throw new Exception($"{nameof(user.Email)}. This email not found");

            var currentPasswordMatchedWithEmail = await _signInManager.CheckPasswordSignInAsync(user, request.CurrentPassword, false);
            if (!currentPasswordMatchedWithEmail.Succeeded) throw new Exception("entered password is not valid");

            bool isNewPasswordDifferentFromCurrent = await _userManager.CheckPasswordAsync(user, request.NewPassword);
            if (isNewPasswordDifferentFromCurrent) throw new Exception("new password must be different from old password");

            var identityResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!identityResult.Succeeded) throw new Exception("something went wrong. Please, try aganin");

            await _appUserChangePasswordReasonRepository.AddAsync(new AppUserChangePasswordReason
            {
                UserId = request.UserId,
                ReasonId = request.ReasonId
            });
            //return new BaseResponse(true);
        }

        public async Task<IReadOnlyList<AppUserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            List<AppUserResponseDto> allUsers = new List<AppUserResponseDto>();

            foreach (var user in users)
            {
                allUsers.Add(new AppUserResponseDto(user.Id, user.UserName, user.Email, user.FullNameAR));
            }

            return allUsers;
        }
        #endregion

        #region Change Password Reasons
        public async Task<ChangePasswordReasonResponseDto> AddChangePasswordReasonAsync(ChangePasswordReasonCreationRequestDto request)
        {
            var newReason = new ChangePasswordReason
            {
                ReasonAr = request.ReasonAr,
                ReasonEn = request.ReasonEn
            };

            await _changePasswordReasonRepository.AddAsync(newReason);

            return new ChangePasswordReasonResponseDto(newReason.Id, newReason.ReasonAr, newReason.ReasonEn);
        }

        public async Task<ChangePasswordReasonResponseDto> UpdateChangePasswordReasonAsync(ChangePasswordReasonUpdateRequestDto request)
        {
            var reason = await _changePasswordReasonRepository.GetByIdAsync(request.Id);
            if (reason is null) throw new Exception("reason not found");

            reason.ReasonAr = request.ReasonAr;
            reason.ReasonEn = request.ReasonEn;

            await _changePasswordReasonRepository.UpdateAsync(reason);

            return new ChangePasswordReasonResponseDto(reason.Id, reason.ReasonAr, reason.ReasonEn);
        }

        public async Task<ChangePasswordReasonResponseDto> GetChangePasswordReasonByIdAsync(Guid reasonId)
        {
            var reason = await _changePasswordReasonRepository.GetByIdAsync(reasonId);
            if (reason is null) throw new Exception("reason not found");

            return new ChangePasswordReasonResponseDto(reason.Id, reason.ReasonAr, reason.ReasonEn);
        }

        public async Task DeleteChangePasswordReasonByIdAsync(Guid reasonId)
        {
            var reason = await _changePasswordReasonRepository.GetByIdAsync(reasonId);
            if (reason is null) throw new Exception("reason not found");

            await _changePasswordReasonRepository.DeleteAsync(reason);
        }

        public async Task<IReadOnlyList<ChangePasswordReasonResponseDto>> GetAllChangePasswordReasonsByIdAsync()
        {
            var reasons = await _changePasswordReasonRepository.GetAllAsync();

            var allReasons = new List<ChangePasswordReasonResponseDto>();

            foreach (var reason in reasons)
            {
                allReasons.Add(new ChangePasswordReasonResponseDto(reason.Id, reason.ReasonAr, reason.ReasonEn));
            }

            return allReasons;
        }
        #endregion

        #region Manage Roles
        public async Task<AppUserRoleResponseDto> CreateUserRoleAsync(AppUserRoleCreationRequestDto request)
        {
            if (await _roleManager.RoleExistsAsync(request.Name))
                throw new Exception("this role is registred");

            var newRole = new AppUserRole
            {
                Name = request.Name,
                DescriptionAr = request.DescriptionAr,
                DescriptionEn = request.DescriptionEn,
                FullDescription = request.FulDescription
            };

            var identityResult = await _roleManager.CreateAsync(newRole);

            return new AppUserRoleResponseDto(newRole.Id, newRole.Name, newRole.DescriptionAr, newRole.DescriptionEn,
                newRole.FullDescription);
        }

        public async Task<AppUserRoleResponseDto> UpdateUserRoleAsync(AppUserRoleUpdateRequestDto request)
        {
            var oldRole = await _roleManager.FindByIdAsync(request.Id);
            if (oldRole is null) throw new Exception("role not found");

            var roleByName = await _roleManager.FindByNameAsync(request.Name);

            if (await _roleManager.RoleExistsAsync(request.Name) && roleByName.Id != oldRole.Id)
                throw new Exception("this role is registred before");

            oldRole.Name = request.Name;
            oldRole.DescriptionAr = request.DescriptionAr;
            oldRole.DescriptionEn = request.DescriptionEn;
            oldRole.FullDescription = request.FulDescription;

            var identityResult = await _roleManager.UpdateAsync(oldRole);

            return new AppUserRoleResponseDto(oldRole.Id, oldRole.Name, oldRole.DescriptionAr, oldRole.DescriptionEn,
                oldRole.FullDescription);
        }

        public async Task<AppUserRoleResponseDto> GetUserRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) throw new Exception("role not found");

            return new AppUserRoleResponseDto(role.Id, role.Name, role.DescriptionAr, role.DescriptionEn,
                role.FullDescription);
        }

        public async Task<(IReadOnlyList<AppUserRoleResponseDto>, int)> GetUsersRolesAsync(int pageIndex,
            int pageSize)
        {
            var roles = await _appUserRoleRepository.GetAllAppUserRolesPaginatedAsync(pageIndex
                , pageSize);

            var count = await _appUserRoleRepository.GetAllAppuserRolesCountAsync();

            List<AppUserRoleResponseDto> allRoles = new List<AppUserRoleResponseDto>();

            foreach (var role in roles)
            {
                allRoles.Add(new AppUserRoleResponseDto(role.Id, role.Name, role.DescriptionAr, role.DescriptionEn,
                    role.FullDescription));
            }

            return (allRoles, count);
        }

        public async Task DeleteUserRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) throw new Exception("role not found");

            await _roleManager.DeleteAsync(role);
        }

        public async Task AddRolePrivilegeAsync(RolePrivilegeRequestDto request)
        {
            if (!request.PagesIds.Any())
                throw new Exception("must enter at least one page Id");

            await _rolePrivilegeRepository.AddAsync(request.RoleId, request.PagesIds);
        }

        public async Task<AppUserRoleWithPrivilegesResponseDto> GetRoleWithPrivilegesAsync(string roleId)
        {
            var roleWithPrivileges = await _appUserRoleRepository.GetRoleWithPrivilegesAsync(roleId);
            var privilegesIds = roleWithPrivileges.RolePrivileges.Select(x => x.PageId).ToList();

            return new AppUserRoleWithPrivilegesResponseDto
            {
                Id = roleWithPrivileges.Id,
                Name = roleWithPrivileges.Name,
                DescriptionEn = roleWithPrivileges.DescriptionEn,
                DescriptionAr = roleWithPrivileges.DescriptionAr,
                FulDescription = roleWithPrivileges.FullDescription,
                RolePrivileges = privilegesIds
            };
        }

        public async Task<RolePrivilegeResponseDto> GetModulesWithPagesAsync()
        {
            var spec = new AppPageSpec();

            var pages = await _unitOfWork.GetRepository<AppPage>().GetAllAsync(spec);

            var groupedPages = pages.GroupBy(p => p.AppModule.EnglishName)
                        .Select(moduleGroup => new
                        {
                            PageModule = moduleGroup.Key,
                            PageTypes = moduleGroup.GroupBy(p => p.PageType)
                                                 .Select(typeGroup => new
                                                 {
                                                     PageType = typeGroup.Key,
                                                     Pages = typeGroup.ToList()
                                                 })
                                                 .ToList()
                        });

            List<ModuleWithPageResponseDto> modules = new List<ModuleWithPageResponseDto>();

            foreach (var moduleGroup in groupedPages)
            {
                var module = new ModuleWithPageResponseDto();
                module.ModuleName = moduleGroup.PageModule;
                
                var types = new List<PageTypeWithPage>();
                foreach (var typeGroup in moduleGroup.PageTypes)
                {
                    var type = new PageTypeWithPage();
                    type.PageType = typeGroup.PageType.ToString();
                    var typePages = new List<Page>();
                    foreach (var page in typeGroup.Pages)
                    {
                        typePages.Add(new Page
                        {
                            PageId=page.Id,
                            PageDesCription = page.PageDesCription,
                            PageNameEn = page.PageNameEn,
                            PageNameAr = page.PageNameAr,
                            PageUrl = page.PageUrl,
                            Sort = page.Sort
                        });

                        module.ModuleId = page.AppModuleId;
                        type.TypeId = (int)page.PageType;
                    }

                    type.Pages = typePages;
                    types.Add(type);
                }

                module.Types = types;
                modules.Add(module);
            }

            return new RolePrivilegeResponseDto
            {
                Parent="Privileges",
                Modules=modules
            };
        }
        #endregion

        #region Helper Methods
        private async Task<string> CreateTokenAsync(AppUser user)
        {
            var dbClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            var claimRoles = userRoles.Select(role => new Claim(ClaimTypes.Role, role));
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.UserName)
            }
            .Union(claimRoles)
            .Union(dbClaims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signingCredentials,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInHours),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}
