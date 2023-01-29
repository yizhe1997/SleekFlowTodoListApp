using AutoMapper;
using FluentValidation;
using MediatR;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListCore.Domain.Contexts;

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
				var user = await Database.GetUserFromDb(request.Id);

				// Map input to user and save the changes
				Mapper.Map(request, user);
				await Database.SaveChangesAsync();

				return Mapper.Map<Model>(user);
			}
		}
	}
}
