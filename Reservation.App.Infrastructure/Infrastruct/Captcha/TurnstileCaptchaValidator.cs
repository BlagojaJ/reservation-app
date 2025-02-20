using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reservation.App.Application.Contracts.Infrastructure;

namespace Reservation.App.Infrastructure.Infrastruct.Captcha;

public class TurnstileCaptchaValidator(
    IHttpClientFactory httpClientFactory,
    IOptions<TurnstileSettings> turnstileSettings,
    ILogger<TurnstileCaptchaValidator> logger
) : ICaptchaValidator
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("TurnstileClient");
    private readonly TurnstileSettings _turnstileSettings = turnstileSettings.Value;
    private readonly ILogger<TurnstileCaptchaValidator> _logger = logger;

    public async Task<bool> ValidateTokenAsync(string token, string? remoteIp = null)
    {
        var validationRequest = new TurnstileValidationRequest
        {
            Secret = _turnstileSettings.SecretKey,
            Response = token,
            RemoteIp = remoteIp,
        };

        // NOTE: Turnstile SiteVerify always returns 200OK even if the token is not valid
        HttpResponseMessage? response;
        try
        {
            response = await _httpClient.PostAsJsonAsync("", validationRequest);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            // Catch network or other unexpected errors
            _logger.LogError(ex, "Unexpected error while verifying token. Token: {token}", token);
            return false;
        }

        var validationResponse =
            await response.Content.ReadFromJsonAsync<TurnstileValidationResponse>();

        if (validationResponse != null && !validationResponse.Success)
        {
            _logger.LogError(
                "Failed to verify token. Token: {token}, Errors: {@errors}",
                token,
                validationResponse.ErrorCodes
            );
        }

        return validationResponse?.Success ?? false;
    }
}
