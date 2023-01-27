using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SleekFlowTodoListCore.Domain.Database
{
    public static class StartupExtensions
    {
        public static void AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseOptions>(configuration.GetSection("Seed"));
            services.AddTransient<DatabaseService>();
        }

        /// <summary>
        ///     Startup service used for create, update and delete data in db
        /// </summary>
        public static void UseDatabaseService(this IServiceProvider provider)
        {
            var service = provider.GetRequiredService<DatabaseService>();
            service.UserAdmin();
            service.EnumValues();
        }
    }
}
