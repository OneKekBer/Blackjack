

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

    public GameHub(IGameHubService gameHubService)
    {
        _gameHubService = gameHubService;
    }

    public async Task JoinGame(JoinGameRequest request)
    {
        await _gameHubService.JoinGame(request.PlayerId, request.GameId, Context.ConnectionId);
        
        
    }
}