namespace Reservation.App.Application.Features.Payments.Queries.GetPaymentsForReservation;

public class GetPaymentsForReservationQueryResponseData
{
    public required List<PaymentForReservationVm> Payments { get; set; }
    public required double TotalAmountOfPayments { get; set; }
}
