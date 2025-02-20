using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Domain.Entities;

public class Email
{
    public int ID { get; set; }
    public string ExternalEmailId { get; set; } = string.Empty;
    public EmailTypeEnum EmailType { get; set; }
    public EmailStatusEnum EmailStatus { get; set; }

    public int? ReservationId { get; set; }
    public Reservation? Reservation { get; set; }
    public int? PaymentId { get; set; }
    public Payment? Payment { get; set; }
}
