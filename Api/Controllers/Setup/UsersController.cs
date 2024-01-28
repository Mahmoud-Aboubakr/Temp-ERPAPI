using Api.Controllers;
using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.User;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api;

public class UsersController : CommonBaseController
{
	public UsersController(IUnitOfWork uof, IMapper mapper, 
		IHttpContextAccessor accessor,
		IResponseModelHandler responseModelHandler,
		IPaginatedModelHandler paginatedModelHandler, 
		IOptions<CustomServiceConfiguration> options) : base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler,options)
	{
	}

	//[HttpGet("GetUsers")]
	//public async Task<IPaginatedModelHandler> GetUsers([FromQuery] PaginationParams paginationParams)
	//{
	//	IGenericRepository<User> userRepository = uof.GetRepository<User>();

	//	ISpecification<User> specifications = Specifications<User>()
	//	.Where(c => c.IsDeleted == false)
	//	.Page(paginationParams.PageNumber, paginationParams.PageSize)
	//	.Build();

	//	IEnumerable<ReadUserDto> users = mapper.Map<IEnumerable<ReadUserDto>>(await userRepository.GetAllAsync(specifications));

	//	IPaginatedModelHandler responseModel = paginatedModelHandler.Create(users, paginationParams.PageNumber, paginationParams.PageSize, await userRepository.CountAsync()).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

	//	return responseModel;
	//}

	//[HttpGet("GetUser/{userId}")]
	//public async Task<IResponseModel> GetUser([FromRoute] int userId)
	//{
	//	IGenericRepository<User> userRepository = uof.GetRepository<User>();

	//	ISpecification<User> specifications = Specifications<User>()
	//	.Where(user => user.Id == userId)
	//	.Include(user => user.Employee)
	//	.Build();

	//	User user = await userRepository.GetAsync(specifications);

	//	if (user is null || user.IsDeleted)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

	//	return responseModelHandler.GetResponseModel(mapper.Map<ReadUserDto>(user), "DONE", StatusCodes.Status200OK, Lang);
	//}

	[HttpPost("AddUser")]
	public async Task<IResponseModel> AddUser([FromBody] CreateUserDto user)
    {
        using var cts = GetCommandCancellationToken();

        if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(user, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<User> userRepository = uof.GetRepository<User>();

		User newUser = mapper.Map<User>(user);

		newUser.IsActive = true;
		newUser.CreateDate = DateTime.Now;

		await userRepository.InsertAsync(newUser, cts.Token);
		await uof.Commit();
		return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	}

	[HttpPut("{id}")]
	public async Task<IResponseModel> UpdateUser(int id, [FromBody] UpdateUserDto user)
	{
		if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(user, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		if (id != user.Id)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<User> userRepository = uof.GetRepository<User>();

		User updateUser = mapper.Map<UpdateUserDto, User>(user);
		updateUser.EditDate = DateTime.Now;

		userRepository.Update(updateUser);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
	}

	[HttpDelete("{id}")]
	public async Task<IResponseModel> DeleteUser([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<User> userRepository = uof.GetRepository<User>();

		User currentUser = await userRepository.GetByIdAsync(id, cts.Token);

		if (currentUser is null || currentUser.IsDeleted)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		currentUser.DeleteDate = DateTime.Now;
		userRepository.SoftDelete(currentUser);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

	}
}
