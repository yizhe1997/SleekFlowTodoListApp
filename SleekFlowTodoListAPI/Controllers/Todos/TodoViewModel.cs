using SleekFlowTodoListAPI.Controllers.ViewModel;

namespace SleekFlowTodoListAPI.Controllers.Todos
{
    public class TodoViewModel : AuditableViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
        public Guid UserId { get; set; }
    }

    public class TodoQueryableViewModel : TodoViewModel
    {
    }
}