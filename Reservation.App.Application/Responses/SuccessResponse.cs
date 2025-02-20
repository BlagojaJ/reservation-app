using System.Text.Json.Serialization;

namespace Reservation.App.Application.Responses;

public class SuccessResponse<T> : BaseResponse
    where T : class
{
    [JsonPropertyOrder(2)]
    public T Data { get; set; } = default!;
}
