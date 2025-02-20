using MediatR;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Reservations.Commands.ChangeReservationAgent;

public class ChangeReservationAgentCommand : IRequest
{
    public int Id { get; set; }
    public int AgentId { get; set; }
}
