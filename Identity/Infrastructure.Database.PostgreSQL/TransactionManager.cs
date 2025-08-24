using Base.Infrastructure.Database.PostgreSQL;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database.PostgreSQL;

public class TransactionManager(
    MopereIdentityContext context, 
    ILogger<TransactionManager> logger
) : BaseTransactionManager(context, logger);