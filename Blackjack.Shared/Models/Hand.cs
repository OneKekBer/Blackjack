using Blackjack.Shared.Helpers;

namespace Blackjack.Shared.Models;

public class Hand
{
    public List<Card> Cards { get; } = new List<Card>();
    
    public int GetScore() => HandHelper.GetScore(Cards);
    
    public void AddCard(Card card) => Cards.Add(card);
}