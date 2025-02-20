using System.Net;
using System.Text.Json;
using Reservation.App.Application.Exceptions;
using Reservation.App.Application.Responses;

namespace Reservation.App.Api.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await TypeConverterException(context, exception);
        }
    }

    private static Task TypeConverterException(HttpContext context, Exception exception)
    {
        var response = new ErrorResponse();

        switch (exception)
        {
            case ValidationException validationException:
                response.Code = HttpStatusCode.BadRequest;
                response.Error.Title = "Invalid input data";
                response.Error.Message = JsonSerializer.Serialize(
                    validationException.ValidationErrors
                );
                break;
            case BadRequestException badRequestException:
                response.Code = HttpStatusCode.BadRequest;
                response.Error.Title = "Invalid syntax for this request was provided.";
                response.Error.Message = badRequestException.Message;
                break;
            case NotFoundException notFoundException:
                response.Code = HttpStatusCode.NotFound;
                response.Error.Title = "We could not find the resource";
                response.Error.Message = notFoundException.Message;
                break;
            case InternalServerException internalServerException:
                response.Code = HttpStatusCode.InternalServerError;
                response.Error.Title = "Internal server error";
                response.Error.Message = internalServerException.Message;
                break;
            case ServiceUnavailableException serviceUnavailableException:
                response.Code = HttpStatusCode.ServiceUnavailable;
                response.Error.Title = "Service unavailable error";
                response.Error.Message = serviceUnavailableException.Message;
                break;
            case UnprocessableEntityException unprocessableEntityException:
                response.Code = HttpStatusCode.UnprocessableEntity;
                response.Error.Title = "Unprocessable entity error";
                response.Error.Message = unprocessableEntityException.Message;
                break;
            case UnauthorizedException unauthorizedException:
                response.Code = HttpStatusCode.Unauthorized;
                response.Error.Title = "Unauthorized";
                response.Error.Message = unauthorizedException.Message;
                break;

            case Exception:
                response.Code = HttpStatusCode.InternalServerError;
                response.Error.Title = "Unhandled operational exception";
                response.Error.Message = exception.Message;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.Code;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
