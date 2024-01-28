using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.Setup.StoreAdjustment;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Setup
{
    public class StoreAdjustmentController : CommonBaseController
    {
        public StoreAdjustmentController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler,
            IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options
            ) : base(uof, mapper, accessor, responseModelHandler,
                paginatedModelHandler, options)
        {
        }

        // GET: api/StoreAdjustments
        [HttpGet("GetStoreAdjustments")]
        public async Task<IResponseModel> GetStoreAdjustments([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<StoreAdjustment> cityRepository = uof.GetRepository<StoreAdjustment>();
            using (ISpecification<StoreAdjustment> specifications = new StoreAdjustmentSpec(paginationParams.PageSize, paginationParams.PageNumber))
            {
                IEnumerable<ReadStoreAdjustmentDto> storeAdjustments = mapper.Map<IEnumerable<ReadStoreAdjustmentDto>>(await cityRepository.GetAllAsync(specifications, cts.Token));

                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(storeAdjustments, paginationParams.PageNumber, paginationParams.PageSize, await cityRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

                return responseModel;
            }
        }

        // GET: api/StoreAdjustments/5
        [HttpGet("GetStoreAdjustment/{id}")]
        public async Task<IResponseModel> GetStoreAdjustment([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<StoreAdjustment> storeAdjustmentRepository = uof.GetRepository<StoreAdjustment>();
            using (ISpecification<StoreAdjustment> specifications = new StoreAdjustmentSpec(id))
            {
                StoreAdjustment storeAdjustment = await storeAdjustmentRepository.GetAsync(specifications, cts.Token);

                if (storeAdjustment is null)
                    return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

                return responseModelHandler.GetResponseModel(mapper.Map<ReadStoreAdjustmentDto>(storeAdjustment), null, StatusCodes.Status200OK, Lang);
            }
        }

        // PUT: api/StoreAdjustments/5
        [HttpPut("UpdateStoreAdjustment/{id}")]
        public async Task<IResponseModel> UpdateStoreAdjustment(int id, [FromBody] UpdateStoreAdjustmentDto updateStoreAdjustmentDto)
        {

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(updateStoreAdjustmentDto, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            if (id != updateStoreAdjustmentDto.Id)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<StoreAdjustment> storeAdjustmentRepository = uof.GetRepository<StoreAdjustment>();

            StoreAdjustment updateStoreAdjustment = mapper.Map<UpdateStoreAdjustmentDto, StoreAdjustment>(updateStoreAdjustmentDto);
            updateStoreAdjustment.EditDate = DateTime.Now;

            storeAdjustmentRepository.Update(updateStoreAdjustment);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
        }

        // POST: api/StoreAdjustments
        [HttpPost("AddStoreAdjustment")]
        public async Task<IResponseModel> AddStoreAdjustment([FromBody] AddStoreAdjustmentDto addStoreAdjustmentDto)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
                return _responseModelHandler.GetResponseModel(addStoreAdjustmentDto, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<StoreAdjustment> storeAdjustmentRepository = uof.GetRepository<StoreAdjustment>();

            StoreAdjustment newStoreAdjustment = mapper.Map<StoreAdjustment>(addStoreAdjustmentDto);

            newStoreAdjustment.IsActive = true;
            newStoreAdjustment.CreateDate = DateTime.Now;

            await storeAdjustmentRepository.InsertAsync(newStoreAdjustment, cts.Token);
            await uof.Commit();
            return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
        }

        // DELETE: api/StoreAdjustments/5
        [HttpDelete("DeleteStoreAdjustment/{id}")]
        public async Task<IResponseModel> DeleteStoreAdjustment([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<StoreAdjustment> storeAdjustmentRepository = uof.GetRepository<StoreAdjustment>();

            StoreAdjustment currentStoreAdjustment = await storeAdjustmentRepository.GetByIdAsync(id, cts.Token);

            if (currentStoreAdjustment is null)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            currentStoreAdjustment.DeleteDate = DateTime.Now;
            storeAdjustmentRepository.SoftDelete(currentStoreAdjustment);
            await uof.Commit();

            return _responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
    }
}
