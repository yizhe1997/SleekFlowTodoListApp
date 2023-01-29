using Microsoft.EntityFrameworkCore;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Todos;
using SleekFlowTodoListCore.Error;
using System.Net;

namespace SleekFlowTodoListAPI.Controllers.Todos
{
    public static class UserServices
    {
        /// <summary>
        ///     Get the requested todo from current user context. Throws RestException if not found
        /// </summary>
        public static async Task<Todo> GetTodoForCurrentUserContext(this CurrentContext currentContext, Guid todoId)
        {
            var todo = await currentContext.Todos.FirstOrDefaultAsync(dr => dr.Id == todoId);
            if (todo == null) throw new RestException(HttpStatusCode.NotFound, "Todo not found.");

            return todo;
        }
    }
}
