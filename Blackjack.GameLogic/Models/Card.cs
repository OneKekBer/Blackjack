using Blackjack.GameLogic.Types;

namespace Blackjack.GameLogic.Models;

public class Card
{
    public Card(Suits suits, Rank rank)
    {
        Suits = suits;
        Rank = rank;
    }

    public Suits Suits { get; init; }
    
    public Rank Rank { get; init; }
}