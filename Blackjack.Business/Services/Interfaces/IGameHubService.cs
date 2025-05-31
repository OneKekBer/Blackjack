namespace Blackjack.Business.Services.Interfaces;

public interface IGameHubService
{
    public Task JoinGame(Guid playerId,Guid gameId, string connectionId);
}