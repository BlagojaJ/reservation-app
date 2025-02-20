using Reservation.App.Application.Features.Reservations.Commands.CreateReservation;
using Reservation.App.Domain.Json;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Reservations.Queries.GetReservationListWithUserAndPayment;

public class ReservationListWithUserAndPaymentVm
{
    public int ID { get; set; }

    public DateTime Date { get; set; }
    public ReservationStatusEnum ReservationStatus { get; set; }

    public string? Message { get; set; }

    // Query data
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int AdultsNumber { get; set; }
    public int ChildrenNumber { get; set; }
    public List<int>? ChildrenAges { get; set; }

    // Additional information gathered from the user
    public List<AdultPassengerInfo> AdultsInfo { get; set; } = [];
    public List<ChildrenPassengerInfo> ChildrenInfo { get; set; } = [];

    // Selected offer data
    public string RoomType { get; set; } = string.Empty;
    public string BoardFinal { get; set; } = string.Empty;
    public RoomStatusEnum Status { get; set; }
    public double Price { get; set; }
    public double? PriceWithDiscount { get; set; }
    public string? PaymentTerms { get; set; }
    public string? CancellationFees { get; set; }
    public string? SpecialOffers { get; set; }

    // Final price in MKD used for validating payment amounts before creating payments => Set up in CreateReservationCommand
    public double FinalPriceInMKDForPayment { get; set; }

    // Offer data that could change
    public int? Markup { get; set; }
    public List<ReservationExternalApiEnum> ActiveApisAtTimeOfReservation { get; set; } = [];

    // Snapshot of hotel name and offer
    public int ArchivedHotelId { get; set; }
    public required string ArchivedHotelName { get; set; }
    public required int ArchivedOfferId { get; set; }

    public ReservationListWithUserAndPaymentUserDto User { get; set; } = new();
    public int? AgentId { get; set; }

    // Email delivery information
    public bool IsEmailToAgencyDelivered { get; set; }
    public bool IsEmailToClientDelivered { get; set; }
}
