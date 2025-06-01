using Blackjack.GameLogic;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Blackjack.Presentation.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Blackjack.Presentation.Hubs;

public class GameHubDispatcher : IGameHubDispatcher
{
    private readonly GameEngine _gameEngine;
    private readonly IHubContext<GameHub> _hubContext;
    
    public GameHubDispatcher(IHubContext<GameHub> hubContext)
    {
        _hubContext = hubContext;
        _gameEngine = new GameEngine(this, this);
    }
    
    public void ShowPlayerHand(Guid playerId, List<Card> cards, int score)
    {
        throw new NotImplementedException();
    }

    public void ShowResult(string message, IEnumerable<Player> players)
    {
        throw new NotImplementedException();
    }

    public PlayerAction GetPlayerAction(Guid playerId)
    {
        throw new NotImplementedException();
    }
}