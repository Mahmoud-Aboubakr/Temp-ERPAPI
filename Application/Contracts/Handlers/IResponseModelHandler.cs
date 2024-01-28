using Application.Contracts.Models;

namespace Application.Contracts.Handlers
{
	public interface IResponseModelHandler
	{
		IResponseModel GetResponseModel(object data, string message, int statusCode, string lang, string ErrorMessage = null);
	}
}
