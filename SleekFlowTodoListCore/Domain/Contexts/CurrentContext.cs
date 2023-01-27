using Microsoft.AspNetCore.Http;
using SleekFlowTodoListCore.Domain.Database.Todos;
using SleekFlowTodoListCore.Domain.Database.Users;
using System.Security.Claims;

namespace SleekFlowTodoListCore.Domain.Contexts
{
    public class CurrentContext
    {
        private readonly DatabaseContext _dbContext;
        private readonly HttpContext? _httpContext;

        public CurrentContext(DatabaseContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public IEnumerable<Claim>? Claims => _httpContext?.User.Claims ?? default;
        public string NameClaim => Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        #region Current User Context

        public Guid CurrentUserId => string.IsNullOrEmpty(NameClaim) ? Guid.Empty : Guid.Parse(NameClaim);
        public User? CurrentUser => _dbContext.Users.FirstOrDefault(u => u.Id == CurrentUserId);

        #endregion

        #region Current Dealer Context

        public Guid CurrentDealerId => Claims != null ? Guid.TryParse(Claims.FirstOrDefault(x => x.Type == "dealerId")?.Value, out Guid result) ? result : Guid.Empty : Guid.Empty;
        public IQueryable<Todo> CurrentDealerMemberships => _dbContext.Todos.Where(m => m.UserId == CurrentDealerId);

        #endregion
    }
}
