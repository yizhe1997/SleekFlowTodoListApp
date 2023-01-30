using MediatR;
using Microsoft.AspNetCore.Mvc;
using SleekFlowTodoListAPI.Controllers.ControllerTypes;

namespace SleekFlowTodoListAPI.Controllers.Users.Authorize
{
    public class AuthorizeController : ApiSecureController
    {
        public AuthorizeController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

		/// <remarks>
		///     Determines user access rights, currently only scope to check is "admin" rights
		/// </remarks>
		/// <response code="200">User authorized</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpPost]
        public async Task<ActionResult<Create.Model>> PostAuthorize([FromBody] Create.Request request) =>
            await _mediator.Send(request);

    }
}
