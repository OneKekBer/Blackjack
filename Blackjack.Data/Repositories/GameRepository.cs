using Blackjack.Data.Context;
using Blackjack.Data.Entities;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        Console.WriteLine(_databaseContext.Games.Count().ToString());
        await _databaseContext.Games.AddAsync(entity);
        Console.WriteLine(_databaseContext.Games.Count().ToString());
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
            .ToListAsync();
        
        return games;
    }

    public async Task Update(GameEntity entity)
    {
        _databaseContext.Games.Update(entity);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task Save()
    {
        await _databaseContext.SaveChangesAsync();
    }

    public async Task Attach(GameEntity entity)
    {
        _databaseContext.Attach(entity);
        await _databaseContext.SaveChangesAsync();
    }
}