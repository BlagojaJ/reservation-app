using MediatR;

namespace Reservation.App.Application.Features.Payments.Commands.CreatePaymentForReservation;

public class CreatePaymentForReservationCommand : IRequest
{
    public int ReservationId { get; set; }
    public double Amount { get; set; }
}
