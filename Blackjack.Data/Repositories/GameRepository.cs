using Blackjack.Data.Context;
using Blackjack.Data.Entities;
using Blackjack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Data.Repositories;

public class GameRepository : IGameRepository
{
    private readonly DatabaseContext _databaseContext;
    
    public GameRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    public async Task Add(GameEntity entity, CancellationToken cancellationToken = default)
    {
        await _databaseContext.Games.AddAsync(entity, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<GameEntity> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var game = await _databaseContext.Games
            .Include(g => g.Players)
            .SingleAsync(g => g.Id == id, cancellationToken);
        
        return game;
    }

    public async Task<IEnumerable<GameEntity>> GetAll(CancellationToken cancellationToken = default)
    {
        var games = await _databaseContext.Games
            .ToListAsync(cancellationToken);
        
        return games;
    }

    public async Task Update(GameEntity entity, CancellationToken cancellationToken = default)
    {
        _databaseContext.Games.Update(entity);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Save(CancellationToken cancellationToken = default)
    {
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}