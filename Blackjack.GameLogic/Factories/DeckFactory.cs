using Blackjack.Shared.Models;
using Blackjack.Shared.Types;

namespace Blackjack.GameLogic.Factories;

// factory which will be creating new full decks
public static class DeckFactory
{
    public static List<Card> NewDeck()
    {
        var deck = new List<Card>();
        var suitValues = Enum.GetValues(typeof(Suits));
        var rankValues = Enum.GetValues(typeof(Rank));
        
        foreach (var suit in suitValues)
        {
            Suits suitEnumVal = (Suits)Enum.Parse(typeof(Suits), suit.ToString());

            foreach (var rank in rankValues)
            {
                Rank rankEnumVal = (Rank)Enum.Parse(typeof(Rank), rank.ToString());
                deck.Add(new Card(suitEnumVal, rankEnumVal));
            }
        }
        
        return deck; 
    }
}