namespace Reservation.App.Domain.Shared.Enums;

public enum ReservationStatusEnum
{
    None,
    Open,
    ClientContacted,
    OnlinePaymentPending,
    OnlinePaymentPartiallyPaid,
    OnlinePaymentFullyPaid,
    ReservedInAgency,
}
