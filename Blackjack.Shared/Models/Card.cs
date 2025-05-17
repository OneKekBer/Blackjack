using Blackjack.Shared.Types;

namespace Blackjack.Shared.Models;

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