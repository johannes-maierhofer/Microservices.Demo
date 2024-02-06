using BuildingBlocks.EfCore;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Mediatr.Behaviors
{
    public class CommandTransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, ICommand<TResponse> 
        where TResponse : notnull
    {
        private readonly IDbContext _dbContext;

        public CommandTransactionBehavior(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // for Connection Resiliency
            // see https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency

            var response = default(TResponse);

            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(
                async () =>
                {
                    await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

                    response = await next();

                    await transaction.CommitAsync(cancellationToken);
                });

            return response!;
        }
    }
}