namespace Reservation.App.Domain.Shared.Enums;

public enum EmailTypeEnum
{
    Undefined,
    ReservationRequestToAgency,
    ReservationRequestConfirmationToClient,
    PaymentLinkToClient,
    PaymentSuccessToClient,
    PaymentSuccessToAgency,
}
