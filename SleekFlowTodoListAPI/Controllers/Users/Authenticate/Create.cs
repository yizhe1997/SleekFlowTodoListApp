using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListAPI.Infrastructure.Security.Jwt;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Users;
using SleekFlowTodoListCore.Error;
using System.Net;

namespace SleekFlowTodoListAPI.Controllers.Users.Authenticate
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }

        public class Model : AuthenticateViewModel
        {
        }

        public class RequestHandler : BaseRequestHandler<Request, Model>
        {
            JwtTokenGenerator JwtTokenGenerator { get; }
            SignInManager<User> SignInManager { get; }

            public RequestHandler(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, CurrentContext userAccessor, SignInManager<User> signInManager, JwtTokenGenerator jwtTokenGenerator) : base(dbContext, mapper, httpContext, userAccessor)
            {
                JwtTokenGenerator = jwtTokenGenerator;
                SignInManager = signInManager;
            }

            public override async Task<Model> Handle(Request request, CancellationToken cancellationToken)
            {
                // Default error msg
                var errorMsg = "Incorrect username & password combination.";

                // Attempt to authenticate email and pass
                var result = await SignInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await Database.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                    if (user == null)
                        throw new RestException(HttpStatusCode.Unauthorized, errorMsg);

                    user.LastLoggedIn = DateTime.UtcNow;

                    await Database.SaveChangesAsync();

                    return new Model { AuthToken = await JwtTokenGenerator.CreateToken(user.Id.ToString()) };
                }

                // Set error msg if authentication failed based on signing result
                if (result.IsLockedOut)
                {
                    errorMsg = "User locked out.";
                }
                else if (result.IsNotAllowed)
                {
                    errorMsg = "User not allowed to authenticate.";
                }
                else if (result.RequiresTwoFactor)
                {
                    errorMsg = "Two-factor authentication required.";
                }

                throw new RestException(HttpStatusCode.Unauthorized, errorMsg);
            }
        }
    }
}