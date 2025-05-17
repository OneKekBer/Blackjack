namespace Blackjack.Shared.Models;

public class Deck
{
    private List<Card> Cards { get; set; } = new List<Card>();

    public int GetLength() => Cards.Count;
    
    public Card GetCardByIndex(int index)
    {
        var card = Cards[index];
        Cards.Remove(card);
        
        return card;
    }
    
    //maybe ref value error
    public void Reset(List<Card> newDeck)
    {
        Cards = newDeck;
    }
}