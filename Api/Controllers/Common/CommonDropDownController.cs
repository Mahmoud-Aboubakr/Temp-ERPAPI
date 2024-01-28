using Api.CustomConfiguration;
using Application.Contracts.Specifications;
using Application.Dtos.General.Responses;
using Application.Dtos.Setup.LookUps;
using Application.Specifications.Setup;
using Domain.Entities.LookUps;
using Domain.Enums;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Common
{

    public class CommonDropDownController : CommonBaseController
    {
        public CommonDropDownController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler,
            IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options
            ) : base(uof, mapper, accessor, responseModelHandler,
                paginatedModelHandler, options)
        {
        }

        [HttpGet("GetAppModules")]
        public async Task<IResponseModel> GetAppModules([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();
            IGenericRepository<AppModule> commonRepository = uof.GetRepository<AppModule>();
            using (ISpecification<AppModule> specifications = new CommonDropDownSpec<AppModule>(paginationParams.PageSize, paginationParams.PageNumber))
            {
                IEnumerable<AppModuleDto> appModules = mapper.Map<IEnumerable<AppModuleDto>>(await commonRepository.GetAllAsync(specifications, cts.Token));

                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(appModules, paginationParams.PageNumber, paginationParams.PageSize, await commonRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

                return responseModel;
            }
        }

        [HttpGet("GetApplicationsNames")]
        public async Task<IResponseModel> GetApplicationsNames([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();
            IGenericRepository<ApplicationTbl> commonRepository = uof.GetRepository<ApplicationTbl>();
            using (ISpecification<ApplicationTbl> specifications = new CommonDropDownSpec<ApplicationTbl>(paginationParams.PageSize, paginationParams.PageNumber))
            {
                IEnumerable<ApplicationTblDto> applications = mapper.Map<IEnumerable<ApplicationTblDto>>(await commonRepository.GetAllAsync(specifications, cts.Token));

                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(applications, paginationParams.PageNumber, paginationParams.PageSize, await commonRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

                return responseModel;
            }
        }

        [HttpGet("GetPagesTypes")]
        public async Task<List<EnumAsListResponseDto>> GetPagesTypes(string lang)
        {
            return new List<EnumAsListResponseDto>
            {
                new EnumAsListResponseDto {
                    Key = PageType.Defined.ToString(),
                    Value = string.Equals(lang, "En") ? "Defined": "تعريفات"
                },
                new EnumAsListResponseDto {
                    Key = PageType.Transact.ToString(),
                    Value = string.Equals(lang, "En") ? "Transact": "برامج"
                },
                new EnumAsListResponseDto {
                    Key = PageType.Reports.ToString(),
                    Value = string.Equals(lang, "En") ? "Reports": "تقارير"
                }
            };
        }

    }
}
