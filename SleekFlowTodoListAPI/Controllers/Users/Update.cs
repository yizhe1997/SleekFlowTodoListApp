using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListAPI.Infrastructure.Security.Jwt;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Users;
using SleekFlowTodoListCore.Error;
using System.Net;

namespace SleekFlowTodoListAPI.Controllers.Users
{
	public class Update
	{
		public class Request : IRequest<Model>
		{
			internal Guid Id { get; set; }
			public string? DisplayName { get; set; }
			public string? Email { get; set; }
			public Guid AzureAdB2CTokenSubClaim { get; set; } = Guid.Empty;
			public string? Password { get; set; }
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
			JwtTokenGenerator JwtTokenGenerator { get; }
			public RequestHandler(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, UserManager<User> userManager, JwtTokenGenerator jwtTokenGenerator) : base(dbContext, mapper, httpContext, userAccessor)
			{
				UserManager = userManager;
				JwtTokenGenerator = jwtTokenGenerator;
			}
			public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
			{
				var user = await Database.GetUserFromDb(request.Id);

				// Update password and validate
				var token = await UserManager.GeneratePasswordResetTokenAsync(user);
				var createUserResult = await UserManager.ResetPasswordAsync(user, token, request.Password);

				if (!createUserResult.Succeeded)
					throw new RestException(HttpStatusCode.BadRequest, createUserResult.Errors.Select(e => e.Description).ToList());

				// Map input to user and save the changes
				Mapper.Map(request, user);
				await Database.SaveChangesAsync();

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
