using Api.Controllers;
using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api;

public class CitiesController : CommonBaseController
{
	public CitiesController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor,
		IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler
        , IOptions<CustomServiceConfiguration> options) : 
	base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
	{
	}

    [HttpGet("GetCities")]
    public async Task<IPaginatedModelHandler> GetCities([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<City> cityRepository = uof.GetRepository<City>();
		using (ISpecification<City> specifications = new CitiesSpec(paginationParams.PageSize, paginationParams.PageNumber))
		{
			IEnumerable<ReadCityDto> cities = mapper.Map<IEnumerable<ReadCityDto>>(await cityRepository.GetAllAsync(specifications, cts.Token));

			IPaginatedModelHandler responseModel = paginatedModelHandler.Create(cities, paginationParams.PageNumber, paginationParams.PageSize, await cityRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

			AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

			return responseModel;
		}
    }

    [HttpGet("GetCity/{cityId}")]
	public async Task<IResponseModel> GetCity([FromRoute] int cityId)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<City> cityRepository = uof.GetRepository<City>();
		using (ISpecification<City> specifications = new CitiesSpec(cityId))
        {
			City city = await cityRepository.GetByIdAsync(cityId, cts.Token);

			if (city is null || city.IsDeleted)
				return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

			return responseModelHandler.GetResponseModel(mapper.Map<ReadCityDto>(city), "DONE", StatusCodes.Status200OK, Lang);
		}
	}

	[HttpPost("AddCity")]
	public async Task<IResponseModel> AddCity([FromBody] CreateCityDto city)
    {
        using var cts = GetCommandCancellationToken();

        if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(city, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<City> cityRepository = uof.GetRepository<City>();

		City newCity = mapper.Map<City>(city);

		newCity.IsActive = true;
		newCity.CreateDate = DateTime.Now;

		await cityRepository.InsertAsync(newCity, cts.Token);
		await uof.Commit();
		return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	}

	[HttpPut("UpdateCity/{id}")]
	public async Task<IResponseModel> UpdateCity(int id, [FromBody] UpdateCityDto city)
    {

        if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(city, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		if (id != city.Id)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<City> cityRepository = uof.GetRepository<City>();

		City updateCity = mapper.Map<UpdateCityDto, City>(city);
		updateCity.EditDate = DateTime.Now;

		cityRepository.Update(updateCity);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
	}

	[HttpDelete("DeleteCity/{id}")]
	public async Task<IResponseModel> DeleteCity([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<City> cityRepository = uof.GetRepository<City>();

		City currentCity = await cityRepository.GetByIdAsync(id, cts.Token);

		if (currentCity is null || currentCity.IsDeleted)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		currentCity.DeleteDate = DateTime.Now;
		cityRepository.SoftDelete(currentCity);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

	}

	////not recommended yet
	//[HttpPut]
	//[Route("Activate/{id}")]
	//public async Task<IResponseModel> ActivateCity([FromRoute] int id)
	//{
	//	IGenericRepository<City> cityRepository = uof.GetRepository<City>();

	//	City currentCity = await cityRepository.GetByIdAsync(id);

	//	if (currentCity is null)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	cityRepository.Activate(currentCity);
	//	await uof.Commit();

	//	return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
	//}
}
