using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Reservation.App.Api.Middleware;
using Reservation.App.Application;
using Reservation.App.Infrastructure.Identity;
using Reservation.App.Infrastructure.Identity.Models;
using Reservation.App.Infrastructure.Identity.Seed;
using Reservation.App.Infrastructure.Infrastruct;
using Reservation.App.Persistence;
using Serilog;

namespace Reservation.App.Api;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        AddSwagger(builder.Services);

        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddPersistenceServices(builder.Configuration);
        builder.Services.AddIdentityServices(builder.Configuration);
        builder.Services.AddInfrastructureServices(builder.Configuration);

        builder.Services.AddControllers();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "local",
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
            );
            options.AddPolicy(
                "production",
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
            );
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("local");
        }

        if (app.Environment.IsProduction())
        {
            app.UseHttpsRedirection();

            app.UseCors("production");
        }

        app.UseAuthentication();

        app.UseCustomExceptionHandler();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Description =
                        @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                }
            );

            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
                }
            );
        });
    }

    public static async Task AddInitialUser(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            if (userManager != null)
            {
                await UserCreator.SeedAsync(userManager);
            }
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<Serilog.ILogger>();
            logger.Error(ex, "An error occurred while adding initial ApplicationUser");
        }
    }
}
