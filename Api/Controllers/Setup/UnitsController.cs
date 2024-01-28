using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.Unit;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Setup
{
    public class UnitsController : CommonBaseController
    {
        public UnitsController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler,
            IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options
            ) : base(uof, mapper, accessor, responseModelHandler,
                paginatedModelHandler, options)
        {
        }

        // GET: api/Units
        [HttpGet("GetUnits")]
        public async Task<IResponseModel> GetUnits([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<Unit> cityRepository = uof.GetRepository<Unit>();
            using (ISpecification<Unit> specifications = new UnitSpec(paginationParams.PageSize, paginationParams.PageNumber))
            {
                IEnumerable<ReadUnitDto> units = mapper.Map<IEnumerable<ReadUnitDto>>(await cityRepository.GetAllAsync(specifications, cts.Token));

                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(units, paginationParams.PageNumber, paginationParams.PageSize, await cityRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

                return responseModel;
            }
        }

        // GET: api/Units/5
        [HttpGet("GetUnit/{id}")]
        public async Task<IResponseModel> GetUnit([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<Unit> unitRepository = uof.GetRepository<Unit>();
            using (ISpecification<Unit> specifications = new UnitSpec(id))
            {
                Unit unit = await unitRepository.GetAsync(specifications, cts.Token);

                if (unit is null)
                    return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

                return responseModelHandler.GetResponseModel(mapper.Map<ReadUnitDto>(unit), null, StatusCodes.Status200OK, Lang);
            }
        }

        // PUT: api/Units/5
        [HttpPut("UpdateUnit/{id}")]
        public async Task<IResponseModel> UpdateUnit(int id, [FromBody] UpdateUnitDto unit)
        {

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(unit, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            if (id != unit.Id)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Unit> unitRepository = uof.GetRepository<Unit>();

            Unit updateUnit = mapper.Map<UpdateUnitDto, Unit>(unit);
            updateUnit.EditDate = DateTime.Now;

            unitRepository.Update(updateUnit);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
        }

        // POST: api/Units
        [HttpPost("AddUnit")]
        public async Task<IResponseModel> AddUnit([FromBody] AddUnitDto unit)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
                return _responseModelHandler.GetResponseModel(unit, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Unit> unitRepository = uof.GetRepository<Unit>();

            Unit newUnit = mapper.Map<Unit>(unit);

            //newUnit.IsActive = true;
            newUnit.CreateDate = DateTime.Now;

            await unitRepository.InsertAsync(newUnit, cts.Token);
            await uof.Commit();
            return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
        }

        // DELETE: api/Units/5
        [HttpDelete("DeleteUnit/{id}")]
        public async Task<IResponseModel> DeleteUnit([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<Unit> unitRepository = uof.GetRepository<Unit>();

            Unit currentUnit = await unitRepository.GetByIdAsync(id, cts.Token);

            if (currentUnit is null)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            currentUnit.DeleteDate = DateTime.Now;
            unitRepository.SoftDelete(currentUnit);
            await uof.Commit();

            return _responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
    }
}
