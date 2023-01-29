using AutoMapper;
using SleekFlowTodoListAPI.Controllers.Pagination;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListCore.Domain.Contexts;

namespace SleekFlowTodoListAPI.Controllers.Todos
{
	public class Index
    {
        public class Request : SearchRequest<SearchResponse<Model>>
        {
        }
        public class Model : TodoViewModel
        {
        }
        public class RequestHandler : SearchRequestHandler<Request, SearchResponse<Model>>
        {
            public RequestHandler(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<SearchResponse<Model>> Handle(Request request, CancellationToken cancellationToken)
            {
                // Convert Todo statuses to string format
                var query = Mapper.Map<List<TodoQueryableViewModel>>(CurrentContext.Todos).ToList().AsQueryable();

                return CreateIndexResponse<TodoQueryableViewModel, Model>(
                    request,
                    query,
                    x => x.Name.Contains(request.SearchString) ||
                    x.Description.Contains(request.SearchString) ||
                    x.Status.Contains(request.SearchString));
            }
        }
    }
}
