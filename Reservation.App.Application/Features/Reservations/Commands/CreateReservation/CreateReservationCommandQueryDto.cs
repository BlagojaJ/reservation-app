namespace Reservation.App.Application.Features.Reservations.Commands.CreateReservation;

public class CreateReservationCommandQueryDto
{
    public required int OfferId { get; set; }
    public required DateTime CheckInDate { get; set; }
    public required int Nights { get; set; }
    public required int AdultsNumber { get; set; }
    public required int ChildrenNumber { get; set; }
    public List<int>? ChildrenAges { get; set; }

    public string GetSignaturePayload()
    {
        return $"{OfferId}{CheckInDate:yyyy-MM-dd}{Nights}{AdultsNumber}{ChildrenNumber}{string.Join("", ChildrenAges ?? [])}";
    }
}
