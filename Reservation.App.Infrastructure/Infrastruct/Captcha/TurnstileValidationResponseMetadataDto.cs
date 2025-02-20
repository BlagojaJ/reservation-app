using System.Text.Json.Serialization;

namespace Reservation.App.Infrastructure.Infrastruct.Captcha;

public class TurnstileValidationResponseMetadataDto
{
    [JsonPropertyName("ephemeral_id")]
    public string EphemeralId { get; set; } = string.Empty;
}
