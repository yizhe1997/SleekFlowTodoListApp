using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace SleekFlowTodoListAPI.Controllers.ControllerTypes
{
    [Authorize(Policy = "admin")]
    public class ApiAdminScopeController : ApiSecureController
    {
        public ApiAdminScopeController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }
    }
}
