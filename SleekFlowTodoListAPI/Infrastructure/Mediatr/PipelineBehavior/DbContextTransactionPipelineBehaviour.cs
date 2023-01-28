using SleekFlowTodoListCore.Domain.Contexts;

namespace SleekFlowTodoListAPI.Infrastructure.Mediatr.PipelineBehavior
{
    /// <summary>
    ///     Adds transaction to the processing pipeline
    /// </summary>
    public class DbContextTransactionPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly DatabaseContext _context;

        public DbContextTransactionPipelineBehaviour(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse result;

            try
            {
                _context.BeginTransaction();

                result = await next();

                _context.CommitTransaction();
            }
            catch (Exception)
            {
                _context.RollbackTransaction();
                throw;
            }

            return result;
        }
    }
}
