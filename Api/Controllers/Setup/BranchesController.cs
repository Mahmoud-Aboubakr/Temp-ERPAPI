using Api.Controllers;
using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api;

public class BranchesController : CommonBaseController
{
	public BranchesController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, 
		IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler
		, IOptions<CustomServiceConfiguration> options) :
		base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler,options)
	{
	}

    [HttpGet]
    public async Task<IPaginatedModelHandler> GetBranches([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<Branch> branchRepository = uof.GetRepository<Branch>();
		using (ISpecification<Branch> specifications = new BranchesSpec(paginationParams.PageSize, paginationParams.PageNumber))
        {
			IEnumerable<ReadBranchDto> companies = mapper.Map<IEnumerable<ReadBranchDto>>(await branchRepository.GetAllAsync(specifications, cts.Token));
			IPaginatedModelHandler responseModel = paginatedModelHandler.Create(companies, paginationParams.PageNumber, paginationParams.PageSize, await branchRepository.CountAsync()).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);
			AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);
			return responseModel;
		}
    }

    [HttpGet("{id}")]
    public async Task<IResponseModel> GetBranch([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<Branch> branchRepository = uof.GetRepository<Branch>();
		using (ISpecification<Branch> specifications = new BranchesSpec(id))
        {
			Branch branch = await branchRepository.GetAsync(specifications, cts.Token);

			if (branch is null || branch.IsDeleted)
				return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

			return responseModelHandler.GetResponseModel(mapper.Map<ReadBranchDto>(branch), "DONE", StatusCodes.Status200OK, Lang);
		}
    }

    [HttpPost]
	public async Task<IResponseModel> AddBranch([FromBody] CreateBranchDto branch)
    {
        using var cts = GetCommandCancellationToken();

        if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(branch, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		IGenericRepository<Branch> branchRepository = uof.GetRepository<Branch>();

		Branch newBranch = mapper.Map<Branch>(branch);

		newBranch.IsActive = true;
		newBranch.CreateDate = DateTime.Now;

		await branchRepository.InsertAsync(newBranch, cts.Token);
		await uof.Commit();
		return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
	}

	[HttpPut("{id}")]
	public async Task<IResponseModel> UpdateBranch(int id, [FromBody] UpdateBranchDto branch)
    {

        if (!ModelState.IsValid)
			return responseModelHandler.GetResponseModel(branch, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		if (id != branch.Id)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		Branch updatedBranch = mapper.Map<UpdateBranchDto, Branch>(branch);
		updatedBranch.EditDate = DateTime.Now;

		uof.GetRepository<Branch>().Update(updatedBranch);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
	}

	[HttpDelete("{id}")]
	public async Task<IResponseModel> DeleteBranch([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<Branch> branchRepository = uof.GetRepository<Branch>();

		Branch currentBranch = await branchRepository.GetByIdAsync(id, cts.Token);

		if (currentBranch is null || currentBranch.IsDeleted)
			return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		currentBranch.DeleteDate = DateTime.Now;
		branchRepository.SoftDelete(currentBranch);
		await uof.Commit();

		return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);
	}

	////not recommended yet
	//[HttpPut]
	//[Route("Activate/{id}")]
	//public async Task<IResponseModel> ActivateBranch([FromRoute] int id)
	//{
	//	IGenericRepository<Branch> branchRepository = uof.GetRepository<Branch>();

	//	Branch currentBranch = await branchRepository.GetByIdAsync(id);

	//	if (currentBranch is null)
	//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

	//	branchRepository.Activate(currentBranch);
	//	await uof.Commit();

	//	return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
	//}
}

