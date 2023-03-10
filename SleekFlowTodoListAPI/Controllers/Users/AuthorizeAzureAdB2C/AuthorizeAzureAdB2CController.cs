using MediatR;
using Microsoft.AspNetCore.Mvc;
using SleekFlowTodoListAPI.Controllers.ControllerTypes;

namespace SleekFlowTodoListAPI.Controllers.Users.AuthorizeAzureAdB2C
{
    public class AuthorizeAzureAdB2CController : ApiAzureAdB2CSecureController
    {
        public AuthorizeAzureAdB2CController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

		/// <remarks>
		///     Determines user access rights, currently only scope to check is "admin" rights. Produces the same token as /api/v1/Authorize POST service but requires azureAdB2CAuthenticateToken generated by Azure AD B2C identity provider.
		/// </remarks>
		/// <response code="200">User authorized</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpPost]
        public async Task<ActionResult<Create.Model>> PostAuthorizeAzureAdB2C([FromBody] Create.Request request) =>
            await _mediator.Send(request);

    }
}
