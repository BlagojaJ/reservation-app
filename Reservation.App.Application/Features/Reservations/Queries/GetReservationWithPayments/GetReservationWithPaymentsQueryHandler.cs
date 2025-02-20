using AutoMapper;
using MediatR;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Domain.Entities;

namespace Reservation.App.Application.Features.Reservations.Queries.GetReservationWithPayments;

public class GetReservationWithPaymentsQueryHandler(
    IMapper mapper,
    IReservationRepository reservationRepository
) : IRequestHandler<GetReservationWithPaymentsQuery, GetReservationWithPaymentsQueryResponse>
{
    private readonly IMapper _mapper = mapper;
    private readonly IReservationRepository _reservationRepository = reservationRepository;

    public async Task<GetReservationWithPaymentsQueryResponse> Handle(
        GetReservationWithPaymentsQuery request,
        CancellationToken cancellationToken
    )
    {
        var response = new GetReservationWithPaymentsQueryResponse();

        var reservationIdAndPasswordMatch =
            await _reservationRepository.CheckIfReservationAndPasswordMatch(
                request.Id,
                request.Password
            );
        if (!reservationIdAndPasswordMatch)
        {
            throw new Exceptions.NotFoundException(nameof(Reservation), request.Id);
        }

        var reservation = await _reservationRepository.GetReservationWithPaymentsAsync(request.Id);

        response.Data = new() { Reservation = _mapper.Map<ReservationWithPaymentsVm>(reservation) };

        return response;
    }
}
