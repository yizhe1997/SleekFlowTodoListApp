namespace SleekFlowTodoListAPI.Controllers.Pagination
{
    public class SearchResponse<T> : PagedResponse<T>
    {
        public string? SearchString { get; set; }
    }
}
