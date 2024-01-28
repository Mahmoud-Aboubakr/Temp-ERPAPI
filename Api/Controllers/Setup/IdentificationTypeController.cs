using Api.CustomConfiguration;
using Application;
using Application.Contracts.IServices;
using Application.Dtos.Setup.IdentitificationTypes.Requests;
using Application.Dtos.Setup.IdentitificationTypes.Responses;
using Application.Specifications.Setup.IdentitificationType;
using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Setup
{
    public class IdentificationTypeController : CommonBaseController
    {
        private readonly IIdentitificationTypeService _identityTypeService;

        public IdentificationTypeController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor,
            IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler,
            IIdentitificationTypeService identityTypeService, IOptions<CustomServiceConfiguration> options)
            : base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
        {
            _identityTypeService = identityTypeService;
        }

        [HttpPost]
        public async Task<IResponseModel> AddIdentityType(IdentitificationTypeCreationRequestDto request)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(request, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            var response = await _identityTypeService.AddIdentityTypeAsync(request);

            return responseModelHandler.GetResponseModel(response, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
        }

        [HttpPut]
        public async Task<IResponseModel> UpdateIdentityType(IdentitificationTypeUpdateRequestDto request)
        {

            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(request, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            var response = await _identityTypeService.UpdateIdentityTypeAsync(request);

            return responseModelHandler.GetResponseModel(response, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
        }

        [HttpGet("{id}")]
        public async Task<IResponseModel> GetIdentityTypeById(int id)
        {
            using var cts = GetCommandCancellationToken();

            var response = await _identityTypeService.GetIdentityTypeByIdAsync(id);

            return responseModelHandler.GetResponseModel(response, "DONE", StatusCodes.Status200OK, Lang);
        }

        //[HttpGet("all")]
        //public async Task<IActionResult> GetAllIdentityTypes([FromQuery] PaginationParams paginationParams)
        //{
        //    using var cts = GetCommandCancellationToken();

        //    var response = await _identityTypeService.GetAllIdentityTypesAsync();

        //    return Ok(response);
        //}

        [HttpGet("allwithspec")]
        public async Task<IPaginatedModelHandler> GetAllIdentityTypesWithSpec([FromQuery] IdentitificationTypeSpecParams specParams)
        {
            using var cts = GetCommandCancellationToken();

            var spec = new IdentityTypeWithFiltersSpecification(specParams);
            var data = await _uof.GetRepository<IdentityType>().GetAllAsync(spec);
            var result = _mapper.Map<IReadOnlyList<IdentitificationTypeResponseDto>>(data);

            var countSpec = new IdentityTypeWithFiltersForCountSpecification(specParams);
            var allDataCount = await _uof.GetRepository<IdentityType>().CountAsync(countSpec);

            IPaginatedModelHandler responseModel = _paginatedModelHandler.Create(result,
                specParams.PageIndex, specParams.PageSize, allDataCount)
                .WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

            AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

            return responseModel;
        }

        [HttpDelete("{id}")]
        public async Task<IResponseModel> DeleteIdentityTypeById(int id)
        {
            using var cts = GetCommandCancellationToken();

            await _identityTypeService.DeleteIdentityTypeByIdAsync(id);

            return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);
        }
    }
}
