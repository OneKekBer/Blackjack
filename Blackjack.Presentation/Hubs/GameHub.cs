using Blackjack.Business.Services.Interfaces;
using Blackjack.Presentation.Contracts.Requests;
using Blackjack.Presentation.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Blackjack.Presentation.Hubs;

// get: back <- front
// send: back -> front

public class GameHub : Hub<IGameHubClient>, IGameHub
{
    private readonly IGameHubService _gameHubService;
    private readonly ILogger<IGameHub> _logger;

    public GameHub(IGameHubService gameHubService, ILogger<IGameHub> logger)
    {
        _gameHubService = gameHubService;
        _logger = logger;
    }
    
    public async Task JoinGame(JoinGameRequest request)
    {
        _logger.LogInformation($"Joining game {request.GameId} {request.UserId}");
        var isPlayerExists = await _gameHubService.IsPlayerExists(request.UserId, request.GameId);
        var game = await _gameHubService.JoinGame(request.UserId, request.GameId, Context.ConnectionId, Context.ConnectionAborted);

        if (game is null)
        {
            await Clients.Client(Context.ConnectionId).SendError($"There is no game with id:{request.GameId}");
            return;
        }
        
        if (isPlayerExists)
        {
            await Clients.Client(Context.ConnectionId).SendGameState(game);
            return;
        }
        
        var connectionIds = game.Players
            .Where(p => !string.IsNullOrEmpty(p.ConnectionId))
            .Select(p => p.ConnectionId)
            .ToList();

        await Clients.Client(Context.ConnectionId).SendNewGame(game);
        await Clients.Clients(connectionIds).SendGameState(game);
    }

    public async Task StartGame(StartGameRequest request)
    {
        var game = await _gameHubService.StartGame(request.GameId, Context.ConnectionAborted);
        var connectionIds = game.Players
            .Where(p => !string.IsNullOrEmpty(p.ConnectionId))
            .Select(p => p.ConnectionId)
            .ToList();
        
        await Clients.Clients(connectionIds).SendGameState(game);
    }
    
    public async Task GetPlayerAction(GetPlayerActionRequest request)
    {
        _logger.LogInformation("Player action");
        await _gameHubService.GetPlayerAction(request.GameId, request.PlayerId, request.Action, Context.ConnectionAborted);
    }

    // public async Task Reconnect(ReconnectRequest request)
    // {
    //     var game = await _gameHubService.Reconnect(request.GameId, request.UserId, Context.ConnectionId, Context.ConnectionAborted);
    //
    //     if (game is null)
    //     {
    //         await Clients.Client(Context.ConnectionId).SendError($"Player with id {request.UserId} in game :{request.GameId} not found");
    //         return;
    //     }
    //     
    //     await Clients.Client(Context.ConnectionId).SendGameState(game);
    // }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}