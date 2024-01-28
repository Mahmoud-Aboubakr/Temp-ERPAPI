using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Specifications.HR.Setup;
using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Setup
{

    public class EmployeeFilesController : CommonBaseController
	{

		public EmployeeFilesController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, 
			IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler,

            IOptions<CustomServiceConfiguration> options
			) : base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler,options)
		{
		}

		// GET: api/EmployeeFiles
		[HttpGet("GetEmployeeFiless")]
		public async Task<IResponseModel> GetEmployeeFiless([FromQuery] PaginationParams paginationParams)
		{
            using var cts = GetCommandCancellationToken();

            IGenericRepository<EmployeeFiles> EmployeeFilesRepository = uof.GetRepository<EmployeeFiles>();
            using (ISpecification<EmployeeFiles> specifications = new EmployeeFilesSpec(paginationParams.PageSize, paginationParams.PageNumber))
            {
                IEnumerable<ReadEmployeeFilesDto> EmployeeFiless = mapper.Map<IEnumerable<ReadEmployeeFilesDto>>(await EmployeeFilesRepository.GetAllAsync(specifications, cts.Token));
                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(EmployeeFiless, paginationParams.PageNumber, paginationParams.PageSize, await EmployeeFilesRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);
                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);
                return responseModel;
            }			
		}


		[HttpPost("AddEmployeeFiles")]
		public async Task<IResponseModel> AddEmployeeFiles([FromBody] CreateEmployeeFilesDto CreateEmployeeFiles)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
				return _responseModelHandler.GetResponseModel(CreateEmployeeFiles, "VALIDATION", StatusCodes.Status404NotFound, Lang);

			IGenericRepository<Employee> EmployeeRepository = uof.GetRepository<Employee>();
			var FindEmployee = await EmployeeRepository.GetByIdAsync(CreateEmployeeFiles.EmployeeId, cts.Token);

			if (CreateEmployeeFiles.EmployeeId == 0 || FindEmployee == null)
				return _responseModelHandler.GetResponseModel(CreateEmployeeFiles, "EMPOLYEE_NOTFOUND", StatusCodes.Status404NotFound, Lang);

			IGenericRepository<HRFile> HRFileRepository = uof.GetRepository<HRFile>();
			var FindFile = await HRFileRepository.GetByIdAsync(CreateEmployeeFiles.HRFileId, cts.Token);

			if (CreateEmployeeFiles.HRFileId == 0 || FindFile == null)
				return _responseModelHandler.GetResponseModel(CreateEmployeeFiles, "HRFILE_NOTFOUND", StatusCodes.Status404NotFound, Lang);

			IGenericRepository<EmployeeFiles> EmployeeFilessRepository = uof.GetRepository<EmployeeFiles>();

			EmployeeFiles newEmployeeFiles = mapper.Map<EmployeeFiles>(CreateEmployeeFiles);

			newEmployeeFiles.IsActive = true;
			newEmployeeFiles.CreateDate = DateTime.Now;

			await EmployeeFilessRepository.InsertAsync(newEmployeeFiles, cts.Token);
			await uof.Commit();
			return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
		}

		// PUT: api/EmployeeFiles/5
		//[HttpPut("UpdateEmployeeFiles/{id}")]
		//public async Task<IResponseModel> UpdateEmployeeFiles(int id, [FromBody] UpdateEmployeeFilessDto EmployeeFiless)
		//{
		//	if (!ModelState.IsValid)
		//		return responseModelHandler.GetResponseModel(EmployeeFiless, "VALIDATION", StatusCodes.Status404NotFound, Lang);

		//	if (id != EmployeeFiless.Id)
		//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		//	IGenericRepository<EmployeeFiles> EmployeeFilesRepository = uof.GetRepository<EmployeeFiles>();

		//	EmployeeFiles updateEmployeeFiles = mapper.Map<UpdateEmployeeFilessDto, EmployeeFiles>(EmployeeFiless);
		//	updateEmployeeFiles.EditDate = DateTime.Now;

		//	EmployeeFilesRepository.Update(updateEmployeeFiles);
		//	await uof.Commit();

		//	return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
		//}

		// POST: api/EmployeeFiles
		// DELETE: api/EmployeeFiles/5
		//[HttpDelete("DeleteEmployeeFiles/{id}")]
		//public async Task<IResponseModel> DeleteEmployeeFiles([FromRoute] int id)
		//{
		//	IGenericRepository<EmployeeFiles> EmployeeFilesRepository = uof.GetRepository<EmployeeFiles>();

		//	EmployeeFiles currentEmployeeFiless = await EmployeeFilesRepository.GetByIdAsync(id);

		//	if (currentEmployeeFiless is null)
		//		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

		//	currentEmployeeFiless.DeleteDate = DateTime.Now;
		//	EmployeeFilesRepository.SoftDelete(currentEmployeeFiless);
		//	await uof.Commit();

		//	return _responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

		//}
	}
}