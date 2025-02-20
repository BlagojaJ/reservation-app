using Reservation.App.Domain.Entities;

namespace Reservation.App.Application.Contracts.Persistence;

public interface IPaymentRepository : IAsyncRepository<Payment>
{
    Task<List<Payment>> GetPaymentsWithEmailsForReservationAsync(int reservationId);
}
