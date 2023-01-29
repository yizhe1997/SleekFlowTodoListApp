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

        [HttpPost]
        public async Task<ActionResult<Create.Model>> PostAuthorizeAzureAdB2C([FromBody] Create.Request request) =>
            await _mediator.Send(request);

    }
}
