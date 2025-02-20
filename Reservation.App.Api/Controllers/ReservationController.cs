using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservation.App.Application.Exceptions;
using Reservation.App.Application.Features.Reservations.Commands.ChangeReservationAgent;
using Reservation.App.Application.Features.Reservations.Commands.ChangeReservationStatus;
using Reservation.App.Application.Features.Reservations.Commands.CreateReservation;
using Reservation.App.Application.Features.Reservations.Queries.GetReservationListWithUserAndPayment;
using Reservation.App.Application.RequestHelpers;

namespace Reservation.App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<
            ActionResult<GetReservationListWithUserAndPaymentQueryResponse>
        > GetReservations(
            [FromQuery] PaginationParameters paginationParameters,
            [FromQuery] SearchParameters searchParameters,
            [FromQuery] SortParameters sortParameters,
            [FromQuery] FilterParameters filterParameters
        )
        {
            var response = await _mediator.Send(
                new GetReservationListWithUserAndPaymentQuery()
                {
                    PaginationParameters = paginationParameters,
                    SearchParameters = searchParameters,
                    SortParameters = sortParameters,
                    FilterParameters = filterParameters,
                }
            );

            return Ok(response);
        }

        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ChangeReservationStatus(
            int id,
            ChangeReservationStatusCommand command
        )
        {
            if (id != command.Id)
            {
                throw new BadRequestException(
                    "The id in the route does not match the id in the command."
                );
            }

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPatch("{id}/agent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ChangeReservationAgent(
            int id,
            ChangeReservationAgentCommand command
        )
        {
            if (id != command.Id)
            {
                throw new BadRequestException(
                    "The id in the route does not match the id in the command."
                );
            }

            await _mediator.Send(command);

            return NoContent();
        }

        [AllowAnonymous]
        // [EnableRateLimiting("CreateReservationLimiter")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateReservation(CreateReservationCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
