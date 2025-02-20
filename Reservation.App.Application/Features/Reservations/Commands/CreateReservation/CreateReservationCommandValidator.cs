using FluentValidation;
using Reservation.App.Application.Contracts.Application;
using Reservation.App.Application.Contracts.Infrastructure;
using Reservation.App.Application.Models.Configuration;
using Reservation.App.Domain.Entities;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Reservations.Commands.CreateReservation;

// cSpell: disable
public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    private readonly ISignatureService _signatureService;
    private readonly SigningSettings _signingSettings;
    private readonly ICaptchaValidator _captchaValidator;

    private readonly CreateReservationCommandQueryDto _query;
    private readonly Offer? _offer;

    public CreateReservationCommandValidator(
        ISignatureService signatureService,
        SigningSettings signingSettings,
        ICaptchaValidator captchaValidator,
        CreateReservationCommandQueryDto query,
        Offer? offer
    )
    {
        _signatureService = signatureService;
        _signingSettings = signingSettings;
        _captchaValidator = captchaValidator;
        _query = query;
        _offer = offer;

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Името е задолжително.")
            .Length(2, 50)
            .WithMessage("Името мора да биде помеѓу 2 и 50 карактери.")
            .Matches(@"^[a-zA-Z\u0400-\u04FF '-]+$")
            .WithMessage("Името содржи неважечки карактери.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Презимето е задолжително.")
            .Length(2, 50)
            .WithMessage("Презимето мора да биде помеѓу 2 и 50 карактери.")
            .Matches(@"^[a-zA-Z\u0400-\u04FF '-]+$")
            .WithMessage("Презимето содржи неважечки карактери.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Е-пошта е задолжителна.")
            .MaximumLength(100)
            .WithMessage("Е-поштата не може да има повеќе од 100 карактери.")
            .EmailAddress()
            .WithMessage("Невалиден формат на е-пошта.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Length(6, 13)
            .WithMessage("Невалиден телефонски број.");

        RuleFor(x => x.Message)
            .MaximumLength(300)
            .WithMessage("Пораката не може да има повеќе од 300 карактери.")
            .When(x => !string.IsNullOrEmpty(x.Message));

        RuleFor(x => x.Offer)
            .Must(OfferExists)
            .WithMessage("Понудата не постои.")
            .Must(ValidReservationRoomStatus)
            .WithMessage("Не може да се направи резервација со невалиден статус на понуда")
            .Must(SignatureMatches)
            .WithMessage("Невалидна понуда.");

        RuleFor(x => x.AdultsInfo)
            .NotEmpty()
            .WithMessage("Информациите за возрасни не можат да бидат празни.")
            .When(x => _query.AdultsNumber > 1);

        RuleForEach(x => x.AdultsInfo)
            .ChildRules(
                (adult) =>
                {
                    adult
                        .RuleFor(a => a.FirstName)
                        .NotEmpty()
                        .WithMessage("Името на возрасниот патник е задолжително.")
                        .Length(2, 50)
                        .WithMessage(a =>
                            $"Името '{a.FirstName}' на возрасниот патник мора да биде помеѓу 2 и 50 карактери."
                        )
                        .Matches(@"^[a-zA-Z\u0400-\u04FF '-]+$")
                        .WithMessage(a =>
                            $"Името '{a.FirstName}' на возрасниот патник содржи неважечки карактери."
                        );

                    adult
                        .RuleFor(a => a.LastName)
                        .NotEmpty()
                        .WithMessage($"Презимето на возрасниот патник е задолжително.")
                        .Length(2, 50)
                        .WithMessage(a =>
                            $"Презимето '{a.LastName}' на возрасниот патник мора да биде помеѓу 2 и 50 карактери."
                        )
                        .Matches(@"^[a-zA-Z\u0400-\u04FF '-]+$")
                        .WithMessage(a =>
                            $"Презимето '{a.LastName}' на возрасниот содржи неважечки карактери."
                        );
                }
            );

        RuleFor(x => x.ChildrenInfo)
            .NotEmpty()
            .WithMessage("Информациите за деца не можат да бидат празни.")
            .When(x => _query.ChildrenNumber > 0);
        ;

        RuleForEach(x => x.ChildrenInfo)
            .ChildRules(child =>
            {
                child
                    .RuleFor(c => c.FirstName)
                    .NotEmpty()
                    .WithMessage("Името на детето е задолжително.")
                    .Length(2, 50)
                    .WithMessage(c =>
                        $"Името '{c.FirstName}' на детето мора да биде помеѓу 2 и 50 карактери."
                    )
                    .Matches(@"^[a-zA-Z\u0400-\u04FF '-]+$")
                    .WithMessage(c =>
                        $"Името '{c.FirstName}' на детето содржи неважечки карактери."
                    );

                child
                    .RuleFor(c => c.LastName)
                    .NotEmpty()
                    .WithMessage("Презимето на детето е задолжително.")
                    .Length(2, 50)
                    .WithMessage(c =>
                        $"Презимето '{c.LastName}' на детето мора да биде помеѓу 2 и 50 карактери."
                    )
                    .Matches(@"^[a-zA-Z\u0400-\u04FF '-]+$")
                    .WithMessage(c =>
                        $"Презимето '{c.LastName}' на детето содржи неважечки карактери."
                    );

                child
                    .RuleFor(c => c.BirthDate)
                    .NotEmpty()
                    .WithMessage("Датумот на раѓање на детето е задолжителелен.")
                    .LessThan(DateTime.Now)
                    .WithMessage("Датумот на раѓање на детето мора да биде во минатото.");
            });

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Настана проблем со токенот за верификација на вашето барање. (404)")
            .MaximumLength(2048)
            .WithMessage("Настана проблем со токенот за верификација на вашето барање. (400)")
            .MustAsync(TokenIsValid)
            .WithMessage(
                "Невалиден токен за верификација. Обидете се повторно да го испратите вашето барање."
            );
    }

    private bool OfferExists(CreateReservationCommandSelectedOfferDto _)
    {
        return _offer != null;
    }

    private bool ValidReservationRoomStatus(CreateReservationCommandSelectedOfferDto offer)
    {
        return offer.Status == RoomStatusEnum.Available
            || offer.Status == RoomStatusEnum.UnAvailable
            || offer.Status == RoomStatusEnum.OnRequest;
    }

    private bool SignatureMatches(
        CreateReservationCommand command,
        CreateReservationCommandSelectedOfferDto selectedOffer
    )
    {
        return _signatureService.GenerateSignature(
                $"{command.Query.GetSignaturePayload()}{selectedOffer.GetSignaturePayload()}",
                _signingSettings.OfferSigningKey
            ) == selectedOffer.Signature;
    }

    private Task<bool> TokenIsValid(string token, CancellationToken cancellationToken)
    {
        return _captchaValidator.ValidateTokenAsync(token);
    }
}
