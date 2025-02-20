using Reservation.App.Domain.Entities;

namespace Reservation.App.Application.Contracts.Persistence;

public interface IUserRepository : IAsyncRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
