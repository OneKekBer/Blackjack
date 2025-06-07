using Blackjack.Data.Interfaces;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Microsoft.AspNetCore.SignalR;

namespace Blackjack.Presentation.Hubs;

public class GameHubDispatcher : IGameHubDispatcher
{
    private readonly IHubContext<GameHub> _hubContext;
    private readonly IGameRepository _gameRepository;
    private readonly Dictionary<Guid, TaskCompletionSource<PlayerAction>> _pendingActions = new();
    
    public GameHubDispatcher(IHubContext<GameHub> hubContext, IGameRepository gameRepository)
    {
        _hubContext = hubContext;
        _gameRepository = gameRepository;
    }
    
    public async void ShowPlayerHand(Guid gameId, Guid playerId, List<Card> cards, int score) // maybe void is bad
    {
        var game = await _gameRepository.GetById(gameId);
        var connectionId = game.Players
            .Single(p => p.Id == playerId).ConnectionId;
        
        await _hubContext.Clients.Client(connectionId).SendAsync("SendPlayerHand", playerId, cards);
    }
    
    public async void ShowResult(Guid gameId,string message, IEnumerable<Player> players)
    {
        var connectionIds = players
            .Select(p => p.ConnectionId);
        
        await _hubContext.Clients.Clients(connectionIds).SendAsync("SendResult", gameId, message);
    }

    public async Task<PlayerAction> GetPlayerAction(Guid gameId, Guid playerId)
    {
        var tcs = new TaskCompletionSource<PlayerAction>();
        _pendingActions[playerId] = tcs;
        var action = await tcs.Task;
        
        return action;
    }

    public void SetPlayerAction(Guid playerId, PlayerAction action)
    {
        if (_pendingActions.TryGetValue(playerId, out var tcs))
        {
            tcs.SetResult(action);
            _pendingActions.Remove(playerId);
        }
    }
}