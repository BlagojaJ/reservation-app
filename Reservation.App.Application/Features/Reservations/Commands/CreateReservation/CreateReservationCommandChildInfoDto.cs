namespace Reservation.App.Application.Features.Reservations.Commands.CreateReservation;

public class CreateReservationCommandChildInfoDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime? BirthDate { get; set; }
}
