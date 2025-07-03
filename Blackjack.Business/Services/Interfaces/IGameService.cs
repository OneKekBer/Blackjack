using Blackjack.GameLogic.Models;

namespace Blackjack.Business.Services.Interfaces;

public interface IGameService
{
    public Task Create(CancellationToken cancellationToken = default);
    public Task DeleteAll(CancellationToken cancellationToken = default);
    Task<IEnumerable<Game>> GetAll(CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetPlayersConnectionIds(Guid gameId, CancellationToken cancellationToken = default);
}