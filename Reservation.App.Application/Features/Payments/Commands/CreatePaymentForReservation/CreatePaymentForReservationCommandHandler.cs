using MediatR;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Domain.Entities;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Payments.Commands.CreatePaymentForReservation;

public class CreatePaymentForReservationCommandHandler(IReservationRepository reservationRepository)
    : IRequestHandler<CreatePaymentForReservationCommand>
{
    private readonly IReservationRepository _reservationRepository = reservationRepository;

    public async Task<Unit> Handle(
        CreatePaymentForReservationCommand request,
        CancellationToken cancellationToken
    )
    {
        var reservation = await _reservationRepository.GetReservationWithPaymentsAsync(
            request.ReservationId
        );

        var validator = new CreatePaymentForReservationCommandValidator(reservation);

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            if (validationResult.Errors.Any(e => e.PropertyName == nameof(request.ReservationId)))
            {
                throw new Exceptions.NotFoundException(nameof(Reservation), request.ReservationId);
            }

            throw new Exceptions.ValidationException(validationResult);
        }

        // 👉 Change the reservation status to OnlinePaymentPending
        reservation!.ReservationStatus = ReservationStatusEnum.OnlinePaymentPending;

        var payment = new Payment { Amount = request.Amount };
        reservation!.Payments.Add(payment);
        await _reservationRepository.UpdateAsync(reservation);

        return Unit.Value;
    }
}
