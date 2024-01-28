using Application;
using Application.Handlers;
using Application.Helpers;

namespace Api;

public class PagedList : IPaginatedModelHandler
{
	private IMessageHandler messageHandler { get; }
	public int CurrentPage { get; set; }
	public int PageSize { get; set; }
	public int TotalPages { get; set; }
	public int TotalCount { get; set; }
	public int StatusCode { get; set; }
	public string Message { get; set; }
	public object Data { get; set; }
	public string ErrorMessage { get; set; }
	public string Lang { get; set; }

	public PagedList(IMessageHandler messageHandler)
	{
		this.messageHandler = messageHandler;
	}

	public IPaginatedModelHandler Create<T>(IEnumerable<T> items, int pageNumber, int pageSize, int totalcount)
	{
		CurrentPage = pageNumber;
		PageSize = pageSize;
		TotalPages = (int)Math.Ceiling(totalcount / (1.0 * pageSize));
		TotalCount = totalcount;
		Data = items;

		return this;
	}

	public IPaginatedModelHandler WithResponseModel(string message, int statusCode, string lang, string errorMessage = null)
	{
		if (message is not null)
			Message = lang == "En" ? messageHandler.GetMessageValueEn(message) : messageHandler.GetMessageValueAr(message);
		StatusCode = statusCode;
		Lang = lang;
		ErrorMessage = errorMessage;
		return this;
	}
}
