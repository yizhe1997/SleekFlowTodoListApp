using SleekFlowTodoListAPI.Controllers.ViewModel;

namespace SleekFlowTodoListAPI.Controllers.Pagination
{
    public abstract class SearchRequest<T> : PagedRequest<T>
    {
        public string SearchString { get; set; } = string.Empty;
        public string OrderBy { get; set; } = nameof(BaseViewModel.Id);
        public string? CreatedBy { get; set; }

    }
}
