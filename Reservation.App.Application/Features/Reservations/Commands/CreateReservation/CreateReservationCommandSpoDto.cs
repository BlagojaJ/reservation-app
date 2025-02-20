namespace Reservation.App.Application.Features.Reservations.Commands.CreateReservation;

public class CreateReservationCommandSpoDto
{
    public required int OfferId { get; set; }
    public required string Title { get; set; }
}
