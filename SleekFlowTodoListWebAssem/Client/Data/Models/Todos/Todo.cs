using SleekFlowTodoListWebAssem.Client.Data.Models.Abstracts;

namespace SleekFlowTodoListWebAssem.Client.Data.Models.Todos
{
	public class Todo : AuditableModel
	{
		public string? Name { get; set; }
		public string? Description { get; set; }
		public DateTime? DueDate { get; set; }
		public string? Status { get; set; }
		public Guid UserId { get; set; }
	}
}
