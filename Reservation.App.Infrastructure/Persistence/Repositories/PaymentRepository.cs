using Microsoft.EntityFrameworkCore;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Domain.Entities;

namespace Reservation.App.Infrastructure.Persistence.Repositories;

public class PaymentRepository(ReservationDbContext dbContext)
    : BaseRepository<Payment>(dbContext),
        IPaymentRepository
{
    public Task<List<Payment>> GetPaymentsWithEmailsForReservationAsync(int reservationId)
    {
        return _dbContext
            .Payments.Where(p => p.ReservationId == reservationId)
            .Include(p => p.Emails)
            .ToListAsync();
    }
}
