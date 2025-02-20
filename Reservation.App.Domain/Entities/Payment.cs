using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Domain.Entities;

public class Payment
{
    public int ID { get; set; }

    // Financial details
    public double Amount { get; set; }
    public CurrencyEnum Currency { get; set; } = CurrencyEnum.MKD;

    // Payment tracking
    public DateTime PaymentCreationDate { get; set; } = DateTime.UtcNow;
    public DateTime? PaymentDate { get; set; }
    public PaymentStatusEnum Status { get; set; } = PaymentStatusEnum.Pending;

    // Payment gateway tracking
    public string GatewayTransactionId { get; set; } = string.Empty;
    public string? GatewayResponse { get; set; }

    public int? ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    public List<Email> Emails { get; set; } = []; // Only types:  PaymentSuccessToClient, PaymentSuccessToAgency
}
