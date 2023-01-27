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
        public bool Admin { get; set; } = false;
        public Guid AzureAdB2CTokenSubClaim { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        //public List<UserPasswordReset> UserPasswordResets { get; set; } = new List<UserPasswordReset>();
        public List<Todo> Todos { get; set; } = new List<Todo>();
    }
}
