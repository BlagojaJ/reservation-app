using System.Text.Json.Serialization;

namespace Reservation.App.Infrastructure.Infrastruct.Captcha;

public class TurnstileValidationResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("challenge_ts")]
    public DateTime ChallengeTimestamp { get; set; }

    [JsonPropertyName("hostname")]
    public string Hostname { get; set; } = string.Empty;

    [JsonPropertyName("error-codes")]
    public List<string> ErrorCodes { get; set; } = [];

    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty;

    [JsonPropertyName("cdata")]
    public string Cdata { get; set; } = string.Empty;

    [JsonPropertyName("metadata")]
    public TurnstileValidationResponseMetadataDto Metadata { get; set; } = new();
}
