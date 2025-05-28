using System.Text;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.Business.Helpers;

public static class CardConverter
{
    public static string CardToString(Card card)
    {
        return $"{(int)card.Suits}-{(int)card.Rank}";
    }
    
    public static string CardToString(IEnumerable<Card> cards)
    {
        var cardsString = new StringBuilder();

        foreach (var card in cards)
        {
            cardsString.Append($"{(int)card.Suits}-{(int)card.Rank} ");
        }
        
        return cardsString.ToString().TrimEnd();
    }
    
    public static Card StringToCard(string cardString)
    {
        var parts = cardString.Split('-');
        if (parts.Length != 2)
            throw new ArgumentException("Invalid card string format");
        
        if (!int.TryParse(parts[0], out int suitValue))
            throw new ArgumentException("Invalid suit value");

        if (!int.TryParse(parts[1], out int rankValue))
            throw new ArgumentException("Invalid rank value");

        var suit = (Suits)suitValue;
        var rank = (Rank)rankValue;

        return new Card(suit, rank);
    }

    public static List<Card> StringToCards(string cardsString)
    {
        return cardsString.Split(" ")
            .Select(StringToCard)
            .ToList();
    }
}