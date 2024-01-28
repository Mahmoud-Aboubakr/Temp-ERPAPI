using Application.Contracts.Handlers;
using Application.Contracts.Models;
using Application.Helpers;

namespace Application.Handlers
{
    public class ResponseModelHandler : IResponseModelHandler
    {
        private readonly IResponseModel _responseModel;
        private readonly IMessageHandler _messageHandler;
        public ResponseModelHandler(IResponseModel responseModel, IMessageHandler messageHandler)
        {
            _responseModel = responseModel;
            _messageHandler = messageHandler;
        }
        public IResponseModel GetResponseModel(object data, string message, int statusCode, string lang, string ErrorMessage = null)
        {
            _responseModel.Data = data;
            if (message != null)
            {
                _responseModel.Message = lang == "En" ? _messageHandler.GetMessageValueEn(message) : _messageHandler.GetMessageValueAr(message);
            }
            _responseModel.StatusCode = statusCode;
            _responseModel.Lang = lang;
            _responseModel.ErrorMessage = ErrorMessage;
            return _responseModel;
        }
    }
}