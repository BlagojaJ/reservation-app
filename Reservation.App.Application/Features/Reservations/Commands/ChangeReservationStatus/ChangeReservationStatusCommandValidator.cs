using FluentValidation;
using Reservation.App.Domain.Entities;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Reservations.Commands.ChangeReservationStatus;

public class ChangeReservationStatusCommandValidator
    : AbstractValidator<ChangeReservationStatusCommand>
{
    public ChangeReservationStatusCommandValidator()
    {
        RuleFor(r => r.Status)
            .Must(status =>
                status == ReservationStatusEnum.Open
                || status == ReservationStatusEnum.ClientContacted
                || status == ReservationStatusEnum.ReservedInAgency
            )
            .WithMessage(
                "Status must be one of the following values: Open, ClientContacted, ReservedInAgency"
            );
    }
}
