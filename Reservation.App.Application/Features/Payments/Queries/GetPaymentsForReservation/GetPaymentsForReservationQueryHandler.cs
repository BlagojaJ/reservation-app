using AutoMapper;
using MediatR;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Domain.Entities;

namespace Reservation.App.Application.Features.Payments.Queries.GetPaymentsForReservation;

public class GetPaymentsForReservationQueryHandler(
    IMapper mapper,
    IReservationRepository reservationRepository,
    IPaymentRepository paymentRepository
) : IRequestHandler<GetPaymentsForReservationQuery, GetPaymentsForReservationQueryResponse>
{
    private readonly IMapper _mapper = mapper;
    private readonly IReservationRepository _reservationRepository = reservationRepository;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;

    public async Task<GetPaymentsForReservationQueryResponse> Handle(
        GetPaymentsForReservationQuery request,
        CancellationToken cancellationToken
    )
    {
        var response = new GetPaymentsForReservationQueryResponse();

        var reservationExists = await _reservationRepository.ExistsAsync(r =>
            r.ID == request.ReservationId
        );

        if (!reservationExists)
        {
            throw new Exceptions.NotFoundException(nameof(Reservation), request.ReservationId);
        }

        var payments = await _paymentRepository.GetPaymentsWithEmailsForReservationAsync(
            request.ReservationId
        );

        var totalAmount = payments.Sum(p => p.Amount);

        response.Data = new GetPaymentsForReservationQueryResponseData
        {
            Payments = _mapper.Map<List<PaymentForReservationVm>>(payments),
            TotalAmountOfPayments = totalAmount,
        };

        return response;
    }
}
