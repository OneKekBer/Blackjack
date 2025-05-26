using Blackjack.GameLogic.Models;

namespace Blackjack.ConsoleApp.View;

public class ViewPlayer
{
    public ViewPlayer(Guid id, List<Card> cards, string name, int balance)
    {
        Id = id;
        Cards = cards;
        Name = name;
        Balance = balance;
    }
    
    public Guid Id { get; set; }
    public List<Card> Cards { get; set; }
    public string Name { get; set; }
    public int Balance { get; set; }
}