using AutoMapper;
using SleekFlowTodoListAPI.Controllers.Pagination;
using SleekFlowTodoListAPI.Controllers.ViewModel;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Todos;
using SleekFlowTodoListCore.Extensions;
using System.Linq.Dynamic.Core;

namespace SleekFlowTodoListAPI.Controllers.Todos.TodoStatuses
{
    public class Index
    {
        public class Request : SearchRequest<SearchResponse<Model>>
        {
        }
        public class Model : TodoStatusViewModel
        {
        }
        public class RequestHandler : SearchRequestHandler<Request, SearchResponse<Model>>
        {
            public RequestHandler(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
            {
            }
            public override async Task<SearchResponse<Model>> Handle(Request request, CancellationToken cancellationToken)
            {
                SearchResponse<Model> response = new SearchResponse<Model>();

                var todoStatusDict = TypeExtension.GetEnumDataTypeDict<TodoStatusEnum>();

                var todoStatusViewModelList = Mapper.Map<List<Model>>(todoStatusDict);

                if (!string.IsNullOrEmpty(request.SearchString))
                {
                    var comparer = System.StringComparison.OrdinalIgnoreCase;

                    HashSet<Model> uniqueList = new HashSet<Model>();

                    // Filter by name
                    var todoStatusViewModelFilterName = todoStatusViewModelList.AsQueryable().Where(x => x.Name.Contains(request.SearchString, comparer)).ToList();
                    uniqueList.UnionWith(todoStatusViewModelFilterName);

                    todoStatusViewModelList = uniqueList.ToList();
                }

                // Change default request.orderBy to any Model field as EnumableViewModel does not have nameof(BaseViewModel.Id)
                if (request.OrderBy == nameof(BaseViewModel.Id))
                    request.OrderBy = nameof(Model.Code);

                todoStatusViewModelList = todoStatusViewModelList.AsQueryable().OrderBy(request.OrderBy).ToList();

                // Pagination 
                var items = todoStatusViewModelList.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

                var r = new PaginatedList<Model>(items, todoStatusViewModelList.Count, request.PageNumber, request.PageSize);
                response.SearchString = request.SearchString;
                response.OrderBy = request.OrderBy;
                response.Results = r.Results;
                response.PageNumber = r.PageNumber;
                response.PageSize = r.PageSize;
                response.TotalPages = r.TotalPages;
                response.TotalItems = r.TotalItems;

                return response;
            }
        }
    }
}
