using Blackjack.Business.Extensions;
using Blackjack.Business.Services.Interfaces;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Blackjack.Presentation.Contracts.Requests;
using Blackjack.Presentation.Hubs.Interfaces;
using Blackjack.Presentation.View;
using Microsoft.AspNetCore.SignalR;

namespace Blackjack.Presentation.Hubs;

// get: back <- front
// send: back -> front

public class GameHub : Hub<IGameHubClient>, IGameHub
{
    private readonly IGameHubService _gameHubService;
    private readonly IPlayerConnectionService _playerConnectionService;
    private readonly ILogger<IGameHub> _logger;
    
    public GameHub(IGameHubService gameHubService, IPlayerConnectionService playerConnectionService, ILogger<IGameHub> logger)
    {
        _gameHubService = gameHubService;
        _playerConnectionService = playerConnectionService;
        _logger = logger;
    }
    
    public async Task JoinGame(JoinGameRequest request)
    {
        _logger.LogInformation($"Joining game {request.GameId} {request.UserId} actual coonectionId: {Context.ConnectionId}");
        var isPlayerExists = await _gameHubService.IsPlayerExists(request.UserId, request.GameId);
        var game = await _gameHubService.JoinGame(request.UserId, request.GameId, Context.ConnectionId, Context.ConnectionAborted);

        if (game is null)
        {
            await Clients.Client(Context.ConnectionId).SendError($"There is no game with id:{request.GameId}");
            return;
        }
        
        if (isPlayerExists) // maybe cringe
        {
            await Clients.Client(Context.ConnectionId).SendGameState(new GameView(game));
            return;
        }
        
        var connectionIds = await _playerConnectionService.GetPlayerConnectionId(game.Players.GetPlayersIds());

        await Clients.Client(Context.ConnectionId).SendNewGame(new GameView(game));
        await Clients.Clients(connectionIds).SendGameState(new GameView(game));
    }

    public async Task StartGame(StartGameRequest request)
    {
        var game = await _gameHubService.StartGame(request.GameId, Context.ConnectionAborted);
        
        var connectionIds = await _playerConnectionService.GetPlayerConnectionId(game.Players.GetPlayersIds());
        
        await Clients.Clients(connectionIds).SendGameState(new GameView(game));
    }
    
    public async Task GetPlayerAction(GetPlayerActionRequest request)
    {
        _logger.LogInformation("Player action");
        await _gameHubService.GetPlayerAction(request.GameId, request.PlayerId, request.Action, Context.ConnectionAborted);
    }
    
    public async Task AddBotToLobby(AddBotToLobbyRequest request)
    {
        var game = await _gameHubService.AddBotToLobby(request.GameId, request.PlayerId, Context.ConnectionAborted);
        if (game == null)
            await Clients.Client(Context.ConnectionId).SendError($"You cant add bot to lobby, or lobby is full, or game is started");

       
        var connectionIds = await _playerConnectionService.GetPlayerConnectionId(game.Players.GetPlayersIds());
        
        await Clients.Clients(connectionIds).SendGameState(new GameView(game));
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}