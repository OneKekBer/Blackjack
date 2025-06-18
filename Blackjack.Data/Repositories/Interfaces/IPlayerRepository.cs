using Blackjack.Data.Entities;

namespace Blackjack.Data.Repositories.Interfaces;

public interface IPlayerRepository
{
    public Task Add(PlayerEntity entity, CancellationToken cancellationToken = default);
    
    public Task<PlayerEntity?> GetById(Guid id, CancellationToken cancellationToken = default);
    
    public Task DeleteById(Guid id, CancellationToken cancellationToken = default);
    
    public Task Update(PlayerEntity entity, CancellationToken cancellationToken = default);
    public Task Save(CancellationToken cancellationToken = default);
}