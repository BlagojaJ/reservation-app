using MediatR;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Reservations.Commands.ChangeReservationStatus;

public class ChangeReservationStatusCommand : IRequest
{
    public int Id { get; set; }
    public ReservationStatusEnum Status { get; set; }
}
