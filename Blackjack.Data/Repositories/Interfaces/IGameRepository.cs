using Blackjack.Data.Entities;

namespace Blackjack.Data.Repositories.Interfaces;

public interface IGameRepository
{
    public Task Add(GameEntity entity);
    
    public Task<GameEntity> GetById(Guid id);
    
    public Task<IEnumerable<GameEntity>> GetAll();
}