using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservation.App.Application.Exceptions;
using Reservation.App.Application.Features.Payments.Commands.CreatePaymentForReservation;
using Reservation.App.Application.Features.Payments.Queries.GetPaymentsForReservation;

namespace Reservation.App.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/reservations/{id}/payments")]
public class ReservationPaymentController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetPaymentsForReservationQueryResponse>> GetReservationPayments(
        int id
    )
    {
        var payments = await _mediator.Send(
            new GetPaymentsForReservationQuery { ReservationId = id }
        );

        return Ok(payments);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreteReservationPayment(
        int id,
        CreatePaymentForReservationCommand command
    )
    {
        if (id != command.ReservationId)
        {
            throw new BadRequestException(
                "The id in the route does not match the id in the command."
            );
        }

        await _mediator.Send(command);

        return NoContent();
    }
}
