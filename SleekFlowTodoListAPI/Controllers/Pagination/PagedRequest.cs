using MediatR;

namespace SleekFlowTodoListAPI.Controllers.Pagination
{
    public class PagedRequest<T> : IPaginationParams, IRequest<T>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
