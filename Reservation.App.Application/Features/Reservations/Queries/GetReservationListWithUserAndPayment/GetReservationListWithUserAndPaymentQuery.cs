using MediatR;
using Reservation.App.Application.RequestHelpers;

namespace Reservation.App.Application.Features.Reservations.Queries.GetReservationListWithUserAndPayment;

public class GetReservationListWithUserAndPaymentQuery
    : IRequest<GetReservationListWithUserAndPaymentQueryResponse>
{
    public PaginationParameters PaginationParameters { get; set; } = default!;
    public SearchParameters SearchParameters { get; set; } = default!;
    public SortParameters SortParameters { get; set; } = default!;
    public FilterParameters FilterParameters { get; set; } = default!;
}
