using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.Business.Services.Interfaces;

public interface IGameHubService
{
    public Task<Game?> JoinGame(Guid userId, Guid gameId, string connectionId, CancellationToken cancellationToken = default);
    public Task<bool> IsPlayerExists(Guid userId, Guid gameId, CancellationToken cancellationToken = default);
    public Task<Game> StartGame(Guid gameId, CancellationToken cancellationToken = default);
    public Task GetPlayerAction(Guid gameId, Guid playerId, PlayerAction action, CancellationToken cancellationToken = default); 
    public Task<Game?> AddBotToLobby(Guid gameId, Guid userId, CancellationToken cancellationToken = default);
}