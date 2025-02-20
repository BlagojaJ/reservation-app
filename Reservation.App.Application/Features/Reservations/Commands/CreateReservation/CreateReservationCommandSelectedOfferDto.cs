using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Reservations.Commands.CreateReservation;

// NOTE: PaymentTerms and CancellationFees are not validated with the signature
public class CreateReservationCommandSelectedOfferDto
{
    public required string RoomType { get; set; }
    public required BoardTypeEnum Board { get; set; }
    public string BoardAlternative { get; set; } = string.Empty;
    public required RoomStatusEnum Status { get; set; }
    public required double Price { get; set; }
    public double? PriceWithDiscount { get; set; }
    public string? PaymentTerms { get; set; } // 💁‍♂️ Differs across APIs, hence received as string
    public List<CreateReservationCommandSelectedOfferDtoCancellationFeeDto>? CancellationFees { get; set; }
    public List<CreateReservationCommandSpoDto>? SpecialOffers { get; set; }

    public required string Signature { get; set; }

    public string GetSignaturePayload()
    {
        var specialOffersString =
            SpecialOffers != null
                ? SpecialOffers
                    .Select(so => $"{so.Title}{so.OfferId}")
                    .Aggregate((x, y) => $"{x}{y}")
                : string.Empty;

        return $"{RoomType}{Board}{BoardAlternative}{Status}{Price}{PriceWithDiscount}{specialOffersString}";
    }
}
