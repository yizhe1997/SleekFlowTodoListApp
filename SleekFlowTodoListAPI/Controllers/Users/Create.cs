using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListAPI.Infrastructure.Security.Jwt;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database;
using SleekFlowTodoListCore.Domain.Database.Users;
using SleekFlowTodoListCore.Error;
using System.Net;

namespace SleekFlowTodoListAPI.Controllers.Users
{
	public class Create
	{
		public class Request : IRequest<Model>
		{
			public Guid AzureAdB2CTokenSubClaim { get; set; } = Guid.Empty;
			public string? DisplayName { get; set; }
			public string? Email { get; set; }
		}
		public class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
				RuleFor(x => x.DisplayName).NotNull().NotEmpty();
			}
		}
		public class Model : UserViewModel
		{
		}
		public class RequestHandler : BaseRequestHandler<Request, Model>
		{
			UserManager<User> UserManager { get; }
			DatabaseOptions StorageOptions { get; set; }
			JwtTokenGenerator JwtTokenGenerator { get; }
			public RequestHandler(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, UserManager<User> userManager, IOptions<DatabaseOptions> options, JwtTokenGenerator jwtTokenGenerator) : base(dbContext, mapper, httpContext, userAccessor)
			{
				UserManager = userManager;
				StorageOptions = options.Value;
				JwtTokenGenerator = jwtTokenGenerator;
			}
			public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
			{
				// Map request
				var user = Mapper.Map<Request, User>(request);

				// Create user and validate
				var createUserResult = UserManager.CreateAsync(user, StorageOptions.DefaultPassword).Result;
				if (!createUserResult.Succeeded)
					throw new RestException(HttpStatusCode.BadRequest, createUserResult.Errors.Select(e => e.Description).ToList());

				// Get new authorize token for client app
				var jwtToken = await JwtTokenGenerator.CreateToken(user.Id.ToString());

				// Mapping
				var result = Mapper.Map<Model>(user);
				result.AuthToken = jwtToken;

				return result;
			}
		}
	}
}
