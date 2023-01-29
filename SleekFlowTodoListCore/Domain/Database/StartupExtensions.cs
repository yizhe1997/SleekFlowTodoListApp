using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SleekFlowTodoListCore.Domain.Contexts;

namespace SleekFlowTodoListCore.Domain.Database
{
    public static class StartupExtensions
    {
        public static void AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseOptions>(configuration.GetSection("Database"));
            services.AddTransient<DatabaseService>();
        }

        /// <summary>
        ///     Startup service used for create, update and delete data in db.
        /// </summary>
        public static void UseDatabaseService(this IServiceProvider provider)
        {
            // Make sure the latest EFCore migration is applied everytime the API is initiated
            var dbContext = provider.GetRequiredService<DatabaseContext>();
            dbContext.Database.Migrate();

            // Applied before migration so that DatabaseService transaction can take place for new/existing DB
            var service = provider.GetRequiredService<DatabaseService>();
            service.UserAdmin();
        }
    }
}
