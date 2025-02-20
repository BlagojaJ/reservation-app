using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Reservations.Queries.GetReservationWithPayments;

public class ReservationWithPaymentsPaymentDto
{
    public int ID { get; set; }

    // Financial details
    public double Amount { get; set; }
    public CurrencyEnum Currency { get; set; } = CurrencyEnum.MKD;

    // Payment tracking
    public PaymentStatusEnum Status { get; set; } = PaymentStatusEnum.Pending;
}
