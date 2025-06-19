using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.Presentation.View;

public class GameView
{
    public Guid Id { get; set; }
    public Guid? CurrentTurn { get; set; }
    public List<Player> Players { get; set; }
    public GameStatus Status { get; set; }

    public GameView(Game game)
    {
        Id = game.Id;
        CurrentTurn = game.TurnQueue.LastOrDefault();
        Players = game.Players;
        Status = game.Status;
    }
}