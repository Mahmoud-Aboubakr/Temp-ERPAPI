
namespace Application.Contracts.Models
{
    public interface IResponseModel
    {
        int StatusCode { get; set; }
        string Message { get; set; }
        object Data { get; set; }
        string ErrorMessage { get; set; }
        string Lang { get; set; }
    }
}
