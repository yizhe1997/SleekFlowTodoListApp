using AutoMapper;
using FluentValidation;
using MediatR;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Users;

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
			public RequestHandler(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor) : base(dbContext, mapper, httpContext, userAccessor)
			{
			}
			public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
			{
				// Map request
				var user = Mapper.Map<Request, User>(request);

				// Add todo to db and save
				Database.Users.Add(user);
				await Database.SaveChangesAsync();

				return Mapper.Map<Model>(user);
			}
		}
	}
}
