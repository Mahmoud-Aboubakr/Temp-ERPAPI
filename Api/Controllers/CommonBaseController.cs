using Api.CustomConfiguration;
using Application;
using Microsoft.Extensions.Options;
using Persistence;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public abstract class CommonBaseController : ControllerBase
	{
		public string Lang { get; set; }
		protected readonly IUnitOfWork _uof, uof;
		protected readonly IMapper _mapper, mapper;
		protected readonly IResponseModelHandler _responseModelHandler, responseModelHandler;
		protected readonly IPaginatedModelHandler _paginatedModelHandler, paginatedModelHandler;
        protected CustomServiceConfiguration ServiceConfiguration { get; set; }

 

		public CommonBaseController(IUnitOfWork uof, IMapper mapper, 
			IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler, 
			IPaginatedModelHandler paginatedModelHandler , IOptions<CustomServiceConfiguration> options)

        {
			this.uof = _uof = uof;
			this.mapper = _mapper = mapper;
			Lang = accessor.HttpContext.Request.Headers["lang"].ToString() == "" ? "En" : accessor.HttpContext.Request.Headers["lang"].ToString();
			this.responseModelHandler = _responseModelHandler = responseModelHandler;
			this.paginatedModelHandler = _paginatedModelHandler = paginatedModelHandler;
            this.ServiceConfiguration = options.Value;

        }

        protected bool IsNullOrEmpty<T>(IEnumerable<T> lst)
		{
			return lst is null || !lst.Any();
		}

		//protected SpecificationBuilder<T> Specifications<T>() => new SpecificationBuilder<T>();

		protected void AddPaginationHeader(int currentPage, int pageSize, int totalCount)
		{
			int totalPages = (int)Math.Ceiling(totalCount / (1.0 * pageSize));
			Response.AddPaginationHeader(new PaginationHeader(currentPage, pageSize, totalCount, totalPages));
		}

        protected CancellationTokenSource GetCommandCancellationToken()
        {
            return new CancellationTokenSource(ServiceConfiguration.DefaultRequestTimeOutInMs);
        }
		/*FOR EXCEPTION COMMANDS TIME OUT EX long task query - report and so on*/
        protected CancellationTokenSource GetCommandCancellationToken(int timeOut)
        {
            return new CancellationTokenSource(timeOut);
        }
    }
}
