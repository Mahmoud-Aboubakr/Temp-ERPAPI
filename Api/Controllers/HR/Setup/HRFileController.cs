using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Specifications.HR.Setup;
using Application.Specifications.Setup;
using Domain.Entities.HR.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Setup
{

    public class HRFileController : CommonBaseController
	{

		public HRFileController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, 
            IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler
            , IOptions<CustomServiceConfiguration> options) : 
            base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
		{
		}

		// GET: api/HRFile
		[HttpGet("GetHRFiles")]
		public async Task<IResponseModel> GetHRFiles([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<HRFile> HRFileRepository = uof.GetRepository<HRFile>();
            using (ISpecification<HRFile> specifications = new HRFileSpec(paginationParams.PageSize, paginationParams.PageNumber))
            {
                IEnumerable<ReadHRFileDto> HRFiles = mapper.Map<IEnumerable<ReadHRFileDto>>(await HRFileRepository.GetAllAsync(specifications, cts.Token));
                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(HRFiles, paginationParams.PageNumber, paginationParams.PageSize, await HRFileRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);
                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);
                return responseModel;
            }
			
		}

		[HttpPost("AddHRFile")]
		public async Task<IResponseModel> AddHRFile([FromBody] CreateHRFileDto CreateHRFile)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
				return _responseModelHandler.GetResponseModel(CreateHRFile, "VALIDATION", StatusCodes.Status404NotFound, Lang);

			IGenericRepository<HRFile> HRFilesRepository = uof.GetRepository<HRFile>();

			HRFile newHRFile = mapper.Map<HRFile>(CreateHRFile);

            newHRFile.IsActive = true;
            newHRFile.CreateDate = DateTime.Now;

			await HRFilesRepository.InsertAsync(newHRFile, cts.Token);
			await uof.Commit();
			return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
		}

        // PUT: api/HRFile/5
        //[HttpPut("UpdateHRFile/{id}")]
        //public async Task<IResponseModel> UpdateHRFile(int id, [FromBody] UpdateHRFilesDto HRFiles)
        //{
        //	if (!ModelState.IsValid)
        //		return responseModelHandler.GetResponseModel(HRFiles, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        //	if (id != HRFiles.Id)
        //		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        //	IGenericRepository<HRFile> HRFileRepository = uof.GetRepository<HRFile>();

        //	HRFile updateHRFile = mapper.Map<UpdateHRFilesDto, HRFile>(HRFiles);
        //	updateHRFile.EditDate = DateTime.Now;

        //	HRFileRepository.Update(updateHRFile);
        //	await uof.Commit();

        //	return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
        //}

        // POST: api/HRFile
        // DELETE: api/HRFile/5
        //[HttpDelete("DeleteHRFile/{id}")]
        //public async Task<IResponseModel> DeleteHRFile([FromRoute] int id)
        //{
        //	IGenericRepository<HRFile> HRFileRepository = uof.GetRepository<HRFile>();

        //	HRFile currentHRFiles = await HRFileRepository.GetByIdAsync(id);

        //	if (currentHRFiles is null)
        //		return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        //	currentHRFiles.DeleteDate = DateTime.Now;
        //	HRFileRepository.SoftDelete(currentHRFiles);
        //	await uof.Commit();

        //	return _responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        //}
    }
}