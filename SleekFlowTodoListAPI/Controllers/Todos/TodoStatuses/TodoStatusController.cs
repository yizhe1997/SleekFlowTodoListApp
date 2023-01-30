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

		/// <remarks>
		///     Return the list of plausible TODO statuses! The "code" value will be used as the int value of "status" for /api/v1/Todo/{id} PUT service.
		/// </remarks>
		/// <response code="200">Fetched the list of plausible TODO statuses</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetTodoStatuses([FromQuery] Index.Request request) => await _mediator.Send(request);
    }
}
