namespace Reservation.App.Application.Features.Reservations.Queries.GetReservationListWithUserAndPayment;

public class ReservationListWithUserAndPaymentUserDto
{
    public int ID { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
