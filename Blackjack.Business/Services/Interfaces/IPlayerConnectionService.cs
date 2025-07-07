namespace Blackjack.Business.Services.Interfaces;

public interface IPlayerConnectionService
{
    public Task ChangePlayerConnectionId(Guid playerId, string newConnectionId, CancellationToken cancellationToken = default);
    public Task<string> GetPlayerConnectionId(Guid playerId, CancellationToken cancellationToken = default);
    public Task<IEnumerable<string>> GetPlayerConnectionId(IEnumerable<Guid> playerId, 
        CancellationToken cancellationToken = default);
    public Task AddNewPlayerConnection(Guid playerId, string connectionId,
        CancellationToken cancellationToken = default);
}