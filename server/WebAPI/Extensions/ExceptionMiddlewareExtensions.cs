using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Logic.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace WebAPI.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
            appError.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                switch (exception)
                {
                    case UnauthorizedAccessException _:
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(exception.Message));
                        break;
                    
                    case NotFoundException nfEx:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = nfEx.Code,
                            Message = nfEx.Message
                        }.ToString());
                        break;
                    
                    case NotAllowedException naEx:
                        context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = naEx.Code,
                            Message = naEx.Message
                        }.ToString());
                        break;
                    
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = exception?.Message ?? "Неизвестная ошибка."
                        }.ToString());
                        break;
                }
            })
        );
    }
}

public class ErrorDetails
{
    public int StatusCode { get; set; }

    public string? Message { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}