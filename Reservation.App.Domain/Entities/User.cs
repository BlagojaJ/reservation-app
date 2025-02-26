namespace Reservation.App.Domain.Entities;

public class User
{
    public int ID { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public List<Reservation> Reservations { get; set; } = [];
}
