using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Reservation.App.Application;

public class MappingProfile : Profile
{
    private readonly IConfiguration? _configuration;

    public MappingProfile() { }

    public MappingProfile(IConfiguration configuration)
    {
        _configuration = configuration;

        // Mapping for Reservations, Payments and Users
    }
}
