using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Payments.Queries.GetPaymentsForReservation;

public class PaymentForReservationVm
{
    public int ID { get; set; }

    // Financial details
    public double Amount { get; set; }

    // Payment tracking
    public DateTime PaymentCreationDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public PaymentStatusEnum Status { get; set; }

    // Payment gateway tracking
    public string GatewayTransactionId { get; set; } = string.Empty;
}
