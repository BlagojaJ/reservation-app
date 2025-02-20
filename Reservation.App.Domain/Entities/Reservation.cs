using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Reservation.App.Domain.Json;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Domain.Entities;

public class Reservation
{
    public int ID { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public ReservationStatusEnum ReservationStatus { get; set; } = ReservationStatusEnum.Open;
    public string Password { get; set; } = string.Empty;

    public string? Message { get; set; }

    // Query data
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int AdultsNumber { get; set; }
    public int ChildrenNumber { get; set; }
    public List<int>? ChildrenAges { get; set; }

    // Additional passenger information provided from the user (stored as JSON)
    public string AdultsPassengersInfo { get; set; } = "[]";
    public string ChildPassengersInfo { get; set; } = "[]";

    // Selected offer data
    public string RoomType { get; set; } = string.Empty;
    public BoardTypeEnum Board { get; set; }
    public string BoardAlternative { get; set; } = string.Empty;
    public RoomStatusEnum Status { get; set; }
    public double Price { get; set; }
    public double? PriceWithDiscount { get; set; }
    public string? PaymentTerms { get; set; }
    public string? CancellationFees { get; set; }
    public string? SpecialOffers { get; set; }

    // Final price in MKD used for validating payment amounts before creating payments => Set up in CreateReservationCommand
    public double FinalPriceInMKDForPayment { get; set; }

    // Snapshot of offer data that could change
    public int? Markup { get; set; }
    public List<ReservationExternalApiEnum> ActiveApisAtTimeOfReservation { get; set; } = [];

    // Snapshot of hotel id, hotel name and offer id since they can be deleted
    public int ArchivedHotelId { get; set; }
    public required string ArchivedHotelName { get; set; }
    public int ArchivedOfferId { get; set; }

    // public Offer? Offer { get; set; }
    // public int? OfferId { get; set; }
    public User User { get; set; } = default!;
    public int UserId { get; set; }
    public Agent? Agent { get; set; }
    public int? AgentId { get; set; }

    public List<Email> Emails { get; set; } = []; // Only types: ReservationRequestToAgency, ReservationRequestConfirmationToClient, PaymentLinkToClient
    public List<Payment> Payments { get; set; } = [];

    [NotMapped]
    public int Nights
    {
        get => (CheckOutDate - CheckInDate).Days;
    }

    [NotMapped]
    public List<AdultPassengerInfo> AdultsPassengersInfoListOfObjects
    {
        get
        {
            if (!string.IsNullOrEmpty(AdultsPassengersInfo))
            {
                return JsonSerializer.Deserialize<List<AdultPassengerInfo>>(AdultsPassengersInfo)
                    ?? [];
            }
            else
            {
                return [];
            }
        }
    }

    [NotMapped]
    public List<ChildrenPassengerInfo> ChildPassengersInfoListOfObjects
    {
        get
        {
            if (!string.IsNullOrEmpty(ChildPassengersInfo))
            {
                var x = JsonSerializer.Deserialize<List<ChildrenPassengerInfo>>(
                    ChildPassengersInfo
                );
                return x ?? [];
            }
            else
            {
                return [];
            }
        }
    }
}
