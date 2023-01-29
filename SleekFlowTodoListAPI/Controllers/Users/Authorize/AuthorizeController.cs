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

        [HttpPost]
        public async Task<ActionResult<Create.Model>> PostAuthorize([FromBody] Create.Request request) =>
            await _mediator.Send(request);

    }
}
