using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Other;

namespace Blackjack.GameLogic.Extensions;

public static class CardsExtension
{
    public static int GetScore(this List<Card> cards)
    {
        var score = 0;
        var aceCounter = 0;
        
        foreach (var card in cards)
        {
            var stringRank = card.Rank.ToString();
            if (stringRank == "Ace") aceCounter += 1;
            score += Constants.RankValues[stringRank];
        }

        for (int i = 0; i < aceCounter; i++)
        {
            if (score > Constants.Limit) score -= 10;
        }
        
        return score;
    }
}