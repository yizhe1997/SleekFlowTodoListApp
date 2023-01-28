using Microsoft.Identity.Web;

namespace SleekFlowTodoListAPI.Infrastructure.Security.Authorization
{
    public static class StartupExtensions
    {
        public static void AddAuthorizations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("admin", policy => policy.RequireClaim("isAdmin", "1"));
            });
            services.AddRequiredScopeAuthorization();
        }
    }
}
