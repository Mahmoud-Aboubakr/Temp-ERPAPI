using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.Page;
using Application.Dtos.Setup.LookUps;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Setup
{
    public class PagesController : CommonBaseController
    {
        public PagesController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler, 
            IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options
            ) : base(uof, mapper, accessor, responseModelHandler, 
                paginatedModelHandler, options)
        {
        }


        // GET: api/Pages
        [HttpGet("GetPages")]
        public async Task<IResponseModel> GetPages([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();
            IGenericRepository<AppPage> pageRepository = uof.GetRepository<AppPage>();
            using (ISpecification<AppPage> specifications = new PagesSpec(paginationParams.PageSize, paginationParams.PageNumber))
            {
                IEnumerable<AppPage> pagesList = await pageRepository.GetAllAsync(specifications, cts.Token);
                IEnumerable<ReadAppPageDto> pages = mapper.Map<IEnumerable<ReadAppPageDto>>(pagesList);

                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(pages, paginationParams.PageNumber, paginationParams.PageSize, await pageRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);
                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);
                return responseModel;
            }
        }

        // GET: api/Pages/5
        [HttpGet("GetPage/{id}")]
        public async Task<IResponseModel> GetPage([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<AppPage> pageRepository = uof.GetRepository<AppPage>();
            using (ISpecification<AppPage> specifications = new PagesSpec(id))
            {
                AppPage page = await pageRepository.GetAsync(specifications, cts.Token);

                if (page is null)
                    return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

                return responseModelHandler.GetResponseModel(mapper.Map<ReadAppPageDto>(page), null, StatusCodes.Status200OK, Lang);
            }
        }

        // PUT: api/Pages/5
        [HttpPut("UpdatePage/{id}")]
        public async Task<IResponseModel> UpdatePage(int id, [FromBody] UpdateAppPageDto page)
        {

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(page, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            if (id != page.Id)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<AppPage> pageRepository = uof.GetRepository<AppPage>();

            AppPage updatePage = mapper.Map<UpdateAppPageDto, AppPage>(page);
            updatePage.EditDate = DateTime.Now;

            pageRepository.Update(updatePage);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
        }

        // POST: api/Pages
        [HttpPost("AddPage")]
        public async Task<IResponseModel> AddPage([FromBody] AddAppPageDto page)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
                return _responseModelHandler.GetResponseModel(page, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<AppPage> pageRepository = uof.GetRepository<AppPage>();

            AppPage newPage = mapper.Map<AppPage>(page);

            //newPage.IsActive = true;
            newPage.CreateDate = DateTime.Now;

            await pageRepository.InsertAsync(newPage, cts.Token);
            await uof.Commit();
            return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
        }

        // DELETE: api/Pages/5
        [HttpDelete("DeletePage/{id}")]
        public async Task<IResponseModel> DeletePage([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<AppPage> pageRepository = uof.GetRepository<AppPage>();

            AppPage currentPage = await pageRepository.GetByIdAsync(id, cts.Token);

            if (currentPage is null)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            currentPage.DeleteDate = DateTime.Now;
            pageRepository.SoftDelete(currentPage);
            await uof.Commit();

            return _responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
    }
}
