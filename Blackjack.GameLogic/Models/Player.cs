using Blackjack.GameLogic.Types;

namespace Blackjack.GameLogic.Models;



public class Player
{
    public Player(bool isPlaying,
        Role role,
        string name,
        int balance = 1000,
        Guid id = new Guid(),
        List<Card> cards = null)
    {
        Id = id;
        IsPlaying = isPlaying;
        Role = role;
        Name = name;
        Balance = balance;
        Cards = cards;
    }

    public Guid Id { get; }
    public bool IsPlaying { get; set; } = true; 
    public Role Role { get; }
    public string Name { get; }
    public int Balance { get; set; }
    public List<Card> Cards { get; }
}