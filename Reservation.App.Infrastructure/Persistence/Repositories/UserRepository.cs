using Microsoft.EntityFrameworkCore;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Domain.Entities;

namespace Reservation.App.Infrastructure.Persistence.Repositories;

public class UserRepository(ReservationDbContext dbContext)
    : BaseRepository<User>(dbContext),
        IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        // Added emails are in lowercase
        return await _dbContext.ReservationUsers.FirstOrDefaultAsync(u => u.Email == email);
    }
}
