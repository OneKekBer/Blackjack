using Blackjack.GameLogic.Models;

namespace Blackjack.Business.Services.Interfaces;

public interface IGameHubService
{
    public Task<Game> JoinGame(Guid playerId,Guid gameId, string connectionId);
}