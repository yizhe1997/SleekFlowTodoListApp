using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Users;
using SleekFlowTodoListCore.Error;
using System.Net;
using System.Security.Claims;

namespace SleekFlowTodoListAPI.Controllers.Users.Authorize
{
    public static class AuthorizeServices
    {
        public static List<Claim> CreateClaimsForUser(this User? user, DatabaseContext Database, CurrentContext CurrentContext)
        {
            #region Validate user

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, "Unrecognized user");

            #endregion

            #region Claims creation

            var claims = new List<Claim>
            {
                new Claim("isAdmin", user.Admin ? "1" : "0")
            };

            #endregion

            return claims;
        }

    }
}
