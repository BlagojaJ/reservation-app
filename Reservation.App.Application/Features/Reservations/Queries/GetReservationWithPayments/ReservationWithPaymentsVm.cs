namespace Reservation.App.Application.Features.Reservations.Queries.GetReservationWithPayments;

public class ReservationWithPaymentsVm
{
    public int ID { get; set; }

    public DateTime CheckInDate { get; set; }
    public int Nights { get; set; }
    public int AdultsNumber { get; set; }
    public int ChildrenNumber { get; set; }
    public List<int>? ChildrenAges { get; set; }

    public double FinalPriceInEUR { get; set; }
    public double FinalPriceInMKD { get; set; }

    public string HotelName { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;

    public List<ReservationWithPaymentsPaymentDto> Payments { get; set; } = [];
}
