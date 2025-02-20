using MediatR;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Emails.Commands.UpdateEmailStatuses;

public class UpdateEmailStatusesCommand : IRequest
{
    public required Dictionary<string, EmailStatusEnum> EmailStatusUpdateDictionary { get; set; }
}
