using System.ComponentModel.DataAnnotations;

namespace SleekFlowTodoListClient.Data.Models.Users
{
	public class UserRegister : UserLogin
	{
		[Required(ErrorMessage = "Confirm password is required.")]
		[DataType(DataType.Password)]
		[Compare("password", ErrorMessage = "Password and confirm password do not match.")]
		public string? confirmPassword{ get; set; }
	}
}
