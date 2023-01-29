using AutoMapper;
using SleekFlowTodoListAPI.Controllers.Pagination;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Users;

namespace SleekFlowTodoListAPI.Controllers.Users
{
	public class Index
	{
		public class Request : SearchRequest<SearchResponse<Model>>
		{
		}
		public class Model : UserViewModel
		{
		}
		public class RequestHandler : SearchRequestHandler<Request, SearchResponse<Model>>
		{
			public RequestHandler(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
			{
			}
			public override async Task<SearchResponse<Model>> Handle(Request request, CancellationToken cancellationToken)
			{
				// Only get non-admin users to prevent self-deletion from client app
				var query = Database.Users.Where(x => x.Admin == false).AsQueryable();

				return await CreateIndexResponseAsync<User, Model>(
					request,
					query,
					x => x.DisplayName.Contains(request.SearchString) ||
					x.Email.Contains(request.SearchString));
			}
		}
	}
}
