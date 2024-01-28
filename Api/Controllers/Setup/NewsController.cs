using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.News;
using Application.Specifications.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Setup
{

    public class NewsController : CommonBaseController
	{

		public NewsController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor, 
			IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler,
            IOptions<CustomServiceConfiguration> options) : 
			base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler,options)
		{
		}

		// GET: api/New
		[HttpGet("GetNews")]
		public async Task<IResponseModel> GetNews([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<New> newRepository = uof.GetRepository<New>();
			using (ISpecification<New> specifications = new NewsSpec(paginationParams.PageSize, paginationParams.PageNumber))
			{
				IEnumerable<ReadNewsDto> news = mapper.Map<IEnumerable<ReadNewsDto>>(await newRepository.GetAllAsync(specifications, cts.Token));

				IPaginatedModelHandler responseModel = paginatedModelHandler.Create(news, paginationParams.PageNumber, paginationParams.PageSize, await newRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

				AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

				return responseModel;
			}
		}

		// GET: api/New/5
		[HttpGet("GetNew/{id}")]
		public async Task<IResponseModel> GetNew([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<New> newRepository = uof.GetRepository<New>();
			using (ISpecification<New> specifications = new NewsSpec(id))
			{
				New newLetter = await newRepository.GetAsync(specifications, cts.Token);

				if (newLetter is null)
					return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

				return responseModelHandler.GetResponseModel(mapper.Map<ReadNewsDto>(newLetter), null, StatusCodes.Status200OK, Lang);
			}
		}

        // PUT: api/New/5
        [HttpPut("UpdateNew/{id}")]
		public async Task<IResponseModel> UpdateNew(int id, [FromBody] UpdateNewsDto news)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
				return responseModelHandler.GetResponseModel(news, "VALIDATION", StatusCodes.Status404NotFound, Lang);

			if (id != news.Id)
				return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

			IGenericRepository<New> newRepository = uof.GetRepository<New>();

			New updateNew = mapper.Map<UpdateNewsDto, New>(news);
			updateNew.EditDate = DateTime.Now;

			newRepository.Update(updateNew);
			await uof.Commit();

			return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
		}

		// POST: api/New
		[HttpPost("AddNew")]
		public async Task<IResponseModel> AddNew([FromBody] AddNewsDto news)
        {
            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
				return _responseModelHandler.GetResponseModel(news, "VALIDATION", StatusCodes.Status404NotFound, Lang);

			IGenericRepository<New> newsRepository = uof.GetRepository<New>();

			New newNews = mapper.Map<New>(news);

			newNews.IsActive = true;
			newNews.CreateDate = DateTime.Now;

			await newsRepository.InsertAsync(newNews, cts.Token);
			await uof.Commit();
			return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
		}

		// DELETE: api/New/5
		[HttpDelete("DeleteNew/{id}")]
		public async Task<IResponseModel> DeleteNew([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<New> newRepository = uof.GetRepository<New>();

			New currentNews = await newRepository.GetByIdAsync(id, cts.Token);

			if (currentNews is null)
				return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

			currentNews.DeleteDate = DateTime.Now;
			newRepository.SoftDelete(currentNews);
			await uof.Commit();

			return _responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

		}
	}
}