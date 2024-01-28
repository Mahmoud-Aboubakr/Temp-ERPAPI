using System.Net;
using System.Text.Json;
using Application;

namespace Api;

public class ExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly IHostEnvironment env;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
    {
        this.next = next;
        this.env = env;
    }

    public async Task InvokeAsync(HttpContext content)
    {
        try
        {
            await next(content);
        }
        catch (Exception ex)
        {
            content.Response.ContentType = "application/json";
            content.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ApiException exception = new ApiException(content.Response.StatusCode, ex.Message, "Internal Server Error!");

            if (env.IsDevelopment())
                exception.Details = ex.StackTrace?.ToString();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(exception, options);

            await content.Response.WriteAsync(json);
        }
    }
}
