namespace SleekFlowTodoListAPI.Controllers.ViewModel
{
    public class EnumableViewModel : AuditableViewModel
    {
        public int Code { get; set; }
        public string? Name { get; set; }
    }
}
