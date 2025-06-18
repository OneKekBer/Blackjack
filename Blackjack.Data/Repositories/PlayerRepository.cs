using Blackjack.Data.Context;
using Blackjack.Data.Entities;
using Blackjack.Data.Other.Exceptions;
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

    public async Task Add(PlayerEntity entity, CancellationToken cancellationToken = default)
    {
        await _databaseContext.Players.AddAsync(entity, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PlayerEntity?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _databaseContext.Players
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

        return entity;
    }

    public async Task DeleteById(Guid id, CancellationToken cancellationToken = default)
    {
        var game = await GetById(id, cancellationToken) 
                   ?? throw new NotFoundInDatabaseException($"game with id: {id} has not been found PlayerRepository.DeleteById");
        
        _databaseContext.Players.Remove(game);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(PlayerEntity entity, CancellationToken cancellationToken = default)
    {
        _databaseContext.Players.Update(entity);
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Save(CancellationToken cancellationToken = default)
    {
        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}