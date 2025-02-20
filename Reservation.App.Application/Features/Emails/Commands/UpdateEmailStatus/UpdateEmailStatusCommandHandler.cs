using MediatR;
using Microsoft.Extensions.Logging;
using Reservation.App.Application.Contracts.Persistence;

namespace Reservation.App.Application.Features.Emails.Commands.UpdateEmailStatus;

public class UpdateEmailStatusCommandHandler(
    IEmailRepository emailRepository,
    ILogger<UpdateEmailStatusCommandHandler> logger
) : IRequestHandler<UpdateEmailStatusCommand>
{
    private readonly IEmailRepository _emailRepository = emailRepository;
    private readonly ILogger<UpdateEmailStatusCommandHandler> _logger = logger;

    public async Task<Unit> Handle(
        UpdateEmailStatusCommand request,
        CancellationToken cancellationToken
    )
    {
        var success = await _emailRepository.UpdateEmailStatus(
            request.ExternalEmailId,
            request.EmailStatus
        );

        if (!success)
        {
            _logger.LogError(
                "Failed to update email status. Email with ExternalEmailId {ExternalEmailId} not found.",
                request.ExternalEmailId
            );
        }

        return Unit.Value;
    }
}
