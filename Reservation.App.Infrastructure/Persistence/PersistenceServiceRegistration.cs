using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Infrastructure.Persistence;
using Reservation.App.Infrastructure.Persistence.Repositories;

namespace Reservation.App.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<ReservationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DbConnectionString"))
        );

        services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IEmailRepository, EmailRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IAgentRepository, AgentRepository>();

        return services;
    }
}
