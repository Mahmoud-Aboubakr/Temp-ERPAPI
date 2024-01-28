using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.Nationality;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Setup
{
    public class NationalityController : CommonBaseController
    {
        public NationalityController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler, 
            IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options
            ) : base(uof, mapper, accessor, responseModelHandler, 
                paginatedModelHandler, options)
        {
        }

        // GET: api/Nationality
        [HttpGet("GetNationalities")]
        public async Task<IResponseModel> GetNationalities([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<Nationality> cityRepository = uof.GetRepository<Nationality>();
            using (ISpecification<Nationality> specifications = new NationalitySpec(paginationParams.PageSize, paginationParams.PageNumber))
            {
                IEnumerable<ReadNationalityDto> cities = mapper.Map<IEnumerable<ReadNationalityDto>>(await cityRepository.GetAllAsync(specifications, cts.Token));

                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(cities, paginationParams.PageNumber, paginationParams.PageSize, await cityRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

                return responseModel;
            }
        }

        // GET: api/Nationality/5
        [HttpGet("GetNationality/{id}")]
        public async Task<IResponseModel> GetNationality([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<Nationality> nationalityRepository = uof.GetRepository<Nationality>();
            using (ISpecification<Nationality> specifications = new NationalitySpec(id))
            {
                Nationality nationality = await nationalityRepository.GetAsync(specifications, cts.Token);

                if (nationality is null)
                    return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

                return responseModelHandler.GetResponseModel(mapper.Map<ReadNationalityDto>(nationality), null, StatusCodes.Status200OK, Lang);
            }
        }

        // PUT: api/Nationality/5
        [HttpPut("UpdateNationality/{id}")]
        public async Task<IResponseModel> UpdateNationality(int id, [FromBody] UpdateNationalityDto nationality)
        {

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(nationality, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            if (id != nationality.Id)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Nationality> nationalityRepository = uof.GetRepository<Nationality>();

            Nationality updateNationality = mapper.Map<UpdateNationalityDto, Nationality>(nationality);
            updateNationality.EditDate = DateTime.Now;

            nationalityRepository.Update(updateNationality);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
        }

        // POST: api/Nationality
        [HttpPost("AddNationality")]
        public async Task<IResponseModel> AddNationality([FromBody] AddNationalityDto nationality)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
                return _responseModelHandler.GetResponseModel(nationality, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Nationality> nationalityRepository = uof.GetRepository<Nationality>();

            Nationality newNationality = mapper.Map<Nationality>(nationality);

            newNationality.IsActive = true;
            newNationality.CreateDate = DateTime.Now;

            await nationalityRepository.InsertAsync(newNationality, cts.Token);
            await uof.Commit();
            return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
        }

        // DELETE: api/Nationality/5
        [HttpDelete("DeleteNationality/{id}")]
        public async Task<IResponseModel> DeleteNationality([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<Nationality> nationalityRepository = uof.GetRepository<Nationality>();

            Nationality currentNationality = await nationalityRepository.GetByIdAsync(id, cts.Token);

            if (currentNationality is null)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            currentNationality.DeleteDate = DateTime.Now;
            nationalityRepository.SoftDelete(currentNationality);
            await uof.Commit();

            return _responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
    }
}
