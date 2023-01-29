namespace SleekFlowTodoListAPI.Infrastructure.Security.Jwt
{
    public static class StartupExtensions
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddTransient<JwtTokenGenerator>();
        }
    }
}
