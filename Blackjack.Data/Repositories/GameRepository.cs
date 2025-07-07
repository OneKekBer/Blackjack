using Blackjack.Data.Context;
using Blackjack.Data.Entities;
using Blackjack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blackjack.Data.Repositories;

public class GameRepository : IGameRepository
{
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;
    private readonly ILogger<GameRepository> _logger;
    
    public GameRepository(IDbContextFactory<DatabaseContext> contextFactory, ILogger<GameRepository> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task Attach(GameEntity entity, CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        databaseContext.Games.Attach(entity);
    }
    
    public async Task Add(GameEntity entity, CancellationToken cancellationToken = default)
    {   
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        
        await databaseContext.Games.AddAsync(entity, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task Save(GameEntity entity, CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        entity.UpdatedAt = DateTime.UtcNow;
        /*
            databaseContext.Games.Attach(entity);
            databaseContext.Update(entity);
        */
        await databaseContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<GameEntity?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var gameEntity = await databaseContext.Games
            .Include(g => g.Players)
            .SingleOrDefaultAsync((g) => g.Id == id, cancellationToken);
        
        return gameEntity;
    }
    
    public async Task<IEnumerable<GameEntity>> GetAll(CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var games = await databaseContext.Games
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return games;
    }
    
    public async Task Delete(GameEntity entity, CancellationToken cancellationToken = default)
    {
        var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        databaseContext.Games.Remove(entity);
        await databaseContext.SaveChangesAsync(cancellationToken);
    }
}



