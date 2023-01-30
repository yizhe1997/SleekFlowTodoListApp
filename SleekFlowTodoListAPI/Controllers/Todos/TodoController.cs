using MediatR;
using Microsoft.AspNetCore.Mvc;
using SleekFlowTodoListAPI.Controllers.ControllerTypes;
using SleekFlowTodoListAPI.Controllers.Pagination;

namespace SleekFlowTodoListAPI.Controllers.Todos
{
    public class TodoController : ApiSecureController
    {
        public TodoController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
        }

		/// <remarks>
		///     Returns the list of TODO for the current user
		/// </remarks>
		/// <response code="200">Fetched the list of TODO</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetTodos([FromQuery] Index.Request request) => await _mediator.Send(request);

		/// <remarks>
		///     Returns the detail of a TODO only from the user's TODO list. Replace arg {id} with any "id" fetched from /api/v1/Todo GET service
		/// </remarks>
		/// <response code="200">Fetched the TODO detail</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpGet("{id}")]
        public async Task<ActionResult<Details.Model>> GetTodo([FromRoute] Guid id, [FromQuery] Details.Request request)
        {
            request.Id = id;
            return await _mediator.Send(request);
        }

		/// <remarks>
		///     Creates a TODO record for the current authorized user
		/// </remarks>
		/// <response code="200">Created the TODO</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] Create.Request request)
        {
            var todoDetail = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetTodo), new { id = todoDetail.Id }, todoDetail);
        }

		/// <remarks>
		///     Updates the detail of a TODO only from the user's TODO list. Replace arg {id} with any "id" fetched from /api/v1/Todo GET service
		/// </remarks>
		/// <response code="200">Updated the TODO detail</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpPut("{id}")]
        public async Task<ActionResult<Update.Model>> UpdateTodo([FromRoute] Guid id, [FromBody] Update.Request request)
        {
            if (id != Guid.Empty) request.Id = id;
            return await _mediator.Send(request);
        }

		/// <remarks>
		///     Deletes the detail of a TODO only from the user's TODO list. Replace arg {id} with any "id" fetched from /api/v1/Todo GET service
		/// </remarks>
		/// <response code="200">Deleted the TODO</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpDelete("{id}")]
        public async Task<ActionResult<Delete.Model>> DeleteTodo([FromRoute] Guid id) => await _mediator.Send(new Delete.Request { Id = id });
    }
}

