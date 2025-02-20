using Microsoft.AspNetCore.Mvc;
using Reservation.App.Application.Contracts.Identity;
using Reservation.App.Application.Models.Authentication;

namespace Reservation.App.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> Authenticate(
            AuthenticationRequest request
        )
        {
            return Ok(await _authenticationService.AuthenticateAsync(request));
        }
    }
}
