using MediatR;

namespace Reservation.App.Application.Features.Payments.Queries.GetPaymentsForReservation;

public class GetPaymentsForReservationQuery : IRequest<GetPaymentsForReservationQueryResponse>
{
    public int ReservationId { get; set; }
}
