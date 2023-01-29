using AutoMapper;
using MediatR;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListAPI.Infrastructure.Security.Jwt;
using SleekFlowTodoListCore.Domain.Contexts;

namespace SleekFlowTodoListAPI.Controllers.Users.Authorize
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
        }

        public class Model : AuthorizeViewModel
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
                // Create claims for user
                var claims = CurrentContext.CurrentUser.CreateClaimsForUser(Database, CurrentContext);

                // Create authorization token
                var token = await JwtTokenGenerator.CreateToken(CurrentContext.CurrentUserId.ToString() ?? string.Empty, claims);

                return new Model { AuthToken = token };
            }
        }
    }
}
