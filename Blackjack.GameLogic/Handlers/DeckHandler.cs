using Blackjack.GameLogic.Factories;
using Blackjack.Shared.Models;

namespace Blackjack.GameLogic.Handlers;

// i want one which will interact with deck, i think that deck cant to reset by itself
public class DeckHandler
{
    private readonly Deck _deck = new();
    private readonly Random _random = new();
    
    public void ResetDeck()
    {
        var newDeck = DeckFactory.NewDeck();
        _deck.Reset(newDeck);
    }

    public Card GetCard()
    {
        var randomIndex = _random.Next(_deck.GetLength());
        var card = _deck.GetCardByIndex(randomIndex);
        return card;
    }
}