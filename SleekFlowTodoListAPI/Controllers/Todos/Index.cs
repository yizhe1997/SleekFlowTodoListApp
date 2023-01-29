using AutoMapper;
using SleekFlowTodoListAPI.Controllers.Pagination;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Todos;

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
                return await CreateIndexResponseAsync<Todo, Model>(
                    request,
                    CurrentContext.Todos,
                    x => x.Name.Contains(request.SearchString) ||
                    ((TodoStatusEnum)x.Status).ToString().Contains(request.SearchString));
            }
        }
    }
}
