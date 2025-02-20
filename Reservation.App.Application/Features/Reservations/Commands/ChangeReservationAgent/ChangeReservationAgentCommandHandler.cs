using MediatR;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Domain.Entities;

namespace Reservation.App.Application.Features.Reservations.Commands.ChangeReservationAgent;

public class ChangeReservationAgentCommandHandler(
    IReservationRepository reservationRepository,
    IAgentRepository agentRepository
) : IRequestHandler<ChangeReservationAgentCommand>
{
    private readonly IReservationRepository _reservationRepository = reservationRepository;
    private readonly IAgentRepository _agentRepository = agentRepository;

    public async Task<Unit> Handle(
        ChangeReservationAgentCommand request,
        CancellationToken cancellationToken
    )
    {
        // 👉 Validation
        var reservation =
            await _reservationRepository.GetByIdAsync(request.Id)
            ?? throw new Exceptions.NotFoundException(nameof(Reservation), request.Id);

        var validator = new ChangeReservationAgentCommandValidator(_agentRepository);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new Exceptions.ValidationException(validationResult);
        }

        // 👉 Change the status
        await _reservationRepository.ChangeReservationAgent(reservation, request.AgentId);

        return Unit.Value;
    }
}
