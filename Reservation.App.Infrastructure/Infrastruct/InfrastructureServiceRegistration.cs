using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Reservation.App.Application.Contracts.Infrastructure;
using Reservation.App.Infrastructure.Infrastruct.Captcha;
using Reservation.App.Infrastructure.Infrastruct.Email;
using SendGrid.Extensions.DependencyInjection;

namespace Reservation.App.Infrastructure.Infrastruct;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddTurnstileValidator(configuration);

        services.AddSendgrid(configuration);

        return services;
    }
}
