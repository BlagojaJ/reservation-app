using System.Text.Json.Serialization;

namespace Reservation.App.Infrastructure.Infrastruct.Captcha;

public class TurnstileValidationRequest
{
    [JsonPropertyName("secret")]
    public required string Secret { get; set; }

    [JsonPropertyName("response")]
    public required string Response { get; set; }

    // cSpell: disable-next-line
    [JsonPropertyName("remoteip")]
    public string? RemoteIp { get; set; }
}
