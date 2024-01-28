using Api.Controllers;
using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api;

public class GovernorateController : CommonBaseController
{
	public GovernorateController(IUnitOfWork uof, IMapper mapper,
		IHttpContextAccessor accessor, 
		IResponseModelHandler responseModelHandler, 
		IPaginatedModelHandler paginatedModelHandler
		, IOptions<CustomServiceConfiguration> options) : 
		base(uof, mapper, accessor, responseModelHandler,
			paginatedModelHandler,options)
	{
	}

    [HttpGet("GetGovernorates")]
    public async Task<IPaginatedModelHandler> GetGovernorates([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<Governorate> governorateRepository = uof.GetRepository<Governorate>();
		using (ISpecification<Governorate> specifications = new GovernoratesSpec(paginationParams.PageSize, paginationParams.PageNumber))
		{

			IEnumerable<ReadGovernorateDto> cities = mapper.Map<IEnumerable<ReadGovernorateDto>>(await governorateRepository.GetAllAsync(specifications, cts.Token));

			IPaginatedModelHandler responseModel = paginatedModelHandler.Create(cities, paginationParams.PageNumber, paginationParams.PageSize, await governorateRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

			AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

			return responseModel;
		}
    }

	[HttpGet("GetGovernorate/{governorateId}")]
	public async Task<IResponseModel> GetGovernorate([FromRoute] int governorateId)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<Governorate> governorateRepository = uof.GetRepository<Governorate>();
		using (ISpecification<Governorate> specifications = new GovernoratesSpec(governorateId))
		{
			Governorate governorate = await governorateRepository.GetByIdAsync(governorateId, cts.Token);

			if (governorate is null || governorate.IsDeleted)
				return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

			return responseModelHandler.GetResponseModel(mapper.Map<ReadGovernorateDto>(governorate), "DONE", StatusCodes.Status200OK, Lang);
		}
	}

	[HttpPost("AddGovernorate")]
	public async Task<IResponseModel> AddGovernorate([FromBody] CreateGovernorateDto governorate)
    {
        using var cts = GetCommandCancellationToken();

        if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(governorate, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<Governorate> governorateRepository = uof.GetRepository<Governorate>();

		Governorate newGovernorate = mapper.Map<Governorate>(governorate);

		newGovernorate.IsActive = true;
		newGovernorate.CreateDate = DateTime.Now;

		await governorateRepository.InsertAsync(newGovernorate, cts.Token);
		await uof.Commit();
		return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	}

	[HttpPut("UpdateGovernorate/{id}")]
	public async Task<IResponseModel> UpdateGovernorate(int id, [FromBody] UpdateGovernorateDto governorate)
	{
		if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(governorate, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		if (id != governorate.Id)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<Governorate> governorateRepository = uof.GetRepository<Governorate>();

		Governorate updateGovernorate = mapper.Map<UpdateGovernorateDto, Governorate>(governorate);
		updateGovernorate.EditDate = DateTime.Now;

		governorateRepository.Update(updateGovernorate);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
	}

	[HttpDelete("DeleteGovernorate/{id}")]
	public async Task<IResponseModel> DeleteGovernorate([FromRoute] int id)
	{
		IGenericRepository<Governorate> governorateRepository = uof.GetRepository<Governorate>();

		Governorate currentGovernorate = await governorateRepository.GetByIdAsync(id);

		if (currentGovernorate is null || currentGovernorate.IsDeleted)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		currentGovernorate.DeleteDate = DateTime.Now;
		governorateRepository.SoftDelete(currentGovernorate);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

	}

	////not recommended yet
	//[HttpPut("AddCity")]
	//public async Task<IResponseModel> AddCity([FromQuery] int governorateId, [FromQuery] int cityId)
	//{
	//	if (governorateId is 0 || cityId is 0)
	//		return responseModelHandler.GetResponseModel(null, "VALIDATION", StatusCodes.Status404NotFound, Lang);

	//	IGenericRepository<Governorate> governorateRepository = uof.GetRepository<Governorate>();

	//	Governorate governorate = await governorateRepository.GetByIdAsync(governorateId);

	//	if (governorate is null || governorate.IsDeleted)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	IGenericRepository<City> citiesRepository = uof.GetRepository<City>();

	//	City city = await citiesRepository.GetByIdAsync(cityId);

	//	if (city is null || city.IsDeleted)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	governorate.Cities.Add(city);

	//	await uof.Commit();
	//	return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	//}

	////not recommended yet
	//[HttpPut]
	//[Route("Activate/{id}")]
	//public async Task<IResponseModel> ActivateGovernorate([FromRoute] int id)
	//{
	//	IGenericRepository<Governorate> governorateRepository = uof.GetRepository<Governorate>();

	//	Governorate currentGovernorate = await governorateRepository.GetByIdAsync(id);

	//	if (currentGovernorate is null)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	governorateRepository.Activate(currentGovernorate);
	//	await uof.Commit();

	//	return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
	//}

	////not recommended yet
	//[HttpDelete("RemoveCity")]
	//public async Task<IResponseModel> RemoveCity([FromQuery] int governorateId, [FromQuery] int cityId)
	//{
	//	if (governorateId is 0 || cityId is 0)
	//		return responseModelHandler.GetResponseModel(null, "VALIDATION", StatusCodes.Status404NotFound, Lang);

	//	IGenericRepository<Governorate> governorateRepository = uof.GetRepository<Governorate>();

	//	ISpecification<Governorate> specification = Specifications<Governorate>()
	//	.Where(gov => gov.Id == governorateId)
	//	.Include(c => c.Cities.Where(g => g.Id == cityId)).Build();

	//	Governorate governorate = await governorateRepository.GetAsync(specification);

	//	if (governorate is null || governorate.IsDeleted)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	City city = governorate.Cities.FirstOrDefault(g => g.Id == cityId);

	//	if (city is null || city.IsDeleted)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	governorate.Cities.Remove(city);

	//	await uof.Commit();
	//	return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	//}
}
