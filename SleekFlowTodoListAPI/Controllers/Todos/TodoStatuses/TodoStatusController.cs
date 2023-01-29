using MediatR;
using Microsoft.AspNetCore.Mvc;
using SleekFlowTodoListAPI.Controllers.ControllerTypes;
using SleekFlowTodoListAPI.Controllers.Pagination;

namespace SleekFlowTodoListAPI.Controllers.Todos.TodoStatuses
{
    public class TodoStatusController : ApiSecureController
    {
        public TodoStatusController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetTodoStatuses([FromQuery] Index.Request request) => await _mediator.Send(request);
    }
}
