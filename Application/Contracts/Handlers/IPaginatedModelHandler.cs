using Application.Contracts.Handlers;
using Application.Contracts.Models;

namespace Application;

public interface IPaginatedModelHandler : IResponseModel
{
	int CurrentPage { get; protected set; }
	int PageSize { get; protected set; }
	int TotalPages { get; protected set; }
	int TotalCount { get; protected set; }

	IPaginatedModelHandler Create<T>(IEnumerable<T> items, int pageNumber, int pageSize, int totalcount);
	IPaginatedModelHandler WithResponseModel(string message, int statusCode, string lang, string ErrorMessage = null);
}
