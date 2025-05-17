
using Blackjack.Shared.Models;
using Blackjack.Shared.Other;

namespace Blackjack.Shared.Helpers;

public static class HandHelper
{
    public static int GetScore(List<Card> cards)
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