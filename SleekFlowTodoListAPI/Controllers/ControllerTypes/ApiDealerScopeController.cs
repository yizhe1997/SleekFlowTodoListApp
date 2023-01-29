using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace SleekFlowTodoListAPI.Controllers.ControllerTypes
{
    [Authorize(Policy = "Dealer")]
    public class ApiDealerScopeController : ApiSecureController
    {
        public ApiDealerScopeController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }
    }
}
