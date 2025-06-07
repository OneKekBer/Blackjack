using Blackjack.GameLogic.Types;

namespace Blackjack.GameLogic.Models;

public class Game
{
    public Game(List<Player> players, Guid id)
    {
        Players = players;
        Id = id;
    }
 
    public Guid Id { get; set; }
    public List<Player> Players { get; set; } // with db i need to change on userId`s
    public List<Card> Deck { get; set; } = new List<Card>();
    public int CurrentPlayerIndex { get; set; } = 0;
    public GameStatus Status { get; set; } = GameStatus.WaitingForPlayers;
    public int Bet { get; set; } = 100;
}
