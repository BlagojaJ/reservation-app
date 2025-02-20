using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Reservation.App.Application.Contracts.Infrastructure;

namespace Reservation.App.Infrastructure.Infrastruct.Captcha;

public static class CaptchaServiceRegistration
{
    public static IServiceCollection AddTurnstileValidator(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var turnstileSettingsConfig = configuration.GetSection("Turnstile");
        services.Configure<TurnstileSettings>(turnstileSettingsConfig);

        var turnstileSettings = turnstileSettingsConfig.Get<TurnstileSettings>();

        if (string.IsNullOrEmpty(turnstileSettings?.SiteVerifyUrl))
        {
            throw new NullReferenceException(
                "Turnstile SiteVerifyUrl is null. Please check the settings."
            );
        }
        if (string.IsNullOrEmpty(turnstileSettings?.SiteKey))
        {
            throw new NullReferenceException(
                "Turnstile SiteKey is null. Please check the settings."
            );
        }
        if (string.IsNullOrEmpty(turnstileSettings?.SecretKey))
        {
            throw new NullReferenceException(
                "Turnstile SecretKey is null. Please check the settings."
            );
        }

        services.AddHttpClient(
            "TurnstileClient",
            client =>
            {
                client.BaseAddress = new Uri(turnstileSettings.SiteVerifyUrl);
            }
        );

        services.AddScoped<ICaptchaValidator, TurnstileCaptchaValidator>();

        return services;
    }
}
