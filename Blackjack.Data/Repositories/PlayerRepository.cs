using Blackjack.Data.Context;
using Blackjack.Data.Entities;
using Blackjack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Data.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly DatabaseContext _databaseContext;
    
    public PlayerRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task Add(PlayerEntity entity)
    {
        await _databaseContext.Players.AddAsync(entity);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<PlayerEntity> GetById(Guid id)
    {
        var entity = await _databaseContext.Players
            .SingleAsync(p => p.Id == id);
        
        return entity;
    }

    public async Task DeleteById(Guid id)
    {
        var game = await GetById(id);
        
        _databaseContext.Players.Remove(game);
        await _databaseContext.SaveChangesAsync();
    }
}