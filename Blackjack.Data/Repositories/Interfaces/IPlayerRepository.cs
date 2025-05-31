using Blackjack.Data.Entities;

namespace Blackjack.Data.Repositories.Interfaces;

public interface IPlayerRepository
{
    public Task Add(PlayerEntity entity);
    
    public Task<PlayerEntity> GetById(Guid id);
    
    public Task DeleteById(Guid id);
    
    public Task Update(PlayerEntity entity);
}