using MediatR;

namespace Reservation.App.Application.Features.Reservations.Queries.GetReservationWithPayments;

public class GetReservationWithPaymentsQuery : IRequest<GetReservationWithPaymentsQueryResponse>
{
    public int Id { get; set; }
    public string Password { get; set; } = string.Empty;
}
