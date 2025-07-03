using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.Presentation.View;

public class GameView
{
    public GameView(Game game)
    {
        Id = game.Id;
        CurrentTurn = game.TurnQueue.FirstOrDefault();
        Players = game.Players
            .Select(p => new PlayerView(p))
            .ToList();
        Status = game.Status;
        Deck = game.Deck.Count;
    }
    
    public Guid Id { get; set; }
    public Guid? CurrentTurn { get; set; }
    public List<PlayerView> Players { get; set; }
    public GameStatus Status { get; set; }
    public int Deck { get; set; }
}