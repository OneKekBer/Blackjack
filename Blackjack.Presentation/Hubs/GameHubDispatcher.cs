using System.Collections.Concurrent;
using Blackjack.Business.Mappers;
using Blackjack.Data.Interfaces;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Microsoft.AspNetCore.SignalR;

namespace Blackjack.Presentation.Hubs;

public class GameHubDispatcher : IGameHubDispatcher
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<GameHub> _hubContext;
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<PlayerAction>> _pendingActions = new();
    
    public GameHubDispatcher(IHubContext<GameHub> hubContext, IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _hubContext = hubContext;
    }
    
    public async void ShowPlayerHand(Guid gameId, Guid playerId, List<Card> cards, int score) // maybe void is bad
    {
        using var scope = _scopeFactory.CreateScope();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        
        var game = await gameRepository.GetById(gameId);
        var connectionId = game.Players
            .Single(p => p.Id == playerId).ConnectionId;
        
        await _hubContext.Clients.Client(connectionId).SendAsync("SendPlayerHand", playerId, cards);
    }
    
    public async void ShowResult(Guid gameId, string message, IEnumerable<Player> players)
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
        if(_pendingActions.TryGetValue(playerId, out var tcs))
        {
            tcs.TrySetResult(action);
            _pendingActions.TryRemove(playerId, out _);
        }
    }

    public async Task SaveGame(Game game)
    {
        using var scope = _scopeFactory.CreateScope();
        
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        await gameRepository.Save();
        // await gameRepository.Update(GameMapper.ModelToEntity(game));
    }

    public async Task SaveGameAndSendState(Game game)
    {
        using var scope = _scopeFactory.CreateScope();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        
        await gameRepository.Update(GameMapper.ModelToEntity(game));
        
        var connectionIds = game.Players
            .Select(p => p.ConnectionId);
        
        await _hubContext.Clients.Clients(connectionIds).SendAsync("SendGameState", game);  
    }
}