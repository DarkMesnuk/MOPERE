using Base.Domain.Infrastructure.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Base.Infrastructure.Database.PostgreSQL;

public abstract class BaseTransactionManager(
    DbContext context, 
    ILogger<BaseTransactionManager> logger
) : ITransactionManager
{
    protected IDbContextTransaction? Transaction { get; private set; }
    
    
    public async Task StartTransactionAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Starting transaction with retry strategy");
        Transaction = await context.Database.BeginTransactionAsync(cancellationToken);   
    }
    
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Transaction != null)
        {
            logger.LogDebug("Committing transaction");
            await Transaction.CommitAsync(cancellationToken);
            logger.LogDebug("Transaction committed successfully");
            await Transaction.DisposeAsync();         
            Transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Transaction != null)
        {
            logger.LogError("Rolling back transaction");
            await Transaction.RollbackAsync(cancellationToken);
            await Transaction.DisposeAsync();      
            Transaction = null;
        }
    }
    
    public Task ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default)
    {
        var executionStrategy = context.Database.CreateExecutionStrategy();
        
        return executionStrategy.ExecuteAsync<object, bool>(
            state: null!,
            async (ctx, _, ct) =>
            {
                logger.LogDebug("Starting transaction with retry strategy");

                await using var transaction = await ctx.Database.BeginTransactionAsync(ct);
                try
                {
                    logger.LogDebug("Executing operation in transaction");
                    await operation();

                    logger.LogDebug("Committing transaction");
                    await transaction.CommitAsync(ct);

                    logger.LogDebug("Transaction committed successfully");
                    return true;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error in transaction, rolling back");
                    await transaction.RollbackAsync(ct);
                    throw;
                }
            },
            verifySucceeded: null,
            cancellationToken: cancellationToken
        );

    }

    public void Dispose()
    {
        context.Dispose();
        Transaction?.Dispose();
        GC.SuppressFinalize(this);
    }
}