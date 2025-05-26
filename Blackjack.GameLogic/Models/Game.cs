namespace Blackjack.GameLogic.Models;

public enum GameStatus
{
    Started = 0,
    WaitingForPlayers = 1,
}

public class Game
{
    public Game(List<Player> players, List<Card> deck)
    {
        Players = players;
        Deck = deck;
    }
 
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<Player> Players { get; set; } // with db i need to change on userId`s
    public List<Card> Deck { get; set; }
    public int CurrentPlayerIndex { get; set; } = 0;
    public GameStatus Status { get; set; } = GameStatus.WaitingForPlayers;
    public int Bet { get; set; } = 100;
}