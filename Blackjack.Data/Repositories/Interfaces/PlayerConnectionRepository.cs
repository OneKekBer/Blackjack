using Blackjack.Data.Entities;

namespace Blackjack.Data.Repositories.Interfaces;

public interface IPlayerConnectionRepository : IRepository<PlayerConnectionEntity>
{
    public Task<PlayerConnectionEntity?> GetByPlayerId(Guid id, CancellationToken cancellationToken = default);
}