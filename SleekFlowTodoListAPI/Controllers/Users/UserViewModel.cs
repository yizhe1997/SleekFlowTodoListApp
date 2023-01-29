using SleekFlowTodoListAPI.Controllers.ViewModel;

namespace SleekFlowTodoListAPI.Controllers.Users
{
	public class UserViewModel : AuditableViewModel
	{
		public string? DisplayName { get; set; }
		public string? Email { get; set; }
		public bool Admin { get; set; } = false;
		public Guid AzureAdB2CTokenSubClaim { get; set; }
		public DateTime? LastLoggedIn { get; set; }
	}
}
