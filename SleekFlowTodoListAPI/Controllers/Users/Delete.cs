﻿using AutoMapper;
using MediatR;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListCore.Domain.Contexts;

namespace SleekFlowTodoListAPI.Controllers.Users
{
	public class Delete
	{
		public class Request : IRequest<Model>
		{
			internal Guid Id { get; set; }
		}
		public class Model : UserViewModel
		{
		}
		public class RequestHandler : BaseRequestHandler<Request, Model>
		{
			public RequestHandler(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
			{
			}
			public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
			{
				var user = await Database.GetUserFromDb(request.Id);

				// Remove from users table if record exit
				Database.Users.Remove(user);
				await Database.SaveChangesAsync();

				return Mapper.Map<Model>(user);
			}
		}
	}
}