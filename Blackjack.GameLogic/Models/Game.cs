using System.Security.Cryptography.X509Certificates;

namespace Blackjack.GameLogic.Models;

public enum GameStatus
{
    Started = 0,
    WaitingForPlayers = 1,
}

public class Game
{
    public Game(
        List<Player> players,
        List<Card> deck,
        Guid id = new (),
        GameStatus status = GameStatus.WaitingForPlayers, 
        int bet = 100)
    {
        Players = players;
        Deck = deck;
        Id = id;
        Status = status;
        Bet = bet;
    }
 
    public Guid Id { get; set; }
    public List<Player> Players { get; set; } // with db i need to change on userId`s
    public List<Card> Deck { get; set; }
    public int CurrentPlayerIndex { get; set; } = 0;
    public GameStatus Status { get; set; }
    public int Bet { get; set; }
}
