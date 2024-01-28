using Api.Controllers;
using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api;

public class CompaniesController : CommonBaseController
{
	public CompaniesController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, 
		IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler
         , IOptions<CustomServiceConfiguration> options) : base(uof, mapper, accessor, responseModelHandler,
			 paginatedModelHandler,options)
	{
	}

    [HttpGet]
    public async Task<IPaginatedModelHandler> GetCompanies([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<Company> companyRepository = uof.GetRepository<Company>();
		using (ISpecification<Company> specifications = new CompaniesSpec(paginationParams.PageSize, paginationParams.PageNumber))
        {
			IEnumerable<ReadCompanyDto> companies = mapper.Map<IEnumerable<ReadCompanyDto>>(await companyRepository.GetAllAsync(specifications, cts.Token));

			IPaginatedModelHandler responseModel = paginatedModelHandler.Create(companies, paginationParams.PageNumber, paginationParams.PageSize, await companyRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

			AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

			return responseModel;
		}
    }

    [HttpGet("{companyId}")]
	public async Task<IResponseModel> GetCompany([FromRoute] int companyId)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<Company> companyRepository = uof.GetRepository<Company>();
		using (ISpecification<Company> specifications = new CompaniesSpec(companyId))
        {
			Company company = await companyRepository.GetByIdAsync(companyId, cts.Token);

			if (company is null || company.IsDeleted)
				return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

			return responseModelHandler.GetResponseModel(mapper.Map<ReadCompanyDto>(company), "DONE", StatusCodes.Status200OK, Lang);
		}
			
	}

	[HttpPost]
	public async Task<IResponseModel> AddCompany([FromBody] CreateCompanyDto company)
    {
        using var cts = GetCommandCancellationToken();

        if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(company, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<Company> companyRepository = uof.GetRepository<Company>();

		Company newCompany = mapper.Map<Company>(company);

		newCompany.IsActive = true;
		newCompany.CreateDate = DateTime.Now;

		await companyRepository.InsertAsync(newCompany, cts.Token);
		await uof.Commit();
		return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	}

	[HttpPut("AddBranch")]
	public async Task<IResponseModel> AddBranch([FromQuery] int companyId, [FromQuery] int branchId)
    {
        using var cts = GetCommandCancellationToken();

        if (companyId is 0 || branchId is 0)
			return responseModelHandler.GetResponseModel(null, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<Company> companiesRepository = uof.GetRepository<Company>();

		Company company = await companiesRepository.GetByIdAsync(companyId, cts.Token);

		if (company is null || company.IsDeleted)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<Branch> branchesRepository = uof.GetRepository<Branch>();

		Branch branch = await branchesRepository.GetByIdAsync(branchId, cts.Token);

		if (branch is null || branch.IsDeleted)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		company.Branches.Add(branch);

		await uof.Commit();
		return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	}

	[HttpPut("{id}")]
	public async Task<IResponseModel> UpdateCompany(int id, [FromBody] UpdateCompanyDto company)
	{
		if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(company, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		if (id != company.Id)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		Company updatedCompany = mapper.Map<UpdateCompanyDto, Company>(company);
		updatedCompany.EditDate = DateTime.Now;

		uof.GetRepository<Company>().Update(updatedCompany);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
	}

	[HttpDelete("{id}")]
	public async Task<IResponseModel> DeleteCompany([FromRoute] int id)
	{
        using var cts = GetCommandCancellationToken();


        IGenericRepository<Company> companyRepository = uof.GetRepository<Company>();

		Company currentCompany = await companyRepository.GetByIdAsync(id, cts.Token);

		if (currentCompany is null || currentCompany.IsDeleted)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		currentCompany.DeleteDate = DateTime.Now;
		companyRepository.SoftDelete(currentCompany);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);
	}

	////not recommended yet
	//[HttpPut]
	//[Route("Activate/{id}")]
	//public async Task<IResponseModel> ActivateCompany([FromRoute] int id)
	//{
	//	IGenericRepository<Company> companyRepository = uof.GetRepository<Company>();

	//	Company currentCompany = await companyRepository.GetByIdAsync(id);

	//	if (currentCompany is null)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	companyRepository.Activate(currentCompany);
	//	await uof.Commit();

	//	return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
	//}

	////not recommended yet
	//[HttpPut("RemoveBranch")]
	//public async Task<IResponseModel> RemoveBranch([FromQuery] int companyId, [FromQuery] int branchId)
	//{
	//	if (companyId is 0 || branchId is 0)
	//		return responseModelHandler.GetResponseModel(null, "VALIDATION", StatusCodes.Status404NotFound, Lang);

	//	IGenericRepository<Company> companiesRepository = uof.GetRepository<Company>();

	//	ISpecification<Company> specification = Specifications<Company>()
	//	.Where(comp => comp.Id == companyId)
	//	.Include(b => b.Branches.Where(b => b.Id == branchId)).Build();

	//	Company company = await companiesRepository.GetAsync(specification);

	//	if (company is null || company.IsDeleted)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	IGenericRepository<Branch> branchesRepository = uof.GetRepository<Branch>();

	//	Branch branch = company.Branches.FirstOrDefault(b => b.Id == branchId);

	//	if (branch is null || branch.IsDeleted)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	company.Branches.Remove(branch);

	//	await uof.Commit();
	//	return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	//}
}
