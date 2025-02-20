using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reservation.App.Application.Contracts.Infrastructure;
using SendGrid.Extensions.DependencyInjection;
using SendGrid.Helpers.EventWebhook;

namespace Reservation.App.Infrastructure.Infrastruct.Email;

public static class EmailServiceRegistration
{
    public static IServiceCollection AddSendgrid(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var sendGridSettingConfig = configuration.GetSection("SendGridSettings");
        services.Configure<SendGridSettings>(sendGridSettingConfig);

        var sendGridSettings = sendGridSettingConfig.Get<SendGridSettings>();

        if (sendGridSettings == null)
        {
            throw new NullReferenceException(
                "SendGridSettings is null. Please check the settings."
            );
        }
        if (string.IsNullOrEmpty(sendGridSettings.ApiKey))
        {
            throw new NullReferenceException("SendGrid ApiKey is null. Please check the settings.");
        }
        if (string.IsNullOrEmpty(sendGridSettings.PublicWebhookVerificationKey))
        {
            throw new NullReferenceException(
                "SendGrid PublicWebhookVerificationKey is null. Please check the settings."
            );
        }

        services.AddSendGrid(options =>
        {
            options.ApiKey = sendGridSettings.ApiKey;
        });
        services.AddScoped<RequestValidator>();
        services.AddScoped<IEmailService, SendGridEmailService>();

        return services;
    }
}
