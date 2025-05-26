using Blackjack.GameLogic.Types;

namespace Blackjack.GameLogic.Handlers;

public class BotHandler
{
    private readonly Random _random = new();
    
    public PlayerAction Logic(int score)
    {
        if (score <= 16) return PlayerAction.Hit;
        return PlayerAction.Stand;
    }
}