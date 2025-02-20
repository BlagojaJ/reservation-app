namespace Reservation.App.Infrastructure.Infrastruct.Captcha;

public class TurnstileSettings
{
    public required string SiteVerifyUrl { get; set; }
    public required string SiteKey { get; set; }
    public required string SecretKey { get; set; }
}
