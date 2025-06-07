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
        var game = await _gameHubService.JoinGame(request.UserId, request.GameId, Context.ConnectionId);

        var connectionIds = game.Players
            .Where(p => !string.IsNullOrEmpty(p.ConnectionId))
            .Select(p => p.ConnectionId)
            .ToList();

        await Clients.Client(Context.ConnectionId).SendNewGame(game);
        await Clients.Clients(connectionIds).SendGameState(game);
    }

    public async Task StartGame(StartGameRequest request)
    {
        var game = await _gameHubService.StartGame(request.GameId);
        var connectionIds = game.Players
            .Where(p => !string.IsNullOrEmpty(p.ConnectionId))
            .Select(p => p.ConnectionId)
            .ToList();
        
        await Clients.Clients(connectionIds).SendGameState(game);
    }
    
    public async Task GetPlayerAction(GetPlayerActionRequest request)
    {
        await _gameHubService.GetPlayerAction(request.GameId, request.PlayerId, request.Action);
    }
}