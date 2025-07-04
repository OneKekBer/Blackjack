using System.Collections.Concurrent;
using Blackjack.Business.Mappers;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Interfaces;
using Blackjack.Data.Other.Exceptions;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Blackjack.Presentation.View;
using Microsoft.AspNetCore.SignalR;
using IServiceScopeFactory = Microsoft.Extensions.DependencyInjection.IServiceScopeFactory;

namespace Blackjack.Presentation.Hubs;

public class GameHubDispatcher : IGameHubDispatcher
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<GameHub> _hubContext;
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<PlayerAction>> _pendingActions = new(); // bullshit,
                                                                                                             // but i don`t know how to make better
    public GameHubDispatcher(IHubContext<GameHub> hubContext, IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _hubContext = hubContext;
    }
    
    public async Task ShowPlayerHand(Guid gameId, Guid playerId, List<Card> cards, int score) // maybe delete this
    {
        using var scope = _scopeFactory.CreateScope();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        
        var gameEntity = await gameRepository.GetById(gameId) 
                         ?? throw new NotFoundInDatabaseException("");
        
        var connectionId = gameEntity.Players
            .Single(p => p.Id == playerId).ConnectionId;
        
        await _hubContext.Clients.Client(connectionId).SendAsync("SendPlayerCards", gameId, playerId, cards, score);
    }
    
    public async Task SendResult(Guid gameId, string message, IEnumerable<Player> players) // maybe delete this
    {
        var connectionIds = players
            .Select(p => p.ConnectionId);
        
        await _hubContext.Clients.Clients(connectionIds).SendAsync("SendResult", gameId, message);
    }

    public async Task SendNewTurnPlayerId(Guid gameId, Guid currentPlayerId)
    {
        using var scope = _scopeFactory.CreateScope();
        var gameService = scope.ServiceProvider.GetRequiredService<IGameService>();

        var connectionIds = await gameService.GetPlayersConnectionIds(gameId);
            
        await _hubContext.Clients.Clients(connectionIds).SendAsync("SendNewTurnId", gameId, currentPlayerId);
    }

    public async Task SendGameState(Game game) // why i cant just take connectionsids from attribute ?
                                                //maybe current game dont have correct connection ids? because its not important in 
                                                //frontend and on reconnect new connectionid can be not updated in current game
    {
        using var scope = _scopeFactory.CreateScope();
        var gameService = scope.ServiceProvider.GetRequiredService<IGameService>(); 
        
        var connectionIds = await gameService.GetPlayersConnectionIds(game.Id);
        
        await _hubContext.Clients.Clients(connectionIds).SendAsync("SendGameState", new GameView(game));
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
            _pendingActions.Remove(playerId, out _);
            tcs.TrySetResult(action);
        }
    }

    public async Task SaveGame(Game game)
    {
        using var scope = _scopeFactory.CreateScope();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        
        await gameRepository.Save(GameMapper.ModelToEntity(game));
    }

    public async Task<Game> LoadGame(Guid gameId)
    {
        using var scope = _scopeFactory.CreateScope();
        var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
        
        var gameEntity = await gameRepository.GetById(gameId)
                         ?? throw new NotFoundInDatabaseException("");
        
        return GameMapper.EntityToModel(gameEntity);
    }
}