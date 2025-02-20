namespace Reservation.App.Infrastructure.Infrastruct.Email;

public class SendGridSettings
{
    public required string ApiKey { get; set; }
    public required string PublicWebhookVerificationKey { get; set; }
    public bool IsSandboxModeEnabled { get; set; } = true;
}
