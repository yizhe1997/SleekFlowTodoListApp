using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Users;

namespace SleekFlowTodoListCore.Domain.EFCORE
{
    public static class StartupExtensions
    {
        public static void AddEFCore(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("Primary");
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(conn);
                options.EnableSensitiveDataLogging(true);
            });
        }
    }
}
