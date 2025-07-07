using Microsoft.EntityFrameworkCore.Storage;

namespace Blackjack.Data.Other.Handlers.Base;

public interface ITransactionHandler
{
    public Task<IDbContextTransaction> StartTransaction(CancellationToken cancellationToken = default);

}