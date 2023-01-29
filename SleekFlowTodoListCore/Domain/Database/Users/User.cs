using Microsoft.AspNetCore.Identity;
using SleekFlowTodoListCore.Domain.Database.Todos;
using System.ComponentModel.DataAnnotations;

namespace SleekFlowTodoListCore.Domain.Database.Users
{
    // Microsoft.AspNetCore.Identity Tables:
    //+------------------+------------------+------------------+
    //|      Table       |   Description    |       Used       |
    //+------------------+------------------+------------------+
    //| AspNetUsers      | The users.       | Yes              | 
    //| AspNetRoles      | The roles.       | No               | 
    //| AspNetUserRoles  | Roles of users.  | No               | 
    //| AspNetUserClaims | Claims by users. | No               | 
    //| AspNetRoleClaims | Claims by roles. | No               | 
    //+------------------+------------------+------------------+

    /// <summary>
    ///     Add properties and use Microsoft.AspNetCore.Identity AspNetUsers table to store users. 
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        [Required]
        public string? DisplayName { get; set; }
        // Ref: https://stackoverflow.com/questions/26430094/asp-net-identity-provider-signinmanager-keeps-returning-failure
        public override string? UserName { get { return Email; } set { Email = value; } }
        public bool Admin { get; set; } = false;
        public Guid AzureAdB2CTokenSubClaim { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public List<Todo> Todos { get; set; } = new List<Todo>();
    }
}
