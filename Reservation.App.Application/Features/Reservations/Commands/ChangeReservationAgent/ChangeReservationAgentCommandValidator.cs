using FluentValidation;
using Reservation.App.Application.Contracts.Persistence;

namespace Reservation.App.Application.Features.Reservations.Commands.ChangeReservationAgent;

public class ChangeReservationAgentCommandValidator
    : AbstractValidator<ChangeReservationAgentCommand>
{
    private readonly IAgentRepository _agentRepository;

    public ChangeReservationAgentCommandValidator(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;

        RuleFor(r => r.AgentId).MustAsync(AgentExistAsync).WithMessage("The agent does not exist.");
    }

    private async Task<bool> AgentExistAsync(int agentId, CancellationToken cancellationToken)
    {
        return await _agentRepository.ExistsAsync(a => a.ID == agentId);
    }
}
