using AutoMapper;
using MediatR;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Application.Responses;

namespace Reservation.App.Application.Features.Reservations.Queries.GetReservationListWithUserAndPayment;

public class GetReservationListWithUserAndPaymentQueryHandler(
    IMapper mapper,
    IReservationRepository reservationRepository
)
    : IRequestHandler<
        GetReservationListWithUserAndPaymentQuery,
        GetReservationListWithUserAndPaymentQueryResponse
    >
{
    private static readonly string DefaultSearchProperty = "User_FirstName_LastName";

    private static readonly IEnumerable<string> AllowedPropertiesForSearching =
    [
        "User_FirstName_LastName",
        "ArchivedHotelName",
    ];
    private static readonly IEnumerable<string> AllowedPropertiesForSorting =
    [
        "User_FirstName",
        "ArchivedHotelName",
        "RoomType",
    ];

    private readonly IMapper _mapper = mapper;
    private readonly IReservationRepository _reservationRepository = reservationRepository;

    public async Task<GetReservationListWithUserAndPaymentQueryResponse> Handle(
        GetReservationListWithUserAndPaymentQuery request,
        CancellationToken cancellationToken
    )
    {
        var response = new GetReservationListWithUserAndPaymentQueryResponse();

        if (!AllowedPropertiesForSearching.Contains(request.SearchParameters.QueryProperty))
        {
            request.SearchParameters.QueryProperty = DefaultSearchProperty;
        }

        var allowSorting = AllowedPropertiesForSorting.Contains(request.SortParameters.SortBy);

        var (list, totalItems) =
            await _reservationRepository.GetReservationsWithUsersEmailsAndPaymentsPaginated(
                request.PaginationParameters,
                request.SearchParameters,
                allowSorting ? request.SortParameters : null,
                request.FilterParameters
            );

        var offers = _mapper.Map<List<ReservationListWithUserAndPaymentVm>>(list);

        response.Data = new GetReservationListWithUserAndPaymentQueryResponseData
        {
            Reservations = offers,
        };
        response.Metadata = new PaginationMetadata(request.PaginationParameters, totalItems);

        return response;
    }
}
