using Blackjack.Data.Context;
using Blackjack.Data.Other.Handlers.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blackjack.Data.Other.Handlers;

public class TransactionHandler : ITransactionHandler
{
    private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

    public TransactionHandler(IDbContextFactory<DatabaseContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IDbContextTransaction> StartTransaction(CancellationToken cancellationToken = default)
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        return transaction;
    }
}