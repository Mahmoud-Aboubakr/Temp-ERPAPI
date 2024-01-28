using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.ApplicationPagePrefix;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Setup
{

    public class ApplicationPagePrefixController : CommonBaseController
	{

		public ApplicationPagePrefixController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor,
			IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler ,
            IOptions<CustomServiceConfiguration> options
            ) : base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
		{
		}

		// GET: api/New
		[HttpGet("GetApplicationPagePrefixs")]
		public async Task<IResponseModel> GetApplicationPagePrefixs([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<ApplicationPagePrefix> applicationPagePrefixRepository = uof.GetRepository<ApplicationPagePrefix>();

			using (ISpecification<ApplicationPagePrefix> specifications = new ApplicationPagePrefixSpec(paginationParams.PageSize, paginationParams.PageNumber))
            {
				IEnumerable<ReadApplicationPagePrefixDto> applicationPagePrefix = mapper.Map<IEnumerable<ReadApplicationPagePrefixDto>>(await applicationPagePrefixRepository.GetAllAsync(specifications, cts.Token));

				IPaginatedModelHandler responseModel = paginatedModelHandler.Create(applicationPagePrefix, paginationParams.PageNumber, paginationParams.PageSize, await applicationPagePrefixRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

				AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

				return responseModel;
			}
		}

        // GET: api/New/5
        [HttpGet("GetApplicationPagePrefix/{id}")]
        public async Task<IResponseModel> GetApplicationPagePrefix([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<ApplicationPagePrefix> applicationPagePrefixRepository = uof.GetRepository<ApplicationPagePrefix>();
			using (ISpecification<ApplicationPagePrefix> specifications = new ApplicationPagePrefixSpec(id))
			{
				ApplicationPagePrefix applicationPagePrefix = await applicationPagePrefixRepository.GetAsync(specifications, cts.Token);

				if (applicationPagePrefix is null)
					return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

				return responseModelHandler.GetResponseModel(mapper.Map<ReadApplicationPagePrefixDto>(applicationPagePrefix), null, StatusCodes.Status200OK, Lang);
			}
        }

        // PUT: api/New/5
        [HttpPut("UpdateApplicationPagePrefix/{id}")]
		public async Task<IResponseModel> UpdateApplicationPagePrefix(int id, [FromBody] UpdateApplicationPagePrefixDto applicationPagePrefix)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
				return responseModelHandler.GetResponseModel(applicationPagePrefix, "VALIDATION", StatusCodes.Status404NotFound, Lang);

			if (id != applicationPagePrefix.Id)
				return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

			IGenericRepository<ApplicationPagePrefix> applicationPagePrefixRepository = uof.GetRepository<ApplicationPagePrefix>();

			ApplicationPagePrefix updateApplicationPagePrefix = mapper.Map<UpdateApplicationPagePrefixDto,ApplicationPagePrefix>(applicationPagePrefix);
			updateApplicationPagePrefix.EditDate = DateTime.Now;

			applicationPagePrefixRepository.Update(updateApplicationPagePrefix);
			await uof.Commit();

			return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
		}

		// POST: api/New
		[HttpPost("AddApplicationPagePrefix")]
		public async Task<IResponseModel> AddApplicationPagePrefix([FromBody] AddApplicationPagePrefixDto applicationPagePrefix)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
				return _responseModelHandler.GetResponseModel(applicationPagePrefix, "VALIDATION", StatusCodes.Status404NotFound, Lang);

			IGenericRepository<ApplicationPagePrefix> applicationPagePrefixRepository = uof.GetRepository<ApplicationPagePrefix>();

			ApplicationPagePrefix newApplicationPagePrefix = mapper.Map<ApplicationPagePrefix>(applicationPagePrefix);

			newApplicationPagePrefix.IsActive = true;
			newApplicationPagePrefix.CreateDate = DateTime.Now;

			await applicationPagePrefixRepository.InsertAsync(newApplicationPagePrefix, cts.Token);
			await uof.Commit();
			return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
		}

		// DELETE: api/New/5
		[HttpDelete("DeleteApplicationPagePrefix/{id}")]
		public async Task<IResponseModel> DeleteApplicationPagePrefix([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<ApplicationPagePrefix> applicationPagePrefixRepository = uof.GetRepository<ApplicationPagePrefix>();

			ApplicationPagePrefix currentApplicationPagePrefix = await applicationPagePrefixRepository.GetByIdAsync(id, cts.Token);

			if (currentApplicationPagePrefix is null)
				return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

			currentApplicationPagePrefix.DeleteDate = DateTime.Now;
			applicationPagePrefixRepository.SoftDelete(currentApplicationPagePrefix);
			await uof.Commit();

			return _responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

		}
	}
}