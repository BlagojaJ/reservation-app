namespace Reservation.App.Domain.Entities;

public class Agent
{
    public int ID { get; set; }
    public required string Name { get; set; }

    public List<Reservation> Reservations { get; set; } = default!;
}
