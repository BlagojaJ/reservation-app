using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Reservation.App.Application.Contracts.Identity;
using Reservation.App.Application.Models.Authentication;
using Reservation.App.Infrastructure.Identity.Models;
using Reservation.App.Infrastructure.Identity.Services;
using Reservation.App.Infrastructure.Persistence;

namespace Reservation.App.Infrastructure.Identity
{
    public static class IdentityServiceRegistration
    {
        public static void AddIdentityServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Lockout.AllowedForNewUsers = false;
                })
                .AddEntityFrameworkStores<ReservationDbContext>();

            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)
                        ),
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                    };
                });
        }
    }
}
