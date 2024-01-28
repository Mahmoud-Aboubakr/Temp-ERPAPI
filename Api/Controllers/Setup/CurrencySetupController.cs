using Api.Controllers;
using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Specifications;
using Application.Specifications.Inventory;
using Application.Specifications.Setup;
using Domain.Entities.Inventory;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api;

public class CurrencySetupController : CommonBaseController
{
	public CurrencySetupController(IUnitOfWork uof, IMapper mapper,
		IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler, 
		IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options) :
		base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler,options)
	{
	}

    [HttpGet("GetCurrencies")]
    public async Task<IPaginatedModelHandler> GetCurrencies([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<CurrencySetup> CurrencyRepository = uof.GetRepository<CurrencySetup>();
		using(ISpecification<CurrencySetup> specifications = new CurrenciesSpec(paginationParams.PageSize, paginationParams.PageNumber, term:paginationParams.Term))
        {
			specifications.Includes.Add(a => a.Country);
			IEnumerable<ReadCurrencyDto> Currencys = mapper.Map<IEnumerable<ReadCurrencyDto>>(await CurrencyRepository.GetAllAsync(specifications));
            var currencySearch = new BaseSpecification<CurrencySetup>(a => a.ArabicName.Contains(paginationParams.Term) || a.EnglishName.Contains(paginationParams.Term));
            IPaginatedModelHandler responseModel = paginatedModelHandler.Create(Currencys, paginationParams.PageNumber, paginationParams.PageSize, await CurrencyRepository.CountAsync(currencySearch, cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

			return responseModel;
		}

        
    }

    [HttpGet("GetCurrency/{CurrencyId}")]
	public async Task<IResponseModel> GetCurrency([FromRoute] int CurrencyId)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<CurrencySetup> CurrencyRepository = uof.GetRepository<CurrencySetup>();
		using (ISpecification<CurrencySetup> specifications = new CurrenciesSpec(CurrencyId))
        {
			CurrencySetup Currency = await CurrencyRepository.GetByIdAsync(CurrencyId, cts.Token);

			if (Currency is null || Currency.IsDeleted)
				return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

			return responseModelHandler.GetResponseModel(mapper.Map<ReadCurrencyDto>(Currency), "DONE", StatusCodes.Status200OK, Lang);
		}
			
	}

	[HttpPost("AddCurrency")]
	public async Task<IResponseModel> AddCurrency([FromBody] CreateCurrencyDto Currency)
    {
        using var cts = GetCommandCancellationToken();

        if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(Currency, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<Country> CountryRepository = uof.GetRepository<Country>();
		var getCountry = await CountryRepository.GetByIdAsync(Currency.CountryId, cts.Token); 
		if( getCountry == null)
            return _responseModelHandler.GetResponseModel(Currency, "COUNTRY_NOT_FOUND", StatusCodes.Status404NotFound, Lang);
		
        IGenericRepository<CurrencySetup> CurrencyRepository = uof.GetRepository<CurrencySetup>();
        //check default
        if (Currency.IsDefault)
        {
            var defaultSearch = new BaseSpecification<CurrencySetup>(a => a.IsDefault);
			var checkDefaultCount = await CurrencyRepository.CountAsync(defaultSearch, cts.Token);
			if(checkDefaultCount > 0)
                return _responseModelHandler.GetResponseModel(Currency, "CURRENCY_DEFAULT_EXSIT", StatusCodes.Status409Conflict, Lang);
        }
        CurrencySetup newCurrency = mapper.Map<CurrencySetup>(Currency);

		newCurrency.IsActive = true;
		newCurrency.CreateDate = DateTime.Now;

		await CurrencyRepository.InsertAsync(newCurrency, cts.Token);
		await uof.Commit();
		return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	}

	[HttpPut("{id}")]
	public async Task<IResponseModel> UpdateCurrency(int id, [FromBody] UpdateCurrencyDto Currency)
	{
		if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(Currency, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		if (id != Currency.Id)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<Country> CountryRepository = uof.GetRepository<Country>();
        var getCountry = await CountryRepository.GetByIdAsync(Currency.CountryId);
        if (getCountry == null)
            return _responseModelHandler.GetResponseModel(Currency, "COUNTRY_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<CurrencySetup> CurrencyRepository = uof.GetRepository<CurrencySetup>();
        //check default
        if (Currency.IsDefault)
        {
            var defaultSearch = new BaseSpecification<CurrencySetup>(a => a.IsDefault);
            var checkDefault = await CurrencyRepository.GetAsync(defaultSearch);
            if (checkDefault != null && checkDefault.Id != Currency.Id)
                return _responseModelHandler.GetResponseModel(Currency, "CURRENCY_DEFAULT_EXSIT", StatusCodes.Status409Conflict, Lang);
            if (checkDefault != null)
                CurrencyRepository.Detach(checkDefault);
        }

        CurrencySetup updateCurrency = mapper.Map<UpdateCurrencyDto, CurrencySetup>(Currency);
		updateCurrency.EditDate = DateTime.Now;

		CurrencyRepository.Update(updateCurrency);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
	}

	[HttpDelete("{id}")]
	public async Task<IResponseModel> DeleteCurrency([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<CurrencySetup> CurrencyRepository = uof.GetRepository<CurrencySetup>();

		CurrencySetup currentCurrency = await CurrencyRepository.GetByIdAsync(id, cts.Token);

		if (currentCurrency is null || currentCurrency.IsDeleted)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		currentCurrency.DeleteDate = DateTime.Now;
		CurrencyRepository.SoftDelete(currentCurrency);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

	}
}
