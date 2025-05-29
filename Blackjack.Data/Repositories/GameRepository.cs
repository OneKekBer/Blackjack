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
    
    public async Task Add(GameEntity entity)
    {
        await _databaseContext.Games.AddAsync(entity);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<GameEntity> GetById(Guid id)
    {
        var game = await _databaseContext.Games
            .SingleAsync(g => g.Id == id);
        
        return game;
    }

    public async Task<IEnumerable<GameEntity>> GetAll()
    {
        var games = await _databaseContext.Games
            .AsNoTracking()
            .ToListAsync();
        
        return games;
    }
}