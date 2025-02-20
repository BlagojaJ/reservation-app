using MediatR;
using Microsoft.Extensions.Logging;
using Reservation.App.Application.Contracts.Persistence;

namespace Reservation.App.Application.Features.Emails.Commands.UpdateEmailStatuses;

public class UpdateEmailStatusCommandHandler(
    IEmailRepository emailRepository,
    ILogger<UpdateEmailStatusCommandHandler> logger
) : IRequestHandler<UpdateEmailStatusesCommand>
{
    private readonly IEmailRepository _emailRepository = emailRepository;
    private readonly ILogger<UpdateEmailStatusCommandHandler> _logger = logger;

    public async Task<Unit> Handle(
        UpdateEmailStatusesCommand request,
        CancellationToken cancellationToken
    )
    {
        var notFoundEmailIds = await _emailRepository.UpdateEmailStatuses(
            request.EmailStatusUpdateDictionary
        );

        if (notFoundEmailIds.Count != 0)
        {
            _logger.LogError(
                "Failed to update email status for {Count} emails. Emails with the following ExternalEmailIds were not found: {ExternalEmailIds}",
                notFoundEmailIds.Count,
                string.Join(", ", notFoundEmailIds)
            );
        }

        return Unit.Value;
    }
}
