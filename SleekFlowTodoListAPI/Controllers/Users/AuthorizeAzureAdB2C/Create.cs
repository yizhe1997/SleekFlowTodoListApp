using AutoMapper;
using FluentValidation;
using MediatR;
using SleekFlowTodoListAPI.Controllers.Users.Authorize;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListAPI.Infrastructure.Security.Jwt;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Error;
using System.Net;

namespace SleekFlowTodoListAPI.Controllers.Users.AuthorizeAzureAdB2C
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
            public Guid AzureAdB2CTokenSubClaim { get; set; }
        }

        public class Model : AuthorizeAzureAdB2CViewModel
		{
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            JwtTokenGenerator JwtTokenGenerator { get; }

            public RequestHandler(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, JwtTokenGenerator jwtTokenGenerator) : base(dbContext, mapper, httpContext, userAccessor)
            {
                JwtTokenGenerator = jwtTokenGenerator;
            }

            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                // Check if current user AzureAdB2CTokenSubClaim is empty Guid. If no forbid this method of authorization.
				if (CurrentContext.CurrentUser.AzureAdB2CTokenSubClaim == Guid.Empty)
					throw new RestException(HttpStatusCode.NotFound, "User not registered for Azure AD B2C authorization. Please authorize conventionally.");

				// Create claims for user
				var claims = CurrentContext.CurrentUser.CreateClaimsForUser(Database, CurrentContext);

                // Create authorization token
                var token = await JwtTokenGenerator.CreateToken(CurrentContext.CurrentUserId.ToString() ?? string.Empty, claims);

                return new Model { AuthToken = token };
            }
        }
    }
}
