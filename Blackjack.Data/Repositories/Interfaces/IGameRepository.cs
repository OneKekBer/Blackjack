using Blackjack.Data.Entities;

namespace Blackjack.Data.Repositories.Interfaces;

public interface IGameRepository
{
    public Task Add(GameEntity entity, CancellationToken cancellationToken = default);
    
    public Task<GameEntity?> GetById(Guid id, CancellationToken cancellationToken = default);
    
    public Task<IEnumerable<GameEntity>> GetAll(CancellationToken cancellationToken = default);
    
    public Task Update(GameEntity entity, CancellationToken cancellationToken = default);
    public Task Save(CancellationToken cancellationToken = default);
}