using Blackjack.Data.Context;
using Blackjack.Data.Entities;
using Blackjack.Data.Other.Exceptions;
using Blackjack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Data.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;
    
    public PlayerRepository(IDbContextFactory<DatabaseContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task Add(PlayerEntity entity, CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        
        await databaseContext.Players.AddAsync(entity, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PlayerEntity?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var entity = await databaseContext.Players
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

        return entity;
    }

    public async Task DeleteById(Guid id, CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var game = await GetById(id, cancellationToken) 
                   ?? throw new NotFoundInDatabaseException($"game with id: {id} has not been found PlayerRepository.DeleteById");
        
        databaseContext.Players.Remove(game);
        await databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(PlayerEntity entity, CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        databaseContext.Players.Update(entity);
        await databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Save(CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        await databaseContext.SaveChangesAsync(cancellationToken);
    }
}