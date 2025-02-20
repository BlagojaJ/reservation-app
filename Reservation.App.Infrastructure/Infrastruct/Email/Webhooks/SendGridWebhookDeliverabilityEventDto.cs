using System.Text.Json.Serialization;

namespace Reservation.App.Infrastructure.Infrastruct.Email.Webhooks;

public class SendGridWebhookDeliverabilityEventDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("event")]
    public string Event { get; set; } = string.Empty;

    // public string SmtpId { get; set; } = string.Empty;
    // public string Ip { get; set; } = string.Empty;
    [JsonPropertyName("sg_event_id")]
    public string SgEventId { get; set; } = string.Empty;

    [JsonPropertyName("sg_message_id")]
    public string SgMessageId { get; set; } = string.Empty;
    // public string Reason { get; set; } = string.Empty;
    // public string Status { get; set; } = string.Empty;
    // public string Response { get; set; } = string.Empty;

    // public string Tls { get; set; } = string.Empty;

    // public string Category { get; set; } = string.Empty;
    // public string UniqueArgs { get; set; } = string.Empty;
    // public string Attempt { get; set; } = string.Empty;
    // public string BounceClassification { get; set; } = string.Empty;
}
