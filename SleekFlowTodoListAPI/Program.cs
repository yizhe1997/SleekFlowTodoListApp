using FluentValidation.AspNetCore;
using FluentValidation;
using SleekFlowTodoListCore.Domain.Database;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SleekFlowTodoListCore.Domain.Contexts;
using Microsoft.AspNetCore.Identity;
using SleekFlowTodoListAPI.Infrastructure.Mediatr.PipelineBehavior;
using SleekFlowTodoListAPI.Infrastructure.Security.Authentication;
using SleekFlowTodoListAPI.Infrastructure.Security.Authorization;
using SleekFlowTodoListAPI.Infrastructure.Security.AzureAdB2C;
using SleekFlowTodoListAPI.Infrastructure.Security.Jwt;
using SleekFlowTodoListAPI.Infrastructure.Swagger;
using SleekFlowTodoListCore.Domain.Database.Users;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Rewrite;
using Serilog;
using SleekFlowTodoListCore.Error;
using SleekFlowTodoListAPI.Infrastructure.Mvc;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var logger = new LoggerConfiguration()
                       .ReadFrom.Configuration(builder.Configuration)
                       .Enrich.FromLogContext()
                       .CreateLogger();

#region Adding services to container

// Serilog
builder.Host.UseSerilog();

// Add cross-origin resource sharing to IServiceCollection
builder.Services.AddCors();

// try removing if not neccessary
builder.Services.AddMvc(opts =>
{
    opts.Filters.Add(typeof(ValidatorActionFilter));
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressInferBindingSourcesForParameters = true;
    options.SuppressModelStateInvalidFilter = true;
    options.SuppressMapClientErrors = true;
})
.AddNewtonsoftJson();

// Fluent Validation Ref: https://github.com/FluentValidation/FluentValidation/issues/1965
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(includeInternalTypes: true);

// Api Versioning
builder.Services.AddApiVersioning(opts => { opts.AssumeDefaultVersionWhenUnspecified = true; opts.ReportApiVersions = true; opts.DefaultApiVersion = new ApiVersion(1, 0); });
builder.Services.AddVersionedApiExplorer(opts => { opts.GroupNameFormat = "'v'VV"; opts.SubstituteApiVersionInUrl = true; opts.DefaultApiVersion = new ApiVersion(1, 0); });

// MediatR
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.FullName.StartsWith("Microsoft.VisualStudio.TraceDataCollector", StringComparison.Ordinal)).ToArray());

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.FullName.StartsWith("Microsoft.VisualStudio.TraceDataCollector", StringComparison.Ordinal)));

// EFCore
var conn = configuration.GetConnectionString("Primary");
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(conn);
    options.EnableSensitiveDataLogging(true);
});

// Seeding Db
builder.Services.AddDatabaseService(configuration);

// Current Context
builder.Services.AddTransient<CurrentContext>();

// Prereq for Iidentity... and to use usermanager
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

// Jwt            
builder.Services.AddJwt(configuration);

// AzureAdB2C            
builder.Services.AddAzureAdB2C(configuration);

// Authentication
builder.Services.AddAuthentications(configuration);

// Authorization
builder.Services.AddAuthorizations(configuration);

// Swagger            
builder.Services.AddSwaggerDocumentation(configuration);

// Add pipeline behaviour
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DbContextTransactionPipelineBehaviour<,>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region Scoped services

// Scoped services
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Database Service
    services.UseDatabaseService();
}

#endregion

#region MiddleWares

// Configure the HTTP request pipeline.
app.ConfigureExceptionHandler();

app.UseSerilogRequestLogging();
Log.Information("Initializing...");
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Make the default route redirect to Swagger documentation
var option = new RewriteOptions();
option.AddRedirect("^$", "docs");
app.UseRewriter(option);

// Expose Swagger documentation
app.UseSwaggerDocumentation(app.Services.GetRequiredService<IApiVersionDescriptionProvider>(), configuration);

app.MapControllers();

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always,
    HttpOnly = HttpOnlyPolicy.Always
});

#endregion

app.Run();
