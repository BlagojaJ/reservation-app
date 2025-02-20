using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Reservations.Queries.GetReservationListWithUserAndPayment;

public class FilterParameters
{
    public ReservationExternalApiEnum? ActiveApiAtTimeOfReservation { get; set; }
    public ReservationStatusEnum? ReservationStatus { get; set; }
    public DateTime? CheckInStart { get; set; }
    public DateTime? CheckInEnd { get; set; }
    public DateTime? CheckOutStart { get; set; }
    public DateTime? CheckOutEnd { get; set; }
    public DateTime? CreatedStart { get; set; }
    public DateTime? CreatedEnd { get; set; }
}
