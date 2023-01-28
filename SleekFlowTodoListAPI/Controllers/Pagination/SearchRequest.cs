namespace SleekFlowTodoListAPI.Controllers.Pagination
{
    public abstract class SearchRequest<T> : PagedRequest<T>
    {
        public string SearchString { get; set; } = string.Empty;
        public string OrderBy { get; set; } = "id";
        public string? CreatedBy { get; set; }

    }
}
