using Api.Controllers;
using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api;

public class CountriesController : CommonBaseController
{
	public CountriesController(IUnitOfWork uof, IMapper mapper,
		IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler, 
		IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options) :
		base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler,options)
	{
	}

    [HttpGet("GetCountries")]
    public async Task<IPaginatedModelHandler> GetCountries([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<Country> countryRepository = uof.GetRepository<Country>();
		using(ISpecification<Country> specifications = new CountriesSpec(paginationParams.PageSize, paginationParams.PageNumber))
        {
			IEnumerable<ReadCountryDto> countries = mapper.Map<IEnumerable<ReadCountryDto>>(await countryRepository.GetAllAsync(specifications));

			IPaginatedModelHandler responseModel = paginatedModelHandler.Create(countries, paginationParams.PageNumber, paginationParams.PageSize, await countryRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

			return responseModel;
		}

        
    }

    [HttpGet("GetCountry/{countryId}")]
	public async Task<IResponseModel> GetCountry([FromRoute] int countryId)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<Country> countryRepository = uof.GetRepository<Country>();
		using (ISpecification<ApplicationPagePrefix> specifications = new ApplicationPagePrefixSpec(countryId))
        {
			Country country = await countryRepository.GetByIdAsync(countryId, cts.Token);

			if (country is null || country.IsDeleted)
				return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

			return responseModelHandler.GetResponseModel(mapper.Map<ReadCountryDto>(country), "DONE", StatusCodes.Status200OK, Lang);
		}
			
	}

	[HttpPost("AddCountry")]
	public async Task<IResponseModel> AddCountry([FromBody] CreateCountryDto country)
    {
        using var cts = GetCommandCancellationToken();

        if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(country, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<Country> countryRepository = uof.GetRepository<Country>();

		Country newCountry = mapper.Map<Country>(country);

		newCountry.IsActive = true;
		newCountry.CreateDate = DateTime.Now;

		await countryRepository.InsertAsync(newCountry, cts.Token);
		await uof.Commit();
		return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	}

	[HttpPut("{id}")]
	public async Task<IResponseModel> UpdateCountry(int id, [FromBody] UpdateCountryDto country)
	{
		if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(country, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		if (id != country.Id)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<Country> countryRepository = uof.GetRepository<Country>();

		Country updateCountry = mapper.Map<UpdateCountryDto, Country>(country);
		updateCountry.EditDate = DateTime.Now;

		countryRepository.Update(updateCountry);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
	}

	[HttpDelete("{id}")]
	public async Task<IResponseModel> DeleteCountry([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<Country> countryRepository = uof.GetRepository<Country>();

		Country currentCountry = await countryRepository.GetByIdAsync(id, cts.Token);

		if (currentCountry is null || currentCountry.IsDeleted)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		currentCountry.DeleteDate = DateTime.Now;
		countryRepository.SoftDelete(currentCountry);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

	}

	////not recommended yet
	//[HttpPut("AddGovernate")]
	//public async Task<IResponseModel> AddGovernorate([FromQuery] int countryId, [FromQuery] int governorateId)
	//{
	//	if (countryId is 0 || governorateId is 0)
	//		return responseModelHandler.GetResponseModel(null, "VALIDATION", StatusCodes.Status404NotFound, Lang);

	//	IGenericRepository<Country> countryRepository = uof.GetRepository<Country>();

	//	Country country = await countryRepository.GetByIdAsync(countryId);

	//	if (country is null || country.IsDeleted)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	IGenericRepository<Governorate> governorateRepository = uof.GetRepository<Governorate>();

	//	Governorate governorate = await governorateRepository.GetByIdAsync(governorateId);

	//	if (governorate is null || governorate.IsDeleted)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	country.Governorates.Add(governorate);

	//	await uof.Commit();
	//	return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	//}

	////not recommended yet
	//[HttpPut]
	//[Route("Activate/{id}")]
	//public async Task<IResponseModel> ActivateCountry([FromRoute] int id)
	//{
	//	IGenericRepository<Country> countryRepository = uof.GetRepository<Country>();

	//	Country currentCountry = await countryRepository.GetByIdAsync(id);

	//	if (currentCountry is null)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	countryRepository.Activate(currentCountry);
	//	await uof.Commit();

	//	return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
	//}

	////not recommended yet
	//[HttpDelete("RemoveGovernate")]
	//public async Task<IResponseModel> RemoveGovernorate([FromQuery] int countryId, [FromQuery] int governorateId)
	//{
	//	if (countryId is 0 || governorateId is 0)
	//		return responseModelHandler.GetResponseModel(null, "VALIDATION", StatusCodes.Status404NotFound, Lang);

	//	IGenericRepository<Country> countryRepository = uof.GetRepository<Country>();

	//	ISpecification<Country> specification = Specifications<Country>()
	//	.Where(c => c.Id == countryId)
	//	.Include(c => c.Governorates.Where(g => g.Id == governorateId)).Build();

	//	Country country = await countryRepository.GetAsync(specification);

	//	if (country is null || country.IsDeleted)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	Governorate governorate = country.Governorates.FirstOrDefault(g => g.Id == governorateId);

	//	if (governorate is null || governorate.IsDeleted)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	country.Governorates.Remove(governorate);

	//	await uof.Commit();
	//	return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	//}
}
