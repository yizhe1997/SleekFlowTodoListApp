using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleekFlowTodoListAPI.Controllers.ControllerTypes;

namespace SleekFlowTodoListAPI.Controllers.Users.Authenticate
{
    public class AuthenticateController : ApiBaseController
    {
        public AuthenticateController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

		/// <remarks>
		///     Verifies the identity of a user in DB
		/// </remarks>
		/// <response code="200">User authenticated</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Create.Model>> PostAuthenticate([FromBody] Create.Request request) =>
            await _mediator.Send(request);
    }
}
