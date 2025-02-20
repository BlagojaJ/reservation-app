using System.Text.Json.Serialization;

namespace Reservation.App.Application.Responses;

public class PaginatedResponse<T> : SuccessResponse<T>
    where T : class
{
    [JsonPropertyOrder(1)]
    public PaginationMetadata Metadata { get; set; } = default!;
}
