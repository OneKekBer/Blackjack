using Blackjack.Data.Entities;

namespace Blackjack.Data.Repositories.Interfaces;

public interface IGameRepository : IRepository<GameEntity>
{
    public Task<IEnumerable<GameEntity>> GetAll(CancellationToken cancellationToken = default);
    public Task Attach(GameEntity entity, CancellationToken cancellationToken = default);
}