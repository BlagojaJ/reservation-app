using MediatR;

namespace Reservation.App.Application.Features.Reservations.Commands.CreateReservation;

public class CreateReservationCommand : IRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Message { get; set; }

    // Search data
    public required CreateReservationCommandQueryDto Query { get; set; }

    // Selected offer data
    public required CreateReservationCommandSelectedOfferDto Offer { get; set; }

    // Additional information gathered from the user
    public required List<CreateReservationCommandAdultInfoDto> AdultsInfo { get; set; }
    public required List<CreateReservationCommandChildInfoDto> ChildrenInfo { get; set; }

    // CF Turnstile token
    public required string Token { get; set; }
}
