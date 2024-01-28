using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.UserType;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Setup
{

    public class UserTypeController : CommonBaseController
	{

		public UserTypeController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler,
            IOptions<CustomServiceConfiguration> options) : 
			base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
		{
		}

		// GET: api/New
		[HttpGet("GetUserTypes")]
		public async Task<IResponseModel> GetUserTypes([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<UserType> userTypeRepository = uof.GetRepository<UserType>();

			using (ISpecification<UserType> specifications = new UserTypeSpec(paginationParams.PageSize, paginationParams.PageNumber))
			{
				IEnumerable<ReadUserTypeDto> userType = mapper.Map<IEnumerable<ReadUserTypeDto>>(await userTypeRepository.GetAllAsync(specifications, cts.Token));

				IPaginatedModelHandler responseModel = paginatedModelHandler.Create(userType, paginationParams.PageNumber, paginationParams.PageSize, await userTypeRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

				AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

				return responseModel;
			}
		}

		// GET: api/New/5
		[HttpGet("GetUserType/{id}")]
		public async Task<IResponseModel> GetUserType([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<UserType> userTypeRepository = uof.GetRepository<UserType>();
			using (ISpecification<UserType> specifications = new UserTypeSpec(id))
			{
				UserType userType = await userTypeRepository.GetAsync(specifications, cts.Token);

				if (userType is null)
					return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

				return responseModelHandler.GetResponseModel(mapper.Map<ReadUserTypeDto>(userType), null, StatusCodes.Status200OK, Lang);
			}
		}

        // PUT: api/New/5
        [HttpPut("UpdateUserType/{id}")]
		public async Task<IResponseModel> UpdateUserType(int id, [FromBody] UpdateUserTypeDto userType)
        {

            if (!ModelState.IsValid)
				return responseModelHandler.GetResponseModel(userType, "VALIDATION", StatusCodes.Status404NotFound, Lang);

			if (id != userType.Id)
				return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

			IGenericRepository<UserType> userTypeRepository = uof.GetRepository<UserType>();

			UserType updateApplicationPagePrefix = mapper.Map<UpdateUserTypeDto,UserType>(userType);
			updateApplicationPagePrefix.EditDate = DateTime.Now;

			userTypeRepository.Update(updateApplicationPagePrefix);
			await uof.Commit();

			return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
		}

		// POST: api/New
		[HttpPost("AddUserType")]
		public async Task<IResponseModel> AddUserType([FromBody] AddUserTypeDto userType)
		{
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
				return _responseModelHandler.GetResponseModel(userType, "VALIDATION", StatusCodes.Status404NotFound, Lang);

			IGenericRepository<UserType> userTypeRepository = uof.GetRepository<UserType>();

			UserType newUserType = mapper.Map<UserType>(userType);

			newUserType.IsActive = true;
			newUserType.CreateDate = DateTime.Now;

			await userTypeRepository.InsertAsync(newUserType, cts.Token);
			await uof.Commit();
			return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
		}

		// DELETE: api/New/5
		[HttpDelete("DeleteUserType/{id}")]
		public async Task<IResponseModel> DeleteUserType([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<UserType> userTypeRepository = uof.GetRepository<UserType>();

			UserType currentApplicationPagePrefix = await userTypeRepository.GetByIdAsync(id, cts.Token);

			if (currentApplicationPagePrefix is null)
				return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

			currentApplicationPagePrefix.DeleteDate = DateTime.Now;
			userTypeRepository.SoftDelete(currentApplicationPagePrefix);
			await uof.Commit();

			return _responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

		}
	}
}