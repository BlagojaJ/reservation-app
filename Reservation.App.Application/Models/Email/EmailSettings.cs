namespace Reservation.App.Application.Models.Email;

public class EmailSettings
{
    public required string SiteSenderEmail { get; set; }
    public required string SiteSenderName { get; set; }
    public required string ReservationsEmail { get; set; }
    public required string ReservationsName { get; set; }
}
