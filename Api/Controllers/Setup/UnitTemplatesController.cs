using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.Setup.UnitTemplate;
using Application.Dtos.UnitTemplate;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Setup
{
    public class UnitTemplatesController : CommonBaseController
    {
        public UnitTemplatesController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler,
            IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options
            ) : base(uof, mapper, accessor, responseModelHandler,
                paginatedModelHandler, options)
        {
        }

        // GET: api/UnitTemplates
        [HttpGet("GetUnitTemplates")]
        public async Task<IResponseModel> GetUnitTemplates([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<UnitTemplate> cityRepository = uof.GetRepository<UnitTemplate>();
            using (ISpecification<UnitTemplate> specifications = new UnitTemplateSpec(paginationParams.PageSize, paginationParams.PageNumber))
            {
                IEnumerable<ReadUnitTemplateDto> unitTemplates = mapper.Map<IEnumerable<ReadUnitTemplateDto>>(await cityRepository.GetAllAsync(specifications, cts.Token));

                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(unitTemplates, paginationParams.PageNumber, paginationParams.PageSize, await cityRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

                return responseModel;
            }
        }

        // GET: api/UnitTemplate/5
        [HttpGet("GetUnitTemplate/{id}")]
        public async Task<IResponseModel> GetUnitTemplate([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<UnitTemplate> unitTemplateRepository = uof.GetRepository<UnitTemplate>();
            using (ISpecification<UnitTemplate> specifications = new UnitTemplateSpec(id))
            {
                UnitTemplate unitTemplate = await unitTemplateRepository.GetAsync(specifications, cts.Token);

                if (unitTemplate is null)
                    return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

                return responseModelHandler.GetResponseModel(mapper.Map<ReadUnitTemplateDto>(unitTemplate), null, StatusCodes.Status200OK, Lang);
            }
        }

        // PUT: api/UnitTemplate/5
        [HttpPut("UpdateUnitTemplate/{id}")]
        public async Task<IResponseModel> UpdateUnitTemplate(int id, [FromBody] UpdateUnitTemplateDto updateUnitTemplateDto)
        {

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(updateUnitTemplateDto, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            if (id != updateUnitTemplateDto.Id)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<UnitTemplate> unitTemplateRepository = uof.GetRepository<UnitTemplate>();

            UnitTemplate updateUnitTemplate = mapper.Map<UpdateUnitTemplateDto, UnitTemplate>(updateUnitTemplateDto);
            updateUnitTemplate.EditDate = DateTime.Now;

            unitTemplateRepository.Update(updateUnitTemplate);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
        }

        // POST: api/UnitTemplate
        [HttpPost("AddUnitTemplate")]
        public async Task<IResponseModel> AddUnitTemplate([FromBody] AddUnitTemplateDto addUnitTemplateDto)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
                return _responseModelHandler.GetResponseModel(addUnitTemplateDto, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<UnitTemplate> unitTemplateRepository = uof.GetRepository<UnitTemplate>();

            UnitTemplate newUnitTemplate = mapper.Map<UnitTemplate>(addUnitTemplateDto);

            newUnitTemplate.IsActive = true;
            newUnitTemplate.CreateDate = DateTime.Now;

            await unitTemplateRepository.InsertAsync(newUnitTemplate, cts.Token);
            await uof.Commit();
            return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
        }

        // DELETE: api/UnitTemplate/5
        [HttpDelete("DeleteUnitTemplate/{id}")]
        public async Task<IResponseModel> DeleteUnitTemplate([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<UnitTemplate> unitTemplateRepository = uof.GetRepository<UnitTemplate>();

            UnitTemplate currentUnitTemplate = await unitTemplateRepository.GetByIdAsync(id, cts.Token);

            if (currentUnitTemplate is null)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            currentUnitTemplate.DeleteDate = DateTime.Now;
            unitTemplateRepository.SoftDelete(currentUnitTemplate);
            await uof.Commit();

            return _responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
    }
}
