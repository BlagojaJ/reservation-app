using System.Net;

namespace Reservation.App.Application.Responses;

public class ErrorResponse : BaseResponse
{
    public ErrorResponseMessage Error { get; set; }

    public ErrorResponse()
        : base(HttpStatusCode.InternalServerError)
    {
        Error = new() { Title = "Something went wrong!", Message = "Please try again later." };
    }
}
