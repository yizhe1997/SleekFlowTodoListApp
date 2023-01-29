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

        [HttpGet]
        public async Task<ActionResult<SearchResponse<Index.Model>>> GetTodos([FromQuery] Index.Request request) => await _mediator.Send(request);

        [HttpGet("{id}")]
        public async Task<ActionResult<Details.Model>> GetTodo([FromRoute] Guid id, [FromQuery] Details.Request request)
        {
            request.Id = id;
            return await _mediator.Send(request);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] Create.Request request)
        {
            var todoDetail = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetTodo), new { id = todoDetail.Id }, todoDetail);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Update.Model>> UpdateTodo([FromRoute] Guid id, [FromBody] Update.Request request)
        {
            if (id != Guid.Empty) request.Id = id;
            return await _mediator.Send(request);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Delete.Model>> DeleteTodo([FromRoute] Guid id) => await _mediator.Send(new Delete.Request { Id = id });
    }
}

