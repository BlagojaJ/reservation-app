using MediatR;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Features.Emails.Commands.UpdateEmailStatus;

public class UpdateEmailStatusCommand : IRequest
{
    public required string ExternalEmailId { get; set; }
    public EmailStatusEnum EmailStatus { get; set; }
}
