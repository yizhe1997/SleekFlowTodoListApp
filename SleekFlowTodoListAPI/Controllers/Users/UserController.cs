using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleekFlowTodoListAPI.Controllers.ControllerTypes;
using SleekFlowTodoListAPI.Controllers.Pagination;

namespace SleekFlowTodoListAPI.Controllers.Users
{
	public class UserController : ApiAdminScopeController
	{
		public UserController(IMediator mediator, LinkGenerator linkGenerator) : base(mediator, linkGenerator)
		{
		}

		#region Admin Scope

		/// <remarks>
		///     Only users with admin access rights is authorized to use this service! The list does not return the record for admin user due to security concern.
		/// </remarks>
		/// <response code="200">Fetched the list of user</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[Authorize(Policy = "admin")]
		[HttpGet]
		public async Task<ActionResult<SearchResponse<Index.Model>>> GetUser([FromQuery] Index.Request request) => await _mediator.Send(request);

		/// <remarks>
		///     Deletes the user with the specified "id". Replace arg {id} with any "id" fetched from /api/v1/User GET service. Deleting admin user will throw an error! Only admin is authorized to delete any users!
		/// </remarks>
		/// <response code="200">Deleted the user</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[Authorize(Policy = "admin")]
		[HttpDelete("{id}")]
		public async Task<ActionResult<Delete.Model>> DeleteUser([FromRoute] Guid id) => await _mediator.Send(new Delete.Request { Id = id });

		#endregion

		/// <remarks>
		///     Returns the detail of a user with the specified "id". Replace arg {id} with any "id" fetched from /api/v1/User GET service. Admin can view all users detail but non admin users can only view their own details
		/// </remarks>
		/// <response code="200">Fetched the user detail</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpGet("{id}")]
		public async Task<ActionResult<Details.Model>> GetUser([FromRoute] Guid id, [FromQuery] Details.Request request)
		{
			request.Id = id;
			return await _mediator.Send(request);
		}

		/// <remarks>
		///     Creates a non-admin user with default password specified in the configuration of the API server
		/// </remarks>
		/// <response code="200">Created the user</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] Create.Request request)
		{
			var todoDetail = await _mediator.Send(request);

			return CreatedAtAction(nameof(GetUser), new { id = todoDetail.Id }, todoDetail);
		}

		/// <remarks>
		///     Updates the detail of a given user with the specified "id". Replace arg {id} with any "id" fetched from /api/v1/User GET service
		/// </remarks>
		/// <response code="200">Updated the user detail</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[HttpPut("{id}")]
		public async Task<ActionResult<Update.Model>> UpdateUser([FromRoute] Guid id, [FromBody] Update.Request request)
		{
			if (id != Guid.Empty) request.Id = id;
			return await _mediator.Send(request);
		}
	}
}
