﻿using MediatR;
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

		// Only admin allowed to view other users details and delete users (not always the case but for the simplicity of this proj)
		[Authorize(Policy = "admin")]
		[HttpGet]
		public async Task<ActionResult<SearchResponse<Index.Model>>> GetUser([FromQuery] Index.Request request) => await _mediator.Send(request);

		[Authorize(Policy = "admin")]
		[HttpDelete("{id}")]
		public async Task<ActionResult<Delete.Model>> DeleteUser([FromRoute] Guid id) => await _mediator.Send(new Delete.Request { Id = id });

		#endregion

		[HttpGet("{id}")]
		public async Task<ActionResult<Details.Model>> GetUser([FromRoute] Guid id, [FromQuery] Details.Request request)
		{
			request.Id = id;
			return await _mediator.Send(request);
		}

		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] Create.Request request)
		{
			var todoDetail = await _mediator.Send(request);

			return CreatedAtAction(nameof(GetUser), new { id = todoDetail.Id }, todoDetail);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<Update.Model>> UpdateUser([FromRoute] Guid id, [FromBody] Update.Request request)
		{
			if (id != Guid.Empty) request.Id = id;
			return await _mediator.Send(request);
		}
	}
}
