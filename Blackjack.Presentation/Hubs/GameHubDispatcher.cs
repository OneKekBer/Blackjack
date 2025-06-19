using System.Collections.Concurrent;
using Blackjack.Business.Mappers;
using Blackjack.Data.Interfaces;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Blackjack.Presentation.View;
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
    
    public async Task ShowPlayerHand(Guid gameId, Guid playerId, List<Card> cards, int score) // maybe delete this
    {
        using var scope = _scopeFactory.CreateScope();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        
        var gameEntity = await gameRepository.GetById(gameId);
        var connectionId = gameEntity.Players
            .Single(p => p.Id == playerId).ConnectionId;
        
        await _hubContext.Clients.Client(connectionId).SendAsync("SendGameState", new GameView(GameMapper.EntityToModel(gameEntity)));
    }
    
    public async Task ShowResult(Guid gameId, string message, IEnumerable<Player> players) // maybe delete this
    {
        var connectionIds = players
            .Select(p => p.ConnectionId);
        
        await _hubContext.Clients.Clients(connectionIds).SendAsync("SendResult", gameId, message);
    }

    public async Task ShowNewTurnPlayerId(Guid gameId, Guid currentPlayerId)
    {
        using var scope = _scopeFactory.CreateScope();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();

        var gameEntity = await gameRepository.GetById(gameId);
        var connectionIds = gameEntity.Players
            .Select(p => p.ConnectionId);
        
        await _hubContext.Clients.Clients(connectionIds).SendAsync("SendNewTurnId", gameId, currentPlayerId);
    }

    public async Task SendGameState(Game game)
    {
        using var scope = _scopeFactory.CreateScope();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        
        var gameEntity = await gameRepository.GetById(game.Id);
        var connectionIds = gameEntity.Players
            .Select(p => p.ConnectionId);
        
        await _hubContext.Clients.Clients(connectionIds).SendAsync("SendGameState", new GameView(GameMapper.EntityToModel(gameEntity)));
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

        var gameEntity = GameMapper.ModelToEntity(game);
        await gameRepository.Update(gameEntity);
    }

    public async Task SaveGameAndSendState(Game game)
    {
        using var scope = _scopeFactory.CreateScope();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        
        await gameRepository.Update(GameMapper.ModelToEntity(game));
        
        var connectionIds = game.Players
            .Select(p => p.ConnectionId);
        
        await _hubContext.Clients.Clients(connectionIds).SendAsync("SendGameState", new GameView(game));  
    }
}