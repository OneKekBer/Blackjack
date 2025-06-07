using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.Business.Services.Interfaces;

public interface IGameHubService
{
    public Task<Game> JoinGame(Guid userId, Guid gameId, string connectionId);
    public Task<Game> StartGame(Guid gameId);
    public Task GetPlayerAction(Guid gameId, Guid playerId, PlayerAction action);
}