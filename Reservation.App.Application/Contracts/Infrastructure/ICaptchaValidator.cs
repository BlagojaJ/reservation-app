namespace Reservation.App.Application.Contracts.Infrastructure;

public interface ICaptchaValidator
{
    Task<bool> ValidateTokenAsync(string token, string? remoteIp = null);
}
