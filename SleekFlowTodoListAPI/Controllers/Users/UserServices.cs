using Microsoft.EntityFrameworkCore;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Users;
using SleekFlowTodoListCore.Error;
using System.Net;

namespace SleekFlowTodoListAPI.Controllers.Users
{
    public static class UserServices
    {
        /// <summary>
        ///     Get the requested user from db context. Throws RestException if not found
        /// </summary>
        public static async Task<User> GetUserFromDb(this DatabaseContext dbContext, Guid todoId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == todoId);
            if (user == null) 
                throw new RestException(HttpStatusCode.NotFound, "User not found.");

            return user;
        }
    }
}
