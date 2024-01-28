using Application.Contracts.Models;

namespace Application.Models
{
    public class ResponseModel : IResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string ErrorMessage { get; set; }
        public string Lang { get; set; }
    }
}
