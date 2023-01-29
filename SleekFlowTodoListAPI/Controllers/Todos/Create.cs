using AutoMapper;
using FluentValidation;
using MediatR;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Todos;

namespace SleekFlowTodoListAPI.Controllers.Todos
{
    public class Create
    {
        public class Request : IRequest<Model>
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public DateTime? DueDate { get; set; }
            public int Status { get; set; } = (int)TodoStatusEnum.New;
        }
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
            }
        }
        public class Model : TodoViewModel
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
                var todo = Mapper.Map<Request, Todo>(request);

                // Assign todo to current user
                todo.UserId = CurrentContext.CurrentUserId;

                // Add todo to db and save
                Database.Todos.Add(todo);
                await Database.SaveChangesAsync();

                return Mapper.Map<Model>(todo);
            }
        }
    }
}
