using AutoMapper;
using MediatR;
using SleekFlowTodoListAPI.Infrastructure.Mediatr;
using SleekFlowTodoListCore.Domain.Contexts;

namespace SleekFlowTodoListAPI.Controllers.Todos
{
    public class Delete
    {
        public class Request : IRequest<Model>
        {
            internal Guid Id { get; set; }
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

                // Remove from todos table if record exit
                Database.Todos.Remove(todo);
                await Database.SaveChangesAsync();

                return Mapper.Map<Model>(todo);
            }
        }
    }
}
