namespace Blackjack.Shared.Models;

public class Game
{
    public Game(int numberOfPlayers, Deck deck)
    {
        NumberOfPlayers = numberOfPlayers;
        Deck = deck;
    }
 
    public Guid Id { get; set; } = Guid.NewGuid();
 
    public List<Player> Players { get; set; } = new List<Player>(); // with db i need to change on userId`s
     
    public Deck Deck { get; init; }
     
    public Queue<Guid> PlayersTurn { get; init; }
     
    public int NumberOfPlayers { get; init; } 
}