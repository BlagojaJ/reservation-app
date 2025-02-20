using MediatR;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Domain.Entities;

namespace Reservation.App.Application.Features.Reservations.Commands.ChangeReservationStatus;

public class ChangeReservationStatusCommandHandler(IReservationRepository reservationRepository)
    : IRequestHandler<ChangeReservationStatusCommand>
{
    private readonly IReservationRepository _reservationRepository = reservationRepository;

    public async Task<Unit> Handle(
        ChangeReservationStatusCommand request,
        CancellationToken cancellationToken
    )
    {
        // 👉 Validation
        var reservation =
            await _reservationRepository.GetByIdAsync(request.Id)
            ?? throw new Exceptions.NotFoundException(nameof(Reservation), request.Id);

        var validator = new ChangeReservationStatusCommandValidator();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new Exceptions.ValidationException(validationResult);
        }

        // 👉 Change the status
        await _reservationRepository.ChangeReservationStatus(reservation, request.Status);

        return Unit.Value;
    }
}
