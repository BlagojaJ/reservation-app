using System.Text;
using System.Text.Json;
using AutoMapper;
using Ganss.Xss;
using MediatR;
using Microsoft.Extensions.Options;
using Reservation.App.Application.Contracts.Application;
using Reservation.App.Application.Contracts.Infrastructure;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Application.Models.Api;
using Reservation.App.Application.Models.Configuration;
using Reservation.App.Application.Models.Email;
using Reservation.App.Domain.Entities;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Reservations.Commands.CreateReservation;

public class CreateReservationCommandHandler(
    IMapper mapper,
    IEmailService emailService,
    HtmlSanitizer htmlSanitizer,
    ISignatureService signatureService,
    IMarkupService markupService,
    IOfferRepository offerRepository,
    IUserRepository userRepository,
    IReservationRepository reservationRepository,
    IEmailRepository emailRepository,
    ICaptchaValidator captchaValidator,
    IPasswordGenerator passwordGenerator,
    IOptions<EmailSettings> emailSettings,
    IOptions<SigningSettings> signingSettings,
    IOptions<ExchangeRates> exchangeRatesSettings
) : IRequestHandler<CreateReservationCommand>
{
    private static readonly string TabSpacingForMailList = "\t   ";

    private readonly IMapper _mapper = mapper;
    private readonly IEmailService _emailService = emailService;
    private readonly HtmlSanitizer _htmlSanitizer = htmlSanitizer;
    private readonly ISignatureService _signatureService = signatureService;
    private readonly IMarkupService _markupService = markupService;
    private readonly IOfferRepository _offerRepository = offerRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IReservationRepository _reservationRepository = reservationRepository;
    private readonly IEmailRepository _emailRepository = emailRepository;
    private readonly ICaptchaValidator _captchaValidator = captchaValidator;
    private readonly IPasswordGenerator _passwordGenerator = passwordGenerator;
    private readonly EmailSettings _emailSettings = emailSettings.Value;
    private readonly SigningSettings _signingSettings = signingSettings.Value;
    private readonly ExchangeRates _exchangeRatesSettings = exchangeRatesSettings.Value;

    public async Task<Unit> Handle(
        CreateReservationCommand request,
        CancellationToken cancellationToken
    )
    {
        // 👉 Validation
        var offer = await _offerRepository.GetActiveOfferWithHotelByIdAsync(request.Query.OfferId);

        var validator = new CreateReservationCommandValidator(
            _signatureService,
            _signingSettings,
            _captchaValidator,
            request.Query,
            offer
        );

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new Exceptions.ValidationException(validationResult);
        }

        // 👉 Input formatting and sanitization
        request.FirstName = CapitalizeAndTrimWords(request.FirstName);
        request.LastName = CapitalizeAndTrimWords(request.LastName);
        request.Email = request.Email.ToLower().Trim();
        request.PhoneNumber = CapitalizeAndTrimWords(request.PhoneNumber);
        request.Message = SanitizeStringNullable(request.Message);

        request.AdultsInfo.ForEach(adult =>
        {
            adult.FirstName = CapitalizeAndTrimWords(adult.FirstName);
            adult.LastName = CapitalizeAndTrimWords(adult.LastName);
        });
        request.ChildrenInfo.ForEach(child =>
        {
            child.FirstName = CapitalizeAndTrimWords(child.FirstName);
            child.LastName = CapitalizeAndTrimWords(child.LastName);
        });

        // 💁‍♂️ PaymentTerms and CancellationFees are not validated with the signature, since they differ across APIs
        request.Offer.PaymentTerms = SanitizeStringNullable(request.Offer.PaymentTerms);
        if (request.Offer.PaymentTerms != null && request.Offer.PaymentTerms.Length > 2000)
        {
            request.Offer.PaymentTerms = request.Offer.PaymentTerms.Substring(0, 2000); // Limiting the payment terms string to 300
        }
        if (request.Offer.CancellationFees != null)
        {
            foreach (var cancellationFee in request.Offer.CancellationFees.Take(15)) // Limiting the number of cancellation fees to 15
            {
                cancellationFee.After = SanitizeString(cancellationFee.After);
                cancellationFee.Value = SanitizeString(cancellationFee.Value);
            }
        }

        // 👉 Writing to database
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            user = _mapper.Map<User>(request);
            await _userRepository.AddAsync(user);
        }

        var activeApis = GetActiveApisAtTimeOfReservation(offer!);

        string? paymentTermsEmailString = ExtractPaymentTermsStringForEmail(request, activeApis);

        string? cancellationFeesEmailString = ExtractCancellationFeesStringForEmail(request);

        string? specialOffersEmailString = ExtractSpecialOfferStringForEmail(request);

        var reservation = _mapper.Map<Reservation>(request);

        reservation.Password = _passwordGenerator.GeneratePassword(6);

        reservation.AdultsPassengersInfo = JsonSerializer.Serialize(request.AdultsInfo);
        reservation.ChildPassengersInfo = JsonSerializer.Serialize(request.ChildrenInfo);

        // Remove tab in string for db entry
        reservation.PaymentTerms = paymentTermsEmailString?.Replace(
            TabSpacingForMailList,
            string.Empty
        );
        reservation.CancellationFees = cancellationFeesEmailString?.Replace(
            TabSpacingForMailList,
            string.Empty
        );
        reservation.SpecialOffers = specialOffersEmailString?.Replace(
            TabSpacingForMailList,
            string.Empty
        );

        //  Set the FinalPriceInMKDForPayment
        if (
            request.Offer.PriceWithDiscount != null
            && request.Offer.PriceWithDiscount != default(double)
        )
        {
            reservation.FinalPriceInMKDForPayment = Math.Ceiling(
                request.Offer.PriceWithDiscount.Value * _exchangeRatesSettings.EurToMkd
            );
        }
        else
        {
            reservation.FinalPriceInMKDForPayment = Math.Ceiling(
                request.Offer.Price * _exchangeRatesSettings.EurToMkd
            );
        }

        reservation.Markup = await _markupService.GetMarkupForApiOfferAsync(
            (ExternalAPIsEnum)activeApis.First(),
            offer!
        );
        reservation.ActiveApisAtTimeOfReservation = activeApis;

        reservation.ArchivedHotelId = offer!.Hotel.ID;
        reservation.ArchivedHotelName = offer!.Hotel.Name;
        reservation.ArchivedOfferId = offer.ID;

        reservation.UserId = user.ID;

        await _reservationRepository.AddAsync(reservation);

        // 👉 Sending a reservation request email to agency
        try
        {
            // ➡️ Send the email
            var uniqueEmailId = await SendReservationRequestEmailToAgency(
                reservation.ID,
                request,
                paymentTermsEmailString,
                cancellationFeesEmailString,
                specialOffersEmailString
            );

            // ➡️ Create db email record for deliverability tracking
            var email = new Email
            {
                ExternalEmailId = uniqueEmailId,
                EmailType = EmailTypeEnum.ReservationRequestToAgency,
                EmailStatus = EmailStatusEnum.Processed,
                ReservationId = reservation.ID,
            };

            await _emailRepository.AddAsync(email);
        }
        catch (Exception)
        {
            // ? Should we throw an exception here since there is already a database record for the reservation
            // throw new Exceptions.InternalServerException(
            //     "Failed to forward the reservation request.",
            //     ex
            // );
        }

        // 👉 Sending a reservation request confirmation email to client
        try
        {
            // ➡️ Send the email
            var uniqueEmailId = await SendReservationRequestConfirmationEmailToClient(
                reservation.ID,
                offer.Hotel.Name,
                request
            );

            // ➡️ Create db email record for deliverability tracking
            var email = new Email
            {
                ExternalEmailId = uniqueEmailId,
                EmailType = EmailTypeEnum.ReservationRequestConfirmationToClient,
                EmailStatus = EmailStatusEnum.Processed,
                ReservationId = reservation.ID,
            };

            await _emailRepository.AddAsync(email);
        }
        catch (Exception)
        {
            // logged inside the email service
        }

        return Unit.Value;
    }

    private string SanitizeString(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return string.Empty;
        }

        return _htmlSanitizer.Sanitize(message).Trim();
    }

    private string? SanitizeStringNullable(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return null;
        }

        return _htmlSanitizer.Sanitize(message).Trim();
    }

    private static string CapitalizeAndTrimWords(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var words = input.Split(
            [' '],
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
        );
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
        }

        return string.Join(' ', words);
    }

    private static List<ReservationExternalApiEnum> GetActiveApisAtTimeOfReservation(Offer offer)
    {
        var activeApis = new List<ReservationExternalApiEnum>();

        // ... some logic to determine the active APIs

        return activeApis;
    }

    private static string? ExtractPaymentTermsStringForEmail(
        CreateReservationCommand request,
        List<ReservationExternalApiEnum> activeApis
    )
    {
        string? paymentTermsEmailString = null;

        //    ... some logic to extract payment terms

        return paymentTermsEmailString;
    }

    private static string? ExtractCancellationFeesStringForEmail(CreateReservationCommand request)
    {
        string? cancellationFeesEmailString = null;
        if (request.Offer.CancellationFees != null)
        {
            var sb = new StringBuilder();

            foreach (var cancellationFee in request.Offer.CancellationFees)
            {
                sb.AppendLine(
                    $"{TabSpacingForMailList}- По {cancellationFee.After}:  {cancellationFee.Value}"
                );
            }

            cancellationFeesEmailString = sb.ToString();
        }

        return cancellationFeesEmailString;
    }

    private static string? ExtractSpecialOfferStringForEmail(CreateReservationCommand request)
    {
        string? specialOffersEmailString = null;
        if (request.Offer.SpecialOffers != null)
        {
            var sb = new StringBuilder();

            foreach (var specialOffer in request.Offer.SpecialOffers)
            {
                sb.AppendLine(
                    $"{TabSpacingForMailList}- {specialOffer.Title} (ID: {(specialOffer.OfferId != 0 ? specialOffer.OfferId : "/")})"
                );
            }

            specialOffersEmailString = sb.ToString();
        }

        return specialOffersEmailString;
    }

    private async Task<string> SendReservationRequestEmailToAgency(
        int reservationId,
        CreateReservationCommand request,
        string? paymentTermsEmailString,
        string? cancellationFeesEmailString,
        string? specialOffersEmailString
    )
    {
        var from = new EmailAddress
        {
            Email = _emailSettings.SiteSenderEmail,
            Name = _emailSettings.SiteSenderName,
        };
        var to = new EmailAddress
        {
            Email = _emailSettings.ReservationsEmail,
            Name = _emailSettings.ReservationsName,
        };
        var replyTo = new EmailAddress
        {
            Email = request.Email,
            Name = $"{request.FirstName} {request.LastName}",
        };

        var emailExternalId = await _emailService.SendEmailAsync(
            from,
            to,
            $"Барање за резервација ({reservationId})",
            GenerateReservationRequestEmailBody(
                request,
                paymentTermsEmailString,
                cancellationFeesEmailString,
                specialOffersEmailString
            ),
            replyTo
        );

        return emailExternalId;
    }

    private static string GenerateReservationRequestEmailBody(
        CreateReservationCommand reservationCommand,
        // 👇 Must be formatted
        string? paymentTerms,
        string? cancellationFees,
        string? specialOffers
    )
    {
        var sb = new StringBuilder();

        // cSpell: disable
        sb.AppendLine("Почитувани,");
        // ... email body generation
        // cSpell: enable

        return sb.ToString();
    }

    private async Task<string> SendReservationRequestConfirmationEmailToClient(
        int reservationId,
        string hotelName,
        CreateReservationCommand request
    )
    {
        var from = new EmailAddress
        {
            Email = _emailSettings.ReservationsEmail,
            Name = _emailSettings.ReservationsName,
        };
        var to = new EmailAddress
        {
            Email = request.Email,
            Name = $"{request.FirstName} {request.LastName}",
        };

        var emailExternalId = await _emailService.SendEmailAsync(
            from,
            to,
            $"Потврда за примено барање за резервација ({reservationId})",
            GenerateReservationRequestConfirmationEmailBody(hotelName, request)
        );

        return emailExternalId;
    }

    private static string GenerateReservationRequestConfirmationEmailBody(
        string hotelName,
        CreateReservationCommand reservationCommand
    )
    {
        var sb = new StringBuilder();

        // cSpell: disable
        sb.AppendLine("Почитувани,");
        sb.AppendLine();
        sb.AppendLine();

        sb.AppendLine("Ви благодариме за вашето барање за резервација за:");

        // ... email body generation
        // cSpell: enable

        return sb.ToString();
    }
}
