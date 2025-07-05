using Blackjack.Data.Context;
using Blackjack.Data.Entities;
using Blackjack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Data.Repositories;

public class PlayerConnectionRepository : IPlayerConnectionRepository
{
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;

    public PlayerConnectionRepository(IDbContextFactory<DatabaseContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task Add(PlayerConnectionEntity entity, CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        await databaseContext.PlayerConnections.AddAsync(entity, cancellationToken);
    }

    public async Task Delete(PlayerConnectionEntity entity, CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        databaseContext.PlayerConnections.Remove(entity);
    }

    public async Task<PlayerConnectionEntity?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await databaseContext.PlayerConnections
            .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity;
    }

    public Task Save(PlayerConnectionEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<PlayerConnectionEntity?> GetByPlayerId(Guid id, CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await databaseContext.PlayerConnections
            .SingleOrDefaultAsync(e => e.PlayerId == id, cancellationToken);

        return entity;
    }
}