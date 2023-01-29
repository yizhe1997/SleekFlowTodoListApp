using AutoMapper;
using MediatR;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Error;
using System.Net;

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

				// Forbid deletion of admin user
				if (user.Admin == true)
					throw new RestException(HttpStatusCode.NotFound, "Admin user not allowed for deletion.");

				// Remove from users table if record exit
				Database.Users.Remove(user);
				await Database.SaveChangesAsync();

				return Mapper.Map<Model>(user);
			}
		}
	}
}
