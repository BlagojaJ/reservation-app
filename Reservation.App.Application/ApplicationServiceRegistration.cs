using System.Reflection;
using Ganss.Xss;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reservation.App.Application.Behaviors;
using Reservation.App.Application.Contracts.Application;
using Reservation.App.Application.Models.Email;
using Reservation.App.Application.Services;

namespace Reservation.App.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        var mappingProfile = new MappingProfile(configuration);

        services.AddAutoMapper(
            cfg =>
            {
                cfg.AddProfile(mappingProfile);
                //  ...
            },
            Assembly.GetExecutingAssembly()
        );
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

        services.AddTransient<IPasswordGenerator, PasswordGenerator>();

        services.AddSingleton<HtmlSanitizer>();

        return services;
    }
}
