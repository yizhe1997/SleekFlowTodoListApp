using SleekFlowTodoListCore.Domain.Database.EntityTypes;
using System.ComponentModel.DataAnnotations;

namespace SleekFlowTodoListCore.Domain.Database.Todos
{
    public class TodoStatus : AuditableEntity
    {
        [Required]
        public int Name { get; set; }
    }

    [Flags]
    public enum TodoStatusNames
    {
        None = 0,
        New = 1,
        WIP = 2,
        Completed = 3,
        Cancelled = 4
    }
}
