using System.Text.Json.Serialization;

namespace Reservation.App.Domain.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmailStatusEnum
{
    [JsonPropertyName("undefined")]
    Undefined,

    [JsonPropertyName("processed")]
    Processed,

    [JsonPropertyName("dropped")]
    Dropped,

    [JsonPropertyName("deferred")]
    Deferred,

    [JsonPropertyName("bounce")]
    Bounce,

    [JsonPropertyName("delivered")]
    Delivered,
}
