using AutoMapper;
using FluentValidation;
using MediatR;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Todos;
using SleekFlowTodoListCore.Extensions;

namespace SleekFlowTodoListAPI.Controllers.Todos
{
    public class Update
    {
        public class Request : IRequest<Model>
        {
            internal Guid Id { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public DateTime? DueDate { get; set; }
            public int Status { get; set; }
        }
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                var todoStatusNames = TypeExtension.GetEnumDataTypeValues<TodoStatusEnum>();
                RuleFor(x => x.Name).NotNull().NotEmpty();
                RuleFor(x => x.Status).NotEmpty().GreaterThan(todoStatusNames.Min()).LessThanOrEqualTo(todoStatusNames.Max());
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
                var todo = await CurrentContext.GetTodoForCurrentUserContext(request.Id);

                // Map input to todo and save the changes
                Mapper.Map(request, todo);
                await Database.SaveChangesAsync();

                return Mapper.Map<Model>(todo);
            }
        }
    }
}
