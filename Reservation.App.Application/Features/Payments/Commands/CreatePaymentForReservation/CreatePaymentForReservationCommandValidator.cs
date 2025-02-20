using FluentValidation;
using Reservation.App.Domain.Entities;

namespace Reservation.App.Application.Features.Payments.Commands.CreatePaymentForReservation;

public class CreatePaymentForReservationCommandValidator
    : AbstractValidator<CreatePaymentForReservationCommand>
{
    private readonly Reservation? _reservation;

    public CreatePaymentForReservationCommandValidator(Reservation? reservation)
    {
        _reservation = reservation;

        RuleFor(c => c.ReservationId)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .Must(ReservationExists)
            .WithMessage("The reservation does not exist.");

        RuleFor(c => c.Amount)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .Must(AmountIsValid)
            .WithMessage(
                "The amount exceeds the final price taken into consideration existing payments."
            );
    }

    private bool ReservationExists(int _)
    {
        return _reservation != null;
    }

    private bool AmountIsValid(double amount)
    {
        // If there is no reservation do not check statuses
        if (_reservation == null)
        {
            return true;
        }

        var alreadyCreatedPayments = _reservation.Payments.Sum(p => p.Amount);

        if (amount <= _reservation.FinalPriceInMKDForPayment - alreadyCreatedPayments)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
