using Api.CustomConfiguration;
using Application;
using Application.Contracts.Identity;
using Application.Dtos.Authentication.Requests;
using Application.Dtos.Setup.IdentitificationTypes.Requests;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Authentication
{
    public class AuthenticationController : CommonBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, 
            IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler,
            IAuthenticationService authenticationService, IOptions<CustomServiceConfiguration> options) : 
            base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options

                )
        {
            _authenticationService = authenticationService;
        }

        #region AppUser
        [HttpGet("languages")]
        public async Task<IResponseModel> GetLanguages()
        {
            var response = await _authenticationService.GetAllLanguagesAsync(Lang);

            return responseModelHandler.GetResponseModel(response, "DONE", StatusCodes.Status200OK, Lang);

        }

        [HttpGet("userstatuses")]
        public async Task<IResponseModel> GetUserStatuses()
        {
            var response = await _authenticationService.GetAllLanguagesAsync(Lang);

            return responseModelHandler.GetResponseModel(response, "DONE", StatusCodes.Status200OK, Lang);

        }

        [HttpGet("genders")]
        public async Task<IResponseModel> GetGenders()
        {
            var response = await _authenticationService.GetAllLanguagesAsync(Lang);

            return responseModelHandler.GetResponseModel(response, "DONE", StatusCodes.Status200OK, Lang);

        }

        [HttpGet("emailexists")]
        public async Task<IResponseModel> CheckEmailExists(string email)
        {
            using var cts = GetCommandCancellationToken();

            var response = await _authenticationService.CheckEmailExistsAsync(email);

            return responseModelHandler.GetResponseModel(response, "DONE", StatusCodes.Status200OK, Lang);
        }

        [HttpGet("usernameexists")]
        public async Task<IResponseModel> CheckUserNameExists(string userName)
        {
            using var cts = GetCommandCancellationToken();

            var response = await _authenticationService.CheckEmailExistsAsync(userName);

            return responseModelHandler.GetResponseModel(response, "DONE", StatusCodes.Status200OK, Lang);
        }

        [HttpGet("phonenumberexists")]
        public async Task<IResponseModel> CheckPhoneNmberExists(string phoneNumber)
        {
            using var cts = GetCommandCancellationToken();

            var response = await _authenticationService.CheckEmailExistsAsync(phoneNumber);

            return responseModelHandler.GetResponseModel(response, "DONE", StatusCodes.Status200OK, Lang);
        }

        [HttpPost("register")]
        public async Task<IResponseModel> Register([FromBody]RegistrationRequestDto request)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(request, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            var response = await _authenticationService.RegisterAsync(request, cts);

            return responseModelHandler.GetResponseModel(response, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto requestDto)
        {
            using var cts = GetCommandCancellationToken();

            var response = await _authenticationService.LoginAsync(requestDto);

            return Ok(response);
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto requestDto)
        {
            using var cts = GetCommandCancellationToken();

            await _authenticationService.ChangeAccountPasswordAsync(requestDto, User );

            return Ok();
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            using var cts = GetCommandCancellationToken();
            var response = await _authenticationService.GetAllUsersAsync();

            return Ok(response);
        }
        #endregion

        #region Change Password Reasons
        [HttpPost("changepasswordreason")]
        public async Task<IActionResult> CreateChangePaswordReason(ChangePasswordReasonCreationRequestDto requestDto)
        {
            using var cts = GetCommandCancellationToken();
            var response = await _authenticationService.AddChangePasswordReasonAsync(requestDto);

            return Ok(response);
        }

        [HttpPut("UpdateChangePaswordReason")]
        public async Task<IActionResult> UpdateChangePaswordReason(ChangePasswordReasonUpdateRequestDto requestDto)
        {
            using var cts = GetCommandCancellationToken();
            var response = await _authenticationService.UpdateChangePasswordReasonAsync(requestDto);

            return Ok(response);
        }

        [HttpPost("DeleteChangePaswordReason")]
        public async Task<IActionResult> DeleteChangePaswordReason(Guid reasonId)
        {
            using var cts = GetCommandCancellationToken();
            await _authenticationService.DeleteChangePasswordReasonByIdAsync(reasonId);

            return Ok();
        }

        [HttpGet("changepasswordreasonbyid")]
        public async Task<IActionResult> GetChangePaswordReason(Guid reasonId)
        {
            using var cts = GetCommandCancellationToken();

            var response = await _authenticationService.GetChangePasswordReasonByIdAsync(reasonId);

            return Ok(response);
        }

        [HttpGet("GetAllChangePaswordReasons")]
        public async Task<IActionResult> GetAllChangePaswordReasons()
        {
            using var cts = GetCommandCancellationToken();

            var response = await _authenticationService.GetAllChangePasswordReasonsByIdAsync();

            return Ok(response);
        }
        #endregion

        #region Role Manager
        [HttpPost("role")]
        public async Task<IResponseModel> CreateRole(AppUserRoleCreationRequestDto request)
        {
            using var cts = GetCommandCancellationToken();

            var response = await _authenticationService.CreateUserRoleAsync(request);

            return responseModelHandler.GetResponseModel(response, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
        }

        [HttpPut("role")]
        public async Task<IResponseModel> UpdateRole(AppUserRoleUpdateRequestDto request)
        {
            using var cts = GetCommandCancellationToken();

            var response = await _authenticationService.UpdateUserRoleAsync(request);

            return responseModelHandler.GetResponseModel(response, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
        }

        [HttpGet("rolebyid")]
        public async Task<IResponseModel> GetRoleById(string roleId)
        {
            using var cts = GetCommandCancellationToken();

            var response = await _authenticationService.GetUserRoleByIdAsync(roleId);
            
            return responseModelHandler.GetResponseModel(response, "DONE", StatusCodes.Status200OK, Lang);
        }

        [HttpGet("allroles")]
        public async Task<IPaginatedModelHandler> GetRoles([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            var (data, count) = await _authenticationService.GetUsersRolesAsync(paginationParams.PageNumber,
                paginationParams.PageSize);

            IPaginatedModelHandler responseModel = _paginatedModelHandler.Create(data,
              paginationParams.PageNumber, paginationParams.PageSize, count)
              .WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

            AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

            return responseModel;
        }

        [HttpDelete("role")]
        public async Task<IResponseModel> DeleteRole(string roleId)
        {
            using var cts = GetCommandCancellationToken();

            await _authenticationService.DeleteUserRoleAsync(roleId);

            return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);
        }

        [HttpPost("roleprivilege")]
        public async Task<IResponseModel> AddRolePrivilegeRole([FromBody] RolePrivilegeRequestDto request)
        {
            using var cts = GetCommandCancellationToken();

            await _authenticationService.AddRolePrivilegeAsync(request);

            return responseModelHandler.GetResponseModel(null, "SAVE_SUCCESS", StatusCodes.Status204NoContent, Lang);
        }

        [HttpGet("roleprivileges/{roleId}")]
        public async Task<IResponseModel> AddRolePrivilegeRole(string roleId)
        {
            using var cts = GetCommandCancellationToken();

            var response = await _authenticationService.GetRoleWithPrivilegesAsync(roleId);

            return responseModelHandler.GetResponseModel(response, "SAVE_SUCCESS", StatusCodes.Status204NoContent, Lang);
        }

        [HttpGet("moduleswithpages")]
        public async Task<IResponseModel> GetModulesWithPages()
        {
            using var cts = GetCommandCancellationToken();

            var response = await _authenticationService.GetModulesWithPagesAsync();

            return responseModelHandler.GetResponseModel(response, "SAVE_SUCCESS", StatusCodes.Status204NoContent, Lang);
        }
        #endregion
    }
}
